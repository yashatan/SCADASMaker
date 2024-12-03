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

namespace SCADACreator.View.Alarm
{
    /// <summary>
    /// Interaction logic for AlarmListWindow.xaml
    /// </summary>
    public partial class AlarmSettingWindow : Window
    {
        List<AlarmSetting> alarmSettinglist;
        public List<TagInfo> tags { get; set; }
        public AlarmSettingWindow()
        {
            InitializeComponent();
            alarmSettinglist = SCADADataProvider.Instance.AlarmSettings;
            tags = SCADADataProvider.Instance.TagInfos;

            alarmlistview.ItemsSource = alarmSettinglist;
            alarmlistview.Items.Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            SCADADataProvider.Instance.AddAlarmSetting(new AlarmSetting
            {

            });
            alarmlistview.Items.Refresh();
        }

        private void btnDeleteAlarmSetting_Cliked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenAlarmSetting = button.DataContext as AlarmSetting;
            alarmSettinglist.Remove(chosenAlarmSetting);
            alarmlistview.Items.Refresh();
        }

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            var chosenAlarmSetting = comboBox.DataContext as AlarmSetting;
            comboBox.SelectedItem = (comboBox.ItemsSource as List<TagInfo>).FirstOrDefault(m => m.Id == chosenAlarmSetting.TriggerTag.Id);
        }
    }

    public class EnumToIndexTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AlarmSetting.AlarmType enumValue)
            {
                return (int)enumValue;
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return (AlarmSetting.AlarmType)index;
            }
            return AlarmSetting.AlarmType.Warning;
        }
    }

    public class EnumToIndexModeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AlarmSetting.LimitType enumValue)
            {
                return (int)enumValue;
            }
            return 0;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
            {
                return (AlarmSetting.LimitType)index;
            }
            return AlarmSetting.LimitType.Lower;
        }
    }
}
