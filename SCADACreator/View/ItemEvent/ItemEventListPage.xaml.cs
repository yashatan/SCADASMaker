using SCADACreator.Utility;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for ItemEventListPage.xaml
    /// </summary>
    public partial class ItemEventListPage : Page
    {
        List<ItemEvent> itemEvents;
        SCADAItem currentItem;
        ItemEvent newItemEvent;
        public ItemEventListPage()
        {
            InitializeComponent();
        }

        public void SetCurrentItem(DesignerItem item)
        {
            currentItem = (item as SCADAItem);
            itemEvents = currentItem.ItemEvents;
            ItemEventList.ItemsSource = itemEvents;
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var itemevent = ItemEventList.SelectedItem as ItemEvent;
            ItemEventDetailWindow itemEventDetail = new ItemEventDetailWindow(itemevent);
            itemEventDetail.ApplyEvent += itemEventDetail_ApplyEventEdit;
            itemEventDetail.ShowDialog();
        }

        private void itemEventDetail_ApplyEventEdit(object sender, EventArgs e)
        {
            ItemEventList.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var itemevent = ItemEventList.SelectedItem as ItemEvent;
            currentItem.ItemEvents.Remove(itemevent);
            ItemEventList.Items.Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            newItemEvent = new ItemEvent();
            ItemEventDetailWindow itemEventDetail = new ItemEventDetailWindow(newItemEvent);
            itemEventDetail.ApplyEvent += itemEventDetail_ApplyEventAddNew;
            itemEventDetail.ShowDialog();
        }
        private void itemEventDetail_ApplyEventAddNew(object sender, EventArgs e)
        {
            currentItem.ItemEvents.Add(newItemEvent);
            ItemEventList.Items.Refresh();
        }
    }
}
