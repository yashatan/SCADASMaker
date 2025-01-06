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
    /// Interaction logic for TablePageSettingDetailWindow.xaml
    /// </summary>
    public partial class TablePageSettingDetailWindow : Window
    {
        TablePage currentTablePageSetting;
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

        public List<TagInfo> tagInfos { get; set; }
        public TablePageSettingDetailWindow()
        {
            InitializeComponent();
            tagInfos = SCADADataProvider.Instance.TagInfos;
            currentTablePageSetting = new TablePage();
            UpdateViewData();
        }

        public TablePageSettingDetailWindow(TablePage tablePage)
        {
            InitializeComponent();
            tagInfos = SCADADataProvider.Instance.TagInfos;
            currentTablePageSetting = tablePage;
            MappingTag();
            UpdateViewData();
        }
        private void MappingTag()
        {
            for (int i = 0; i < currentTablePageSetting.Tags.Count; i++)
            {
                var foundtag = tagInfos.FirstOrDefault(p => p.Id == currentTablePageSetting.Tags[i].Id);
                if (foundtag != null)
                {
                    currentTablePageSetting.Tags[i] = foundtag;
                }
                else
                {
                    currentTablePageSetting.Tags.RemoveAt(i);
                }

            }
        }
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            currentTablePageSetting.Name = txtName.Text;
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();
        }

        private void UpdateViewData()
        {
            txtName.Text = currentTablePageSetting.Name;
            lvTags.ItemsSource = currentTablePageSetting.Tags;
            lvTags.Items.Refresh();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            TablePageTagListWindow tablePageTagListWindow = new TablePageTagListWindow(currentTablePageSetting.Tags);
            tablePageTagListWindow.Show();
            tablePageTagListWindow.ChosenEvent += TablePageTagListWindow_ChosenEvent;
        }

        private void TablePageTagListWindow_ChosenEvent(object sender, EventArgs e)
        {
            lvTags.Items.Refresh();
        }

        private void btnDeleteTag_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenDeleteTag = button.DataContext as TagInfo;
            if (chosenDeleteTag != null)
            {
                currentTablePageSetting.Tags.Remove(chosenDeleteTag);
                lvTags.Items.Refresh();
            }
        }
    }
}
