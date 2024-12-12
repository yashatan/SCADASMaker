using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for AlarmListWindow.xaml
    /// </summary>
    public partial class TagLoggingSettingWindow : Window
    {
        List<TagLoggingSetting> tagLoggingSettingList;
        public List<TagInfo> tags { get; set; }
        public TagLoggingSettingWindow()
        {
            InitializeComponent();
            tagLoggingSettingList = SCADADataProvider.Instance.TagLoggingSettings;
            tags = SCADADataProvider.Instance.TagInfos;
            taglogginglistview.ItemsSource = tagLoggingSettingList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            SCADADataProvider.Instance.AddTagLoggingSetting(new TagLoggingSetting());
            taglogginglistview.Items.Refresh();
        }

        private void btnDeleteTrendSetting_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            var chosenTrendSetting = button.DataContext as TagLoggingSetting;
            tagLoggingSettingList.Remove(chosenTrendSetting);
            taglogginglistview.Items.Refresh();

        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var tagLoggingSetting = comboBox.DataContext as TagLoggingSetting;
            if (tagLoggingSetting.Tag != null)
            {
                comboBox.SelectedItem = (comboBox.ItemsSource as List<TagInfo>).FirstOrDefault(m => m.Id == tagLoggingSetting.Tag.Id);
            }
        }
    }

    public class EnumToIndexCycleTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TagLoggingSetting.CycleType enumValue)
            {
                return (int)enumValue;
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return (TagLoggingSetting.CycleType)index;
            }
            return TagLoggingSetting.CycleType.em2Sec;
        }
    }
}
