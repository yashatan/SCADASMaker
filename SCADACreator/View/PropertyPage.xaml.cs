using SCADACreator.Model;
using SCADACreator.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for PropertyPage.xaml
    /// </summary>
    public partial class PropertyPage : Page
    {
        DesignerItem currentItem { get; set; }
        FrameworkElement content { get; set; }
        List<TagInfo> tagsList = DataProvider.Instance.DB.TagInfoes.ToList();
        public PropertyPage()
        {
            InitializeComponent();
            cbbTag.ItemsSource = tagsList;
        }

        public void SetCurrentItem(DesignerItem item)
        {
            currentItem = item;
            content = (FrameworkElement)item.Content;
            UpdateDataField();
        }

        private void UpdateDataField()
        {
            if (currentItem != null)
            {
                UpdatePositionGroupData();
                UpdateSizeGroupData();
                UpdateContentGroupData();
                UpdateColorGroupData();
                UpdateConectionGroupData();
            }
        }

        private void UpdateConectionGroupData()
        {
            if (content.GetType() == typeof(TextBox))
            {
                ConnectionGroup.Visibility = Visibility.Visible;
                cbbTag.SelectedItem = (currentItem as SCADAItem).TagConnection;
            }
            else
            {
                ConnectionGroup.Visibility = Visibility.Collapsed;
            }
        }
        private void UpdateColorGroupData()
        {
            if (content.GetType() == typeof(TextBlock) || content.GetType() == typeof(Button) || content.GetType() == typeof(TextBox) || content.GetType().IsSubclassOf(typeof(Shape)))
            {
                ColorGroup.Visibility = Visibility.Visible;
                if (content.GetType().IsSubclassOf(typeof(Shape)))
                {
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;

                    Brush b1 = (Brush)content.GetType().GetProperty("Fill").GetValue(content, null);
                    if (!(b1 is null))
                    {
                        Color c1 = ((SolidColorBrush)b1).Color;
                        var colorRGB = new ColorRGB(c1);

                        txtBackgroundR.Text = colorRGB.R.ToString();
                        txtBackgroundG.Text = colorRGB.G.ToString();
                        txtBackgroundB.Text = colorRGB.B.ToString();
                    }
                }
                else
                {
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorFillGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Visible;
                    ColorForegroundGroup.Visibility = Visibility.Visible;
                    ColorForegroundGroup.Visibility = Visibility.Visible;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;
                    ColorBackgroundGroup.Visibility = Visibility.Visible;

                    Brush b1 = (Brush)content.GetType().GetProperty("Background").GetValue(content, null);
                    if (!(b1 is null))
                    {
                        Color c1 = ((SolidColorBrush)b1).Color;
                        var colorRGB = new ColorRGB(c1);

                        txtBackgroundR.Text = colorRGB.R.ToString();
                        txtBackgroundG.Text = colorRGB.G.ToString();
                        txtBackgroundB.Text = colorRGB.B.ToString();
                    }

                    if (content.GetType() == typeof(Button))
                    {
                        Brush bf = ((content as Button).Content as TextBlock).Foreground;
                        if (!(bf is null))
                        {
                            Color c1 = ((SolidColorBrush)bf).Color;
                            var colorRGB = new ColorRGB(c1);

                            txtForegroundR.Text = colorRGB.R.ToString();
                            txtForegroundG.Text = colorRGB.G.ToString();
                            txtForegroundB.Text = colorRGB.B.ToString();
                        }
                    }
                    else
                    {
                        Brush bf = (Brush)content.GetType().GetProperty("Foreground").GetValue(content, null);
                        if (!(bf is null))
                        {
                            Color c1 = ((SolidColorBrush)bf).Color;
                            var colorRGB = new ColorRGB(c1);

                            txtForegroundR.Text = colorRGB.R.ToString();
                            txtForegroundG.Text = colorRGB.G.ToString();
                            txtForegroundB.Text = colorRGB.B.ToString();
                        }
                    }

                }
            }
            else
            {
                ColorGroup.Visibility = Visibility.Collapsed;
            }
        }
        void UpdatePositionGroupData()
        {
            txtPositionX.Text = Canvas.GetLeft(currentItem).ToString();
            txtPositionY.Text = Canvas.GetTop(currentItem).ToString();
        }
        void UpdateSizeGroupData()
        {
            txtSizeWidth.Text = currentItem.Width.ToString();
            txtSizeHeight.Text = currentItem.Height.ToString();
        }
        void UpdateContentGroupData()
        {
            if (content.GetType() == typeof(TextBlock) || content.GetType() == typeof(Button) || content.GetType() == typeof(TextBox))
            {
                ContentGroup.Visibility = Visibility.Visible;

                if (content.GetType() == typeof(Button))
                {
                    TextBlock txtContent;
                    txtContent = (TextBlock)(content as Button).Content;
                    txtContentText.IsEnabled = true;
                    txtContentText.Text = txtContent.Text;
                    txtContentSize.Text = txtContent.FontSize.ToString();
                }
                else if (content.GetType() == typeof(TextBlock))
                {
                    TextBlock txtContent;
                    txtContent = (content as TextBlock);
                    txtContentText.IsEnabled = true;
                    txtContentText.Text = txtContent.Text;
                    txtContentSize.Text = txtContent.FontSize.ToString();
                }
                else if (content.GetType() == typeof(TextBox))
                {
                    TextBox txtContent;
                    txtContent = (content as TextBox);
                    txtContentText.IsEnabled = false;
                    txtContentSize.Text = txtContent.FontSize.ToString();
                }

            }
            else
            {
                ContentGroup.Visibility = Visibility.Collapsed;
            }
        }

        private void txtContentText_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBlock txtContent = new TextBlock();
            if (content.GetType() == typeof(Button))
            {
                txtContent = (TextBlock)(content as Button).Content;
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                txtContent = (content as TextBlock);

            }
            txtContent.Text = txtContentText.Text;
        }

        private void txtContentSize_LostFocus(object sender, RoutedEventArgs e)
        {

            if (content.GetType() == typeof(Button))
            {
                TextBlock txtContent;
                txtContent = (TextBlock)(content as Button).Content;
                txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                TextBlock txtContent;
                txtContent = (content as TextBlock);
                txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
            }
            else if (content.GetType() == typeof(TextBox))
            {
                TextBox txtContent;
                txtContent = (content as TextBox);
                try
                {
                    txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
                }
                catch { }
            }

        }

        private void txtForeground_LostFocus(object sender, RoutedEventArgs e)
        {
            Brush b1 = (Brush)content.GetType().GetProperty("Foreground").GetValue(content, null);
            Color c1 = new Color();
            if (!(b1 is null))
            {
                c1 = ((SolidColorBrush)b1).Color;
            }
            ColorRGB newColor = new ColorRGB(c1);
            try
            {
                newColor.R = Convert.ToInt16(txtForegroundR.Text);
                newColor.G = Convert.ToInt16(txtForegroundG.Text);
                newColor.B = Convert.ToInt16(txtForegroundB.Text);
            }
            catch { }
            content.GetType().GetProperty("Foreground").SetValue(content, new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B)));
        }
        private void txtBackground_LostFocus(object sender, RoutedEventArgs e)
        {
            if (content.GetType().IsSubclassOf(typeof(Shape))) {
                Brush b1 = (Brush)content.GetType().GetProperty("Fill").GetValue(content, null);
                Color c1 = new Color();
                if (!(b1 is null))
                {
                    c1 = ((SolidColorBrush)b1).Color;
                }
                ColorRGB newColor = new ColorRGB(c1);
                try
                {
                    newColor.R = Convert.ToInt16(txtBackgroundR.Text);
                    newColor.G = Convert.ToInt16(txtBackgroundG.Text);
                    newColor.B = Convert.ToInt16(txtBackgroundB.Text);
                }
                catch { }
                content.GetType().GetProperty("Fill").SetValue(content, new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B)));
            }
            else
            {
                Brush b1 = (Brush)content.GetType().GetProperty("Background").GetValue(content, null);
                Color c1 = new Color();
                if (!(b1 is null))
                {
                    c1 = ((SolidColorBrush)b1).Color;
                }
                ColorRGB newColor = new ColorRGB(c1);
                try
                {
                    newColor.R = Convert.ToInt16(txtBackgroundR.Text);
                    newColor.G = Convert.ToInt16(txtBackgroundG.Text);
                    newColor.B = Convert.ToInt16(txtBackgroundB.Text);
                }
                catch { }
                content.GetType().GetProperty("Background").SetValue(content, new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B)));
            }

        }
        private void txtFill_LostFocus(object sender, RoutedEventArgs e)
        {
            Brush b1 = (Brush)content.GetType().GetProperty("Fill").GetValue(content, null);
            Color c1 = new Color();
            if (!(b1 is null))
            {
                c1 = ((SolidColorBrush)b1).Color;
            }
            ColorRGB newColor = new ColorRGB(c1);
            try
            {
                newColor.R = Convert.ToInt16(txtFillR.Text);
                newColor.G = Convert.ToInt16(txtFillG.Text);
                newColor.B = Convert.ToInt16(txtFillB.Text);
            }
            catch { }
            (content as Shape).Fill = new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B));
        }


        private void cbbTag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (currentItem as SCADAItem).TagConnection = (cbbTag.SelectedItem as TagInfo);
        }
    }
}
