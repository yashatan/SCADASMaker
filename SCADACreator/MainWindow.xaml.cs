﻿
using System;
using System.Collections;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SCADACreator.Utility;
using SCADACreator.View;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using SCADACreator.Model;
using InTheHand.Net.Sockets;
using Microsoft.Win32;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Xml.Linq;
using System.Windows.Markup;
using System.Data.SqlTypes;
using SCADACreator.View.Alarm;


namespace SCADACreator
{
    public partial class MainWindow : Window
    {
        public PropertyPage propertyPage;
        public AnimationListPage animationListPage;
        public ItemEventListPage itemEventListPage;
        private ProjectInformation currentprojectInformation;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            currentprojectInformation = new ProjectInformation();
            Title = $"SCADA Creator - {currentprojectInformation.Name}";
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (propertyPage == null) propertyPage = new PropertyPage();
            PropertyFrame.Content = propertyPage;
            if (animationListPage == null) animationListPage = new AnimationListPage();
            AnimationFrame.Content = animationListPage;
            if (itemEventListPage == null) itemEventListPage = new ItemEventListPage();
            ItemEventFrame.Content = itemEventListPage;
        }

        private void AddControButton_Click(object sender, RoutedEventArgs e)
        {
            SCADAItem Item1 = new SCADAItem();

            Image imageMotor1 = new Image();
            imageMotor1.MinWidth = 50;
            imageMotor1.MinHeight = 50;
            imageMotor1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            imageMotor1.IsHitTestVisible = false;
            Item1.Content = imageMotor1;
            setItemsize(Item1, imageMotor1);
            AnimationSense animation1 = new AnimationSense();
            animation1.Name = "testanmiationList1";
            animation1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation1.Tagvaluemax = 0;
            animation1.Tagvaluemin = 0;
            animation1.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation1.PropertyBoolValueWhenTagInRange = false;

            AnimationSense animation1_2 = new AnimationSense();
            animation1_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation1_2.Name = "testanmiationList1_2";
            animation1_2.Tagvaluemax = 1;
            animation1_2.Tagvaluemin = 1;
            animation1_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation1_2.PropertyBoolValueWhenTagInRange = true;
            Item1.AnimationSenses.Add(animation1);
            Item1.AnimationSenses.Add(animation1_2);

            Canvas.SetLeft(Item1, 50);
            Canvas.SetTop(Item1, 50);

            SCADAItem Item2 = new SCADAItem();
            Image imageMotor2 = new Image();
            imageMotor2.MinWidth = 50;
            imageMotor2.MinHeight = 50;
            imageMotor2.Source = new BitmapImage(new Uri("../Images/pump_side_grey.gif", UriKind.Relative));
            imageMotor2.IsHitTestVisible = false;
            Item2.Content = imageMotor2;
            setItemsize(Item2, imageMotor2);
            AnimationSense animation2 = new AnimationSense();
            animation2.Name = "testanmiationList2";
            animation2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation2.Tagvaluemax = 0;
            animation2.Tagvaluemin = 0;
            animation2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation2.PropertyBoolValueWhenTagInRange = true;
            AnimationSense animation2_2 = new AnimationSense();
            animation2_2.Name = "testanmiationList2_2";
            animation2_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.MemoryAddress == "DB1.DBX2.4").SingleOrDefault();
            animation2_2.Tagvaluemax = 1;
            animation2_2.Tagvaluemin = 1;
            animation2_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation2_2.PropertyBoolValueWhenTagInRange = false;
            Item2.AnimationSenses.Add(animation2);
            Item2.AnimationSenses.Add(animation2_2);

            Canvas.SetLeft(Item2, 50);
            Canvas.SetTop(Item2, 50);
            SCADAItem Item3 = new SCADAItem();
            Image imageMotor3 = new Image();
            imageMotor3.MinWidth = 50;
            imageMotor3.MinHeight = 50;
            imageMotor3.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            imageMotor3.IsHitTestVisible = false;
            Item3.Content = imageMotor3;
            setItemsize(Item3, imageMotor3);
            AnimationSense animation3 = new AnimationSense();
            animation3.Name = "testanmiationList3";
            animation3.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation3.Tagvaluemax = 0;
            animation3.Tagvaluemin = 0;
            animation3.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation3.PropertyBoolValueWhenTagInRange = false;
            AnimationSense animation3_2 = new AnimationSense();
            animation3_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation3_2.Name = "testanmiationList3_2";
            animation3_2.Tagvaluemax = 1;
            animation3_2.Tagvaluemin = 1;
            animation3_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation3_2.PropertyBoolValueWhenTagInRange = true;
            Item3.AnimationSenses.Add(animation3);
            Item3.AnimationSenses.Add(animation3_2);

            Canvas.SetLeft(Item3, 200);
            Canvas.SetTop(Item3, 50);
            SCADAItem Item4 = new SCADAItem();
            Image imageMotor4 = new Image();
            imageMotor4.MinWidth = 50;
            imageMotor4.MinHeight = 50;
            imageMotor4.Source = new BitmapImage(new Uri("../Images/pump_side_grey.gif", UriKind.Relative));
            imageMotor4.IsHitTestVisible = false;
            Item4.Content = imageMotor4;
            setItemsize(Item4, imageMotor4);
            AnimationSense animation4 = new AnimationSense();
            animation4.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation4.Name = "testanmiationList4";
            animation4.Tagvaluemax = 0;
            animation4.Tagvaluemin = 0;
            animation4.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation4.PropertyBoolValueWhenTagInRange = true;
            AnimationSense animation4_2 = new AnimationSense();
            animation4_2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Status").SingleOrDefault();
            animation4_2.Name = "testanmiationList4_2";
            animation4_2.Tagvaluemax = 1;
            animation4_2.Tagvaluemin = 1;
            animation4_2.PropertyNeedChange = AnimationSense.PropertyType.emIsVisible;
            animation4_2.PropertyBoolValueWhenTagInRange = false;
            Item4.AnimationSenses.Add(animation4);
            Item4.AnimationSenses.Add(animation4_2);

            Canvas.SetLeft(Item4, 200);
            Canvas.SetTop(Item4, 50);

            SCADAItem Item5 = new SCADAItem();
            TextBox label1 = new TextBox();
            label1.MinWidth = 50;
            label1.MinHeight = 50;
            label1.Text = "Text";
            label1.IsHitTestVisible = false;
            Item5.Width = 50;
            Item5.Height = 60;

            Item5.Content = label1;
            Item5.TagConnection = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Level_value").SingleOrDefault();
            //setItemsize(Item4, imageMotor4);
            Canvas.SetLeft(Item5, 150);
            Canvas.SetTop(Item5, 180);

            SCADAItem Item6 = new SCADAItem();
            Button button1 = new Button();
            button1.MinWidth = 50;
            button1.MinHeight = 50;
            var button1content = new TextBlock();
            button1content.Text = "start";
            button1.Content = button1content;
            button1.IsHitTestVisible = false;
            var btn1Content = new TextBlock();
            btn1Content.Text = "Start";
            button1.Content = btn1Content;
            Item6.Content = button1;
            setItemsize(Item6, button1);
            ItemEvent item1event1 = new ItemEvent();
            item1event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Start").SingleOrDefault();
            item1event1.Name = "Event 1";
            item1event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item1event1.EventType = ItemEvent.ItemEventType.emPress;

            ItemEvent item1event2 = new ItemEvent();
            item1event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Start").SingleOrDefault();
            item1event2.Name = "Event 2";
            item1event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item1event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item6.ItemEvents.Add(item1event1);
            Item6.ItemEvents.Add(item1event2);

            Canvas.SetLeft(Item6, 50);
            Canvas.SetTop(Item6, 250);

            SCADAItem Item7 = new SCADAItem();
            Button button2 = new Button();
            button2.MinWidth = 50;
            button2.MinHeight = 50;
            button2.IsHitTestVisible = false;
            var btn2Content = new TextBlock();
            btn2Content.Text = "Stop";
            button2.Content = btn2Content;
            Item7.Content = button2;
            setItemsize(Item7, button2);
            ItemEvent item2event1 = new ItemEvent();
            item2event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Stop").SingleOrDefault();
            item2event1.Name = "Event 1";
            item2event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item2event1.EventType = ItemEvent.ItemEventType.emPress;

            ItemEvent item2event2 = new ItemEvent();
            item2event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_1_Stop").SingleOrDefault();
            item2event2.Name = "Event 2";
            item2event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item2event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item7.ItemEvents.Add(item2event1);
            Item7.ItemEvents.Add(item2event2);

            Canvas.SetLeft(Item7, 50);
            Canvas.SetTop(Item7, 350);

            SCADAItem Item8 = new SCADAItem();
            Button button3 = new Button();
            button3.MinWidth = 50;
            button3.MinHeight = 50;
            button3.IsHitTestVisible = false;
            var btn3Content = new TextBlock();
            btn3Content.Text = "Start";
            button3.Content = btn3Content;
            Item8.Content = button3;
            setItemsize(Item8, button3);
            ItemEvent item3event1 = new ItemEvent();
            item3event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Start").SingleOrDefault();
            item3event1.Name = "Event 1";
            item3event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item3event1.EventType = ItemEvent.ItemEventType.emPress;

            ItemEvent item3event2 = new ItemEvent();
            item3event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Start").SingleOrDefault();
            item3event2.Name = "Event 2";
            item3event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item3event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item8.ItemEvents.Add(item3event1);
            Item8.ItemEvents.Add(item3event2);

            Canvas.SetLeft(Item8, 220);
            Canvas.SetTop(Item8, 250);

            SCADAItem Item9 = new SCADAItem();
            Button button4 = new Button();
            button4.MinWidth = 50;
            button4.MinHeight = 50;
            button4.IsHitTestVisible = false;
            var btn4Content = new TextBlock();
            btn4Content.Text = "Stop";
            button4.Content = btn4Content;
            Item9.Content = button4;
            setItemsize(Item9, button2);
            ItemEvent item4event1 = new ItemEvent();
            item4event1.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Stop").SingleOrDefault();
            item4event1.Name = "Event 1";
            item4event1.ActionType = ItemEvent.ItemActiontype.emSetbit;
            item4event1.EventType = ItemEvent.ItemEventType.emPress;

            ItemEvent item4event2 = new ItemEvent();
            item4event2.Tag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Name == "Motor_2_Stop").SingleOrDefault();
            item4event2.Name = "Event 2";
            item4event2.ActionType = ItemEvent.ItemActiontype.emResetBit;
            item4event2.EventType = ItemEvent.ItemEventType.emRelease;
            Item9.ItemEvents.Add(item4event1);
            Item9.ItemEvents.Add(item4event2);

            Canvas.SetLeft(Item9, 220);
            Canvas.SetTop(Item9, 350);
            //SCADAItem Item8 = new SCADAItem();
            //Ellipse ellipse1 = new Ellipse();
            //ellipse1.MinWidth = 50;
            //ellipse1.MinHeight = 50;
            ////ellipse1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            //ellipse1.IsHitTestVisible = false;
            //ellipse1.Fill = System.Windows.Media.Brushes.Red;
            //Item8.Content = ellipse1;
            //setItemsize(Item8, ellipse1);
            //AnimationSense animatione1 = new AnimationSense();
            //animatione1.Tagname = "DB1.DBX2.4";
            //animatione1.Tagvaluemax = 0;
            //animatione1.Tagvaluemin = 0;
            //animatione1.PropertyNeedChange = AnimationSense.PropertyType.emBackgroundcColor;
            //animatione1.ColorWhenTagInRange = new ColorRGB(((SolidColorBrush)System.Windows.Media.Brushes.Red).Color);

            //AnimationSense animatione1_2 = new AnimationSense();
            //animatione1_2.Tagname = "DB1.DBX2.4";
            //animatione1_2.Tagvaluemax = 1;
            //animatione1_2.Tagvaluemin = 1;
            //animatione1_2.PropertyNeedChange = AnimationSense.PropertyType.emBackgroundcColor;
            //animatione1_2.ColorWhenTagInRange = new ColorRGB(((SolidColorBrush)System.Windows.Media.Brushes.Blue).Color);
            //Item8.animationSenses.Add(animatione1);
            //Item8.animationSenses.Add(animatione1_2);
            //Canvas.SetLeft(Item8, 350);
            //Canvas.SetTop(Item8, 350);

            //SCADAItem Item9 = new SCADAItem();
            //Line line1 = new Line();
            //line1.MinWidth = 50;
            //line1.MinHeight = 50;
            ////ellipse1.Source = new BitmapImage(new Uri("../Images/pump_side_green.gif", UriKind.Relative));
            //line1.IsHitTestVisible = false;
            //line1.Stroke = System.Windows.Media.Brushes.Red;
            //line1.X1 = 0;
            //line1.Y1 = 0;
            //line1.X2 = 50;
            //line1.Y2 = 50;
            //Item9.Content = line1;
            //setItemsize(Item9, line1);
            //Canvas.SetLeft(Item9, 350);
            //Canvas.SetTop(Item9, 50);

            //TextBox 
            //    entry   
            MyDesignerCanvas.Children.Add(Item2);
            MyDesignerCanvas.Children.Add(Item1);
            MyDesignerCanvas.Children.Add(Item4);
            MyDesignerCanvas.Children.Add(Item3);
            MyDesignerCanvas.Children.Add(Item5);
            MyDesignerCanvas.Children.Add(Item6);
            MyDesignerCanvas.Children.Add(Item7);
            MyDesignerCanvas.Children.Add(Item8);
            MyDesignerCanvas.Children.Add(Item9);
        }

        public void SetCurrentItemForPage(DesignerItem designerItem)
        {
            propertyPage.SetCurrentItem(designerItem);
            animationListPage.SetCurrentItem(designerItem);
            itemEventListPage.SetCurrentItem(designerItem);
        }

        void setItemsize(DesignerItem item, FrameworkElement content)
        {
            if (content.MinHeight != 0 && content.MinWidth != 0)
            {
                item.Width = content.MinWidth * 2; ;
                item.Height = content.MinHeight * 2;
            }
            else
            {
                item.Width = 65;
                item.Height = 65;
            }
        }



        private string SerializeAllDesignerItems()
        {
            var options = new JsonSerializerOptions
            {
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };
            XElement serializedItems = new XElement("SCADAProject",
                 new XElement("TagInfos", JsonSerializer.Serialize(SCADADataProvider.Instance.TagInfos, options)),
                 new XElement("ConnectDevices", JsonSerializer.Serialize(SCADADataProvider.Instance.ConnectDevices, options)),
                 new XElement("AlarmSettings", JsonSerializer.Serialize(SCADADataProvider.Instance.AlarmSettingList, options)),
                 from item in MyDesignerCanvas.Children.OfType<SCADAItem>()
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

        private void DeSerrializeAllDesignerItems(string openstring)
        {
            XElement parsedElement = XElement.Parse(openstring);
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
                Canvas.SetLeft(pasteItem, (double)itemElement.Element("Left"));
                Canvas.SetTop(pasteItem, (double)itemElement.Element("Top"));
                MyDesignerCanvas.Children.Add(pasteItem);
            }

            List<TagInfo> taginfos = JsonSerializer.Deserialize<List<TagInfo>>((string)parsedElement.Element("TagInfos"));
            List<ConnectDevice> connectedDevices = JsonSerializer.Deserialize<List<ConnectDevice>>((string)parsedElement.Element("ConnectDevices"));
            List<AlarmSetting> alarmSettings = JsonSerializer.Deserialize<List<AlarmSetting>>((string)parsedElement.Element("AlarmSettings"));

            SCADADataProvider.Instance.AddListTagInfos(taginfos);
            SCADADataProvider.Instance.AddListConnectDevices(connectedDevices);
            SCADADataProvider.Instance.AddListAlarmSettingList(alarmSettings);
            //SCADADataProvider.Instance.AddDummyListAlarmPointList();//Need update
        }

        private void ClearCurrentProject()
        {
            MyDesignerCanvas.Children.Clear();
            SCADADataProvider.Instance.Clear();
            currentprojectInformation = new ProjectInformation();
        }
        private void SaveProject()
        {
            var savestring = SerializeAllDesignerItems();
            if (currentprojectInformation.IsNewProject)
            {
                currentprojectInformation.IsNewProject = false;
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = "untitled";
                saveFileDialog.Filter = "JsonString (*.json)|*.json";
                saveFileDialog.ShowDialog();
                currentprojectInformation.FilePath = saveFileDialog.FileName;
                Title = currentprojectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
            }
            File.WriteAllText(currentprojectInformation.FilePath, savestring);//Save Json string to file
        }


        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                SaveProject();
            }
        }

        #region MenuEvent
        private void MenuItemNewSCADA_Click(object sender, RoutedEventArgs e)
        {
            ClearCurrentProject();
            Title = $"SCADA Creator - {currentprojectInformation.Name}";
        }
        private void MenuItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            var savestring = SerializeAllDesignerItems();
            currentprojectInformation.IsNewProject = false;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "untitled";
            saveFileDialog.Filter = "JsonString (*.json)|*.json";
            saveFileDialog.ShowDialog();
            currentprojectInformation.FilePath = saveFileDialog.FileName;
            Title = currentprojectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
            File.WriteAllText(currentprojectInformation.FilePath, savestring);//Save Json string to file
        }
        private void MenuItemExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MenuItemSave_Click(object sender, RoutedEventArgs e)
        {
            SaveProject();
        }
        private void MenuItemOpen_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                string jsonString = File.ReadAllText(openFileDialog.FileName);
                ClearCurrentProject();
                try
                {
                    DeSerrializeAllDesignerItems(jsonString);
                    this.Title = currentprojectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    currentprojectInformation.IsNewProject = false;
                    currentprojectInformation.FilePath = openFileDialog.FileName;
                    currentprojectInformation.Name = System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("Cannot Open Project File");
                }
            }
        }
        private void MenuItemTags_Click(object sender, RoutedEventArgs e)
        {
            TagListWindow Tagwindow = new TagListWindow();
            Tagwindow.Show();
        }
        private void MenuItemDevices_Click(object sender, RoutedEventArgs e)
        {
            DeviceListWindow devicewindow = new DeviceListWindow();
            devicewindow.Show();
        }
        private void MenuItemStartSCADA_Click(object sender, RoutedEventArgs e)
        {
            BluetoothSendingView controlPropertyView = new BluetoothSendingView(MyDesignerCanvas, currentprojectInformation);
            controlPropertyView.Show();
        }
        #endregion

        #region ToolBar Event
        private void OnClickBringToFront(object sender, RoutedEventArgs e)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                int maxZ = int.MinValue;
                foreach (DesignerItem child in MyDesignerCanvas.Children)
                {
                    if (child != item)
                    {
                        maxZ = Math.Max(maxZ, Canvas.GetZIndex(child));
                    }
                }
                Canvas.SetZIndex(item, maxZ + 1);
            }
        }
        private void OnClickSendToBack(object sender, RoutedEventArgs e)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                int maxZ = int.MaxValue;
                foreach (DesignerItem child in MyDesignerCanvas.Children)
                {
                    if (child != item)
                    {
                        maxZ = Math.Min(maxZ, Canvas.GetZIndex(child));
                    }
                }
                Canvas.SetZIndex(item, maxZ - 1);
            }
        }
        private void OnClickRotateLeft(object sender, RoutedEventArgs args)
        {
            Rotate(-90);
        }

        private void OnClickRotateRight(object sender, RoutedEventArgs args)
        {
            Rotate(90);
        }
        private void Rotate(double angle)
        {
            foreach (DesignerItem item in MyDesignerCanvas.SelectedItems)
            {
                FrameworkElement element = item.Content as FrameworkElement;
                if (element != null)
                {
                    RotateTransform rotateTransform = element.LayoutTransform as RotateTransform;
                    if (rotateTransform == null)
                    {
                        rotateTransform = new RotateTransform();
                        element.LayoutTransform = rotateTransform;
                    }

                    rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;
                    Canvas.SetLeft(item, Canvas.GetLeft(item) - (item.Height - item.Width) / 2);
                    Canvas.SetTop(item, Canvas.GetTop(item) - (item.Width - item.Height) / 2);
                    double width = item.Width;
                    item.Width = item.Height;
                    item.Height = width;
                }
            }
        }
        #endregion

        private void MenuItemAlarmSetting_Click(object sender, RoutedEventArgs e)
        {
            AlarmSettingWindow alarmListWindow = new AlarmSettingWindow();
            alarmListWindow.Show();
        }

        private void MenuItemTagSetting_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
