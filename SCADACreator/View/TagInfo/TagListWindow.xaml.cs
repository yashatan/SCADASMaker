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
using static SCADACreator.View.TagInfoDetailWindow;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for TagListWindow.xaml
    /// </summary>
    public partial class TagListWindow : Window
    {
        TagInfo newTagInfo;
        private List<TagInfo> tagsList;
        public TagListWindow()
        {
            InitializeComponent();
            tagsList = DataProvider.Instance.DB.TagInfoes.ToList();
            TagList.ItemsSource = tagsList;
            TagList.Items.Refresh();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var tag = TagList.SelectedItem as TagInfo;
            TagInfoDetailWindow tagInfoDetailWindow = new TagInfoDetailWindow(tag);
            tagInfoDetailWindow.ApplyEvent += TagInfoDetail_ApplyEventEdit;
            tagInfoDetailWindow.ShowDialog();
        }

        private void TagInfoDetail_ApplyEventEdit(object sender, TagInfoEventArgs e)
        {
            var tagInfo = TagList.SelectedItem as TagInfo;
            var chosenTag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Id == e.TagInfo.Id).SingleOrDefault();
            chosenTag = e.TagInfo;
            DataProvider.Instance.DB.SaveChanges();
            TagList.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var taginfo = TagList.SelectedItem as TagInfo;
            var chosenTag = DataProvider.Instance.DB.TagInfoes.Where(x => x.Id == taginfo.Id).SingleOrDefault();
            DataProvider.Instance.DB.TagInfoes.Remove(chosenTag);
            DataProvider.Instance.DB.SaveChanges();
            tagsList.Remove(taginfo);
            TagList.Items.Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            newTagInfo = new TagInfo();
            TagInfoDetailWindow tagInfoDetailWindow = new TagInfoDetailWindow(newTagInfo);
            tagInfoDetailWindow.ApplyEvent += TagInfoDetail_ApplyEventNew;
            tagInfoDetailWindow.ShowDialog();
        }

        private void TagInfoDetail_ApplyEventNew(object sender, EventArgs e)
        {
            DataProvider.Instance.DB.TagInfoes.Add(newTagInfo);
            DataProvider.Instance.DB.SaveChanges();
            tagsList.Add(newTagInfo);
            TagList.Items.Refresh();
        }
    }
}
