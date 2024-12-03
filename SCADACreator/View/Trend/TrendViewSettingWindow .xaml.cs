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

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for AlarmListWindow.xaml
    /// </summary>
    public partial class TrendViewSettingWindow : Window
    {
        List<TrendViewSetting> trendSettingsList;
        TrendViewSetting newTrendViewSetting;
        public TrendViewSettingWindow()
        {
            InitializeComponent();
            trendSettingsList = SCADADataProvider.Instance.TrendViewSettings;
            trendlistview.ItemsSource = trendSettingsList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {

            newTrendViewSetting = new TrendViewSetting();
            TrendViewSettingDetailWindow trendViewDetail = new TrendViewSettingDetailWindow(newTrendViewSetting);
            trendViewDetail.ApplyEvent += TrendViewDetailAdd_ApplyEvent;
            trendViewDetail.ShowDialog();
        }

        private void TrendViewDetailAdd_ApplyEvent(object sender, EventArgs e)
        {
            trendSettingsList.Add(newTrendViewSetting);
            trendlistview.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenTrendSetting = button.DataContext as TrendViewSetting;
            trendSettingsList.Remove(chosenTrendSetting);
            trendlistview.Items.Refresh();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var trendviewSetting = trendlistview.SelectedItem as TrendViewSetting;
            TrendViewSettingDetailWindow trendViewDetail = new TrendViewSettingDetailWindow(trendviewSetting);
            trendViewDetail.ApplyEvent += TrendViewDetailEdit_ApplyEvent;
            trendViewDetail.ShowDialog();
        }

        private void TrendViewDetailEdit_ApplyEvent(object sender, EventArgs e)
        {
            trendlistview.Items.Refresh();
        }
    }

}
