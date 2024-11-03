using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Text.Json;
using System.Windows.Media;
using System.Windows.Shapes;
using SCADACreator.Utility;
using System.Xml;
using System.Xml.Linq;
using System.Runtime.Serialization;
using System.Runtime.InteropServices.ComTypes;
using System.Text.Json.Serialization;
using System.Data.SqlTypes;
using System.Xml.Serialization;

namespace SCADACreator
{
    public class DesignerCanvas : Canvas
    {
        private Point? dragStartPoint = null;
        private MainWindow _window;
        public DesignerCanvas()
        {
            Focusable = true;
            Loaded += OnCanvasGraphLoaded;
        }

        private void OnCanvasGraphLoaded(object sender, RoutedEventArgs e)
        {
            Focus();
            Loaded -= OnCanvasGraphLoaded;
            _window = AdvancedFindParent.FindAncestor<MainWindow>(this);
        }

        public IEnumerable<DesignerItem> SelectedItems
        {
            get
            {
                var selectedItems = from item in this.Children.OfType<DesignerItem>()
                                    where item.IsSelected == true
                                    select item;

                return selectedItems;
            }
        }

        public void DeselectAll()
        {
            foreach (DesignerItem item in this.SelectedItems)
            {
                item.IsSelected = false;
            }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                this.dragStartPoint = new Point?(e.GetPosition(this));
                this.DeselectAll();

                e.Handled = true;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            // DesignerCanvas designer = VisualTreeHelper.GetParent(this) as DesignerCanvas;
            if (this != null)
            {
                if (e.Key == Key.Delete)
                {
                    var list = new List<UIElement>();

                    foreach (DesignerItem uie in this.SelectedItems)
                    {
                        // if this element must be removed, then add it to list:  
                        list.Add(uie);
                    }

                    foreach (var uie in list) this.Children.Remove(uie);
                }
                if (e.Key == Key.C && (Keyboard.Modifiers & ModifierKeys.Control) ==ModifierKeys.Control)
                {
                    var xmlString = SerrializeControl();
                    Clipboard.SetText(xmlString);
                }
                if (e.Key == Key.X && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    var xmlString = SerrializeControl();
                    Clipboard.SetText(xmlString);
                    var list = new List<UIElement>();

                    foreach (DesignerItem uie in this.SelectedItems)
                    {
                        // if this element must be removed, then add it to list:  
                        list.Add(uie);
                    }

                    foreach (var uie in list) this.Children.Remove(uie);
                    Focus();
                }
                if (e.Key == Key.V && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                {
                    string xmlString = Clipboard.GetText();
                    XElement parsedElement = XElement.Parse(xmlString);
                    DeselectAll();
                    foreach (var itemElement in parsedElement.Elements("SCADAItem"))
                    {
                        SCADAItem pasteItem = new SCADAItem();
                        FrameworkElement content = XamlReader.Parse((string)itemElement.Element("Content")) as FrameworkElement;
                        pasteItem.Content = content;
                        pasteItem.Width = (double)itemElement.Element("Width");
                        pasteItem.Height = (double)itemElement.Element("Height");
                        pasteItem.TagConnection = JsonSerializer.Deserialize<TagInfo>((string)itemElement.Element("TagConnection"));
                        pasteItem.AnimationSenses = JsonSerializer.Deserialize<List<AnimationSense>>((string)itemElement.Element("AnimationSenses"));
                        pasteItem.ItemEvents = JsonSerializer.Deserialize<List<ItemEvent>>((string)itemElement.Element("ItemEvents"));
                        Canvas.SetLeft(pasteItem, (double)itemElement.Element("Left") + 10.0);
                        Canvas.SetTop(pasteItem, (double)itemElement.Element("Top") + 10.0);
                        pasteItem.IsSelected = true;
                        this.Children.Add(pasteItem);

                    }
                    var xmlStringnext = SerrializeControl();
                    Clipboard.SetText(xmlStringnext);
                }
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
            {
                this.dragStartPoint = null;
            }

            if (this.dragStartPoint.HasValue)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, this.dragStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }

                e.Handled = true;
            }
        }

        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            string xamlString = e.Data.GetData("DESIGNER_ITEM") as string;
            if (!String.IsNullOrEmpty(xamlString))
            {
                SCADAItem newItem = null;
                FrameworkElement content = XamlReader.Load(XmlReader.Create(new StringReader(xamlString))) as FrameworkElement;

                if (content != null)
                {
                    newItem = new SCADAItem();
                    newItem.Content = content;

                    Point position = e.GetPosition(this);
                    if (content.MinHeight != 0 && content.MinWidth != 0)
                    {
                        newItem.Width = content.MinWidth * 2; ;
                        newItem.Height = content.MinHeight * 2;
                    }
                    else
                    {
                        newItem.Width = 65;
                        newItem.Height = 65;
                    }
                    DesignerCanvas.SetLeft(newItem, Math.Max(0, position.X - newItem.Width / 2));
                    DesignerCanvas.SetTop(newItem, Math.Max(0, position.Y - newItem.Height / 2));
                    this.Children.Add(newItem);

                    this.DeselectAll();
                    newItem.IsSelected = true;
                }
                _window.SetCurrentItemForPage(newItem);
                e.Handled = true;
                Focus();
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;

                element.Measure(constraint);

                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }

            // add some extra margin
            size.Width += 10;
            size.Height += 10;
            return size;
        }

        private string SerrializeControl()
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            XElement serializedItems = new XElement("SCADAItems",
                 from item in this.SelectedItems
                 let contentXaml = XamlWriter.Save(((SCADAItem)item).Content)
                 let tagconnectionjson = JsonSerializer.Serialize(((SCADAItem)item).TagConnection, options)
                 let animationsensejson = JsonSerializer.Serialize(((SCADAItem)item).AnimationSenses, options)
                 let itemeventjson = JsonSerializer.Serialize(((SCADAItem)item).ItemEvents, options)
                 select new XElement("SCADAItem",
                             new XElement("Left", Canvas.GetLeft(item)),
                             new XElement("Top", Canvas.GetTop(item)),
                             new XElement("Width", item.Width),
                             new XElement("Height", item.Height),
                             new XElement("zIndex", Canvas.GetZIndex(item)),
                             new XElement("Content", contentXaml),
                             new XElement("AnimationSenses", animationsensejson),
                             new XElement("ItemEvents", itemeventjson),
                             new XElement("TagConnection", tagconnectionjson)
                             )
                  );
            return serializedItems.ToString();
        }
    }
}
