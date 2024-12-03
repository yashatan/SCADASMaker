using MaterialDesignThemes.Wpf;
using SCADACreator.Model;
using SCADACreator.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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
    public partial class PropertyPage : Page, INotifyPropertyChanged
    {
        DesignerItem currentItem { get; set; }
        FrameworkElement content { get; set; }
        List<TagInfo> tagsList = SCADADataProvider.Instance.TagInfos;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        // public bool HaveTargetItem { get; set; }
        private bool _HaveTargetItem = false;
        public bool HaveTargetItem
        {
            get
            {
                return _HaveTargetItem;
            }
            set
            {
                _HaveTargetItem = value; OnPropertyChanged(nameof(HaveTargetItem));
            }
        }
        public PropertyPage()
        {
            InitializeComponent();
            DataContext = this;
            cbbTag.ItemsSource = tagsList;
            HaveTargetItem = false;
        }

        public void SetCurrentItem(DesignerItem item)
        {
            currentItem = item;
            content = (FrameworkElement)item.Content;
            HaveTargetItem = true;
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
            if (content.GetType() == typeof(System.Windows.Controls.TextBox))
            {
                ConnectionGroup.Visibility = Visibility.Visible;
                cbbTag.SelectedItem = (currentItem as SCADAItem).TagConnection;
                if ((currentItem as SCADAItem).TagConnection != null)
                {
                    cbbTag.Text = (currentItem as SCADAItem).TagConnection.Name;
                }

                cbbTag.Items.Refresh();

            }
            else
            {
                ConnectionGroup.Visibility = Visibility.Collapsed;
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
            if (content.GetType() == typeof(TextBlock) || content.GetType() == typeof(System.Windows.Controls.Button) || content.GetType() == typeof(System.Windows.Controls.TextBox))
            {
                ContentGroup.Visibility = Visibility.Visible;

                if (content.GetType() == typeof(System.Windows.Controls.Button))
                {
                    TextBlock txtContent;
                    txtContent = (TextBlock)(content as System.Windows.Controls.Button).Content;
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
                else if (content.GetType() == typeof(System.Windows.Controls.TextBox))
                {
                    System.Windows.Controls.TextBox txtContent;
                    txtContent = (content as System.Windows.Controls.TextBox);
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
            if (content.GetType() == typeof(System.Windows.Controls.Button))
            {
                txtContent = (TextBlock)(content as System.Windows.Controls.Button).Content;
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                txtContent = (content as TextBlock);

            }
            txtContent.Text = txtContentText.Text;
        }

        private void txtContentSize_LostFocus(object sender, RoutedEventArgs e)
        {

            if (content.GetType() == typeof(System.Windows.Controls.Button))
            {
                TextBlock txtContent;
                txtContent = (TextBlock)(content as System.Windows.Controls.Button).Content;
                txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                TextBlock txtContent;
                txtContent = (content as TextBlock);
                txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
            }
            else if (content.GetType() == typeof(System.Windows.Controls.TextBox))
            {
                System.Windows.Controls.TextBox txtContent;
                txtContent = (content as System.Windows.Controls.TextBox);
                try
                {
                    txtContent.FontSize = Convert.ToDouble(txtContentSize.Text);
                }
                catch { }
            }

        }

        private void cbbTag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (currentItem as SCADAItem).TagConnection = (cbbTag.SelectedItem as TagInfo);
        }

        private void txtPositionX_LostFocus(object sender, RoutedEventArgs e)
        {
            Canvas.SetLeft(currentItem, Convert.ToDouble(txtPositionX.Text));
        }

        private void txtContentText_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            TextBlock txtContent = new TextBlock();
            if (content.GetType() == typeof(System.Windows.Controls.Button))
            {
                txtContent = (TextBlock)(content as System.Windows.Controls.Button).Content;
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                txtContent = (content as TextBlock);

            }
            txtContent.Text = txtContentText.Text;
        }

        private void txtContentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBlock txtContent = new TextBlock();
            if (content.GetType() == typeof(System.Windows.Controls.Button))
            {
                txtContent = (TextBlock)(content as System.Windows.Controls.Button).Content;
            }
            else if (content.GetType() == typeof(TextBlock))
            {
                txtContent = (content as TextBlock);

            }
            txtContent.Text = txtContentText.Text;
        }

        private void txtPositionY_LostFocus(object sender, RoutedEventArgs e)
        {
            Canvas.SetTop(currentItem, Convert.ToDouble(txtPositionY.Text));

        }

        private void txtSizeWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            currentItem.Width = Convert.ToDouble(txtSizeWidth.Text);
        }

        private void txtSizeHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            currentItem.Height = Convert.ToDouble(txtSizeHeight.Text);
        }



        #region ColorGroup
        private void UpdateColorGroupData()
        {
            if (content.GetType() == typeof(TextBlock) || content.GetType() == typeof(System.Windows.Controls.Button) || content.GetType() == typeof(System.Windows.Controls.TextBox) || content.GetType().IsSubclassOf(typeof(Shape)))
            {
                ColorGroup.Visibility = Visibility.Visible;
                if (content.GetType().IsSubclassOf(typeof(Shape)))
                {
                    ColorFillGroup.Visibility = Visibility.Visible;
                    ColorFillGroup.Visibility = Visibility.Visible;
                    ColorFillGroup.Visibility = Visibility.Visible;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorForegroundGroup.Visibility = Visibility.Collapsed;
                    ColorBackgroundGroup.Visibility = Visibility.Collapsed;
                    ColorBackgroundGroup.Visibility = Visibility.Collapsed;
                    ColorBackgroundGroup.Visibility = Visibility.Collapsed;

                    Brush b1 = (Brush)content.GetType().GetProperty("Fill").GetValue(content, null);
                    if (!(b1 is null))
                    {
                        Color c1 = ((SolidColorBrush)b1).Color;
                        var colorRGB = new ColorRGB(c1);

                        txtFillR.Text = colorRGB.R.ToString();
                        txtFillG.Text = colorRGB.G.ToString();
                        txtFillB.Text = colorRGB.B.ToString();
                        btnFill.Background = b1;
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
                        btnBackground.Background = b1;
                    }

                    if (content.GetType() == typeof(System.Windows.Controls.Button))
                    {
                        Brush bf = ((content as System.Windows.Controls.Button).Content as TextBlock).Foreground;
                        if (!(bf is null))
                        {
                            Color c1 = ((SolidColorBrush)bf).Color;
                            var colorRGB = new ColorRGB(c1);

                            txtForegroundR.Text = colorRGB.R.ToString();
                            txtForegroundG.Text = colorRGB.G.ToString();
                            txtForegroundB.Text = colorRGB.B.ToString();
                            btnForeground.Background = bf;
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
                            btnForeground.Background = bf;
                        }
                    }

                }
            }
            else
            {
                ColorGroup.Visibility = Visibility.Collapsed;
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
                newColor.R = Convert.ToByte(txtForegroundR.Text);
                newColor.G = Convert.ToByte(txtForegroundG.Text);
                newColor.B = Convert.ToByte(txtForegroundB.Text);
            }
            catch { }
            Brush fg = new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B));
            content.GetType().GetProperty("Foreground").SetValue(content, fg);
            btnForeground.Background = fg;
        }
        private void txtBackground_LostFocus(object sender, RoutedEventArgs e)
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
                newColor.R = Convert.ToByte(txtBackgroundR.Text);
                newColor.G = Convert.ToByte(txtBackgroundG.Text);
                newColor.B = Convert.ToByte(txtBackgroundB.Text);
            }
            catch { }
            Brush bg = new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B));
            content.GetType().GetProperty("Background").SetValue(content, bg);
            btnBackground.Background = bg;
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
                newColor.R = Convert.ToByte(txtFillR.Text);
                newColor.G = Convert.ToByte(txtFillG.Text);
                newColor.B = Convert.ToByte(txtFillB.Text);
            }
            catch { }
            Brush bg = new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B));
            (content as Shape).Fill = bg;
            btnFill.Background = bg;
        }

        private void btnForeground_Clicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //(btnAdd.Content as PackIcon).Foreground = new SolidColorBrush(Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                Brush fg = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                btnForeground.Background = fg;
                content.GetType().GetProperty("Foreground").SetValue(content, fg);
                if (!(fg is null))
                {
                    Color c1 = ((SolidColorBrush)fg).Color;
                    var colorRGB = new ColorRGB(c1);

                    txtForegroundR.Text = colorRGB.R.ToString();
                    txtForegroundG.Text = colorRGB.G.ToString();
                    txtForegroundB.Text = colorRGB.B.ToString();
                }
            }
        }
        #endregion

        private void btnBackground_Clicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Brush bg = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                btnBackground.Background = bg;
                content.GetType().GetProperty("Background").SetValue(content, bg);
                if (!(bg is null))
                {
                    Color c1 = ((SolidColorBrush)bg).Color;
                    var colorRGB = new ColorRGB(c1);

                    txtBackgroundR.Text = colorRGB.R.ToString();
                    txtBackgroundG.Text = colorRGB.G.ToString();
                    txtBackgroundB.Text = colorRGB.B.ToString();
                }
            }
        }

        private void btnFill_Clicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Brush bg = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                btnFill.Background = bg;
                content.GetType().GetProperty("Fill").SetValue(content, bg);
                if (!(bg is null))
                {
                    Color c1 = ((SolidColorBrush)bg).Color;
                    var colorRGB = new ColorRGB(c1);

                    txtFillR.Text = colorRGB.R.ToString();
                    txtFillG.Text = colorRGB.G.ToString();
                    txtFillB.Text = colorRGB.B.ToString();
                }
            }
        }
    }
}
