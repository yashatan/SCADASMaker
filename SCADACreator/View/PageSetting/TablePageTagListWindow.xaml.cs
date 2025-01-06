using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for TablePageTagListWindow.xaml
    /// </summary>
    public partial class TablePageTagListWindow : Window
    {
        List<TagInfo> chosenTags;
        List<TagInfo> tagsList;
        private event EventHandler _ChosenEvent;//event handle when confirm button clicked
        public event EventHandler ChosenEvent
        {
            add
            {
                _ChosenEvent += value;
            }
            remove
            {
                _ChosenEvent -= value;
            }
        }
        public TablePageTagListWindow()
        {
            InitializeComponent();
            chosenTags = new List<TagInfo>();
            tagsList = SCADADataProvider.Instance.TagInfos;
            lvTags.ItemsSource = tagsList;
            lvTags.Items.Refresh();
        }

        public TablePageTagListWindow(List<TagInfo> currenttags)
        {
            InitializeComponent();
            chosenTags = currenttags;
            tagsList = SCADADataProvider.Instance.TagInfos;
            lvTags.ItemsSource = tagsList;
            lvTags.Items.Refresh();
            lvTags.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;// for Setting color of card

        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (lvTags.ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated)
            {
                lvTags.ItemContainerGenerator.StatusChanged -= ItemContainerGenerator_StatusChanged;
                SetListViewItemColor();
            }
        }

        private void SetListViewItemColor()
        {
            foreach (TagInfo tag in chosenTags)
            {
                //var listViewItem = lvTags.ItemContainerGenerator.ContainerFromItem(tag) as ListViewItem;
                //var listViewItemIndex = lvTags.Items.IndexOf(tag);
                //if (lvTags.Items[listViewItemIndex] != null)
                //{ // Tìm control bên trong ItemTemplate (Card)
                //    var item = lvTags.Items[listViewItemIndex];
                //    var listViewItem = lvTags.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                //    var card = FindChild<Card>(listViewItem);
                //    if (card != null)
                //    {
                //        card.Background = Brushes.LightGreen;
                //    }
                //}

                var listViewItem = lvTags.ItemContainerGenerator.ContainerFromItem(tag) as ListViewItem;
                if (listViewItem != null)
                { // Tìm control bên trong ItemTemplate (Card)
                    var card = FindChild<Card>(listViewItem);
                    if (card != null)
                    {
                        card.Background = Brushes.LightGreen;
                    }
                }
            }

        }
        public static T FindChild<T>(DependencyObject parent) where T : DependencyObject
        {
            // Kiểm tra các nút con
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                // Nếu tìm thấy control với tên tương ứng, trả về control đó
                if (child is T element)
                    return child as T;

                // Đệ quy để tìm trong cây con
                var result = FindChild<T>(child);
                if (result != null)
                    return result;
            }
            return null;
        }
        private void SelectButton_Click(object sender, MouseButtonEventArgs e)
        {
            var button = sender as Card;
            var chosenTag = button.DataContext as TagInfo;
            if (chosenTag != null)
            {
                if (!chosenTags.Contains(chosenTag))
                {
                    chosenTags.Add(chosenTag);
                    button.Background = new SolidColorBrush((Color)Colors.LightGreen);
                }
                else
                {
                    chosenTags.Remove(chosenTag);
                    button.Background = new SolidColorBrush((Color)Colors.White);
                }
            }

            OnChosenEvent();
        }
        private void OnChosenEvent()
        {
            if (_ChosenEvent != null)
            {
                _ChosenEvent(this, new EventArgs());
            }
        }
    }
}
