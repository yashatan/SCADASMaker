using SCADACreator.Model;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for TablePageListSetting.xaml
    /// </summary>
    public partial class TablePageSettingWindow : Window
    {
        List<TablePage> tablePagesList;
        TablePage newTablePage;
        public TablePageSettingWindow()
        {
            InitializeComponent();
            tablePagesList = SCADADataProvider.Instance.TablePages;
            lvTablePage.ItemsSource = tablePagesList;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            newTablePage = new TablePage();
            TablePageSettingDetailWindow tablePageDetailWindow = new TablePageSettingDetailWindow(newTablePage);
            tablePageDetailWindow.ApplyEvent += TablePageDetailWindow_NewApplyEvent;
            tablePageDetailWindow.ShowDialog();
        }

        private void TablePageDetailWindow_NewApplyEvent(object sender, EventArgs e)
        {
            tablePagesList.Add(newTablePage);
            lvTablePage.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenTrendSetting = button.DataContext as TablePage;
            tablePagesList.Remove(chosenTrendSetting);
            lvTablePage.Items.Refresh();
        }
        
        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var tablePage = lvTablePage.SelectedItem as TablePage;
            TablePageSettingDetailWindow tablePageDetailWindow = new TablePageSettingDetailWindow(tablePage);
            tablePageDetailWindow.ApplyEvent += TablePageDetailWindow_EditApplyEvent; ;
            tablePageDetailWindow.ShowDialog();
        }

        private void TablePageDetailWindow_EditApplyEvent(object sender, EventArgs e)
        {
            lvTablePage.Items.Refresh();
        }
    }
}
