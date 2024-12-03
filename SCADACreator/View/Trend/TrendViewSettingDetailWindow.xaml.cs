using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for TrendViewSettingDetailWindow.xaml
    /// </summary>
    public partial class TrendViewSettingDetailWindow : Window
    {
        TrendViewSetting currentTrendViewSetting;
        private event EventHandler _ApplyEvent;//event handle when confirm button clicked
        public event EventHandler ApplyEvent
        {
            add
            {
                _ApplyEvent += value;
            }
            remove
            {
                _ApplyEvent -= value;
            }
        }

        public List<TagLoggingSetting> tagLoggingSetting {  get; set; }

        public TrendViewSettingDetailWindow()
        {
            InitializeComponent();
            tagLoggingSetting = SCADADataProvider.Instance.TagLoggingSettings;
            currentTrendViewSetting = new TrendViewSetting();
            UpdateViewData();
        }

        public TrendViewSettingDetailWindow(TrendViewSetting trendViewSetting)
        {
            InitializeComponent();
            currentTrendViewSetting = trendViewSetting;
            tagLoggingSetting = SCADADataProvider.Instance.TagLoggingSettings;
            UpdateViewData();
        }

        private void UpdateViewData()
        {
            txtName.Text = currentTrendViewSetting.Name;
            trendsettinglist.ItemsSource = currentTrendViewSetting.Trends;
            trendsettinglist.Items.Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            currentTrendViewSetting.Trends.Add(new TrendSetting(currentTrendViewSetting.Trends.Count));
            trendsettinglist.Items.Refresh();
        }

        private void btnDeleteTrendSetting_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var chosenTrendSetting = button.DataContext as TrendSetting;
            currentTrendViewSetting.Trends.Remove(chosenTrendSetting);
            trendsettinglist.Items.Refresh();
        }

        private void btnColor_Clicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var button = sender as System.Windows.Controls.Button;
                var chosenTrendSetting = button.DataContext as TrendSetting;
                Brush fg = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                chosenTrendSetting.ColorStyle.R = colorDialog.Color.R;
                chosenTrendSetting.ColorStyle.G = colorDialog.Color.G;
                chosenTrendSetting.ColorStyle.B = colorDialog.Color.B;
                button.Background = fg;
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            currentTrendViewSetting.Name = txtName.Text;
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as System.Windows.Controls.ComboBox;
            var trendSetting = comboBox.DataContext as TrendSetting;
            if (trendSetting != null)
            {
                if(trendSetting.TagLogging != null)
                {
                    comboBox.SelectedItem = (comboBox.ItemsSource as List<TagLoggingSetting>).FirstOrDefault(m => m.Id == trendSetting.TagLogging.Id);

                }
            }
        }
    }

    public class ColorRGBToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorRGB colorRGB)
            {
                return new SolidColorBrush(Color.FromRgb((byte)colorRGB.R, (byte)colorRGB.G, (byte)colorRGB.B));
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is SolidColorBrush colorBrush)
            {
                return new ColorRGB(colorBrush.Color);
            }
            return AlarmSetting.LimitType.Lower;
        }
    }
}
