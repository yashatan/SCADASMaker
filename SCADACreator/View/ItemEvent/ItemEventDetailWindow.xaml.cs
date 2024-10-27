using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using static SCADACreator.AnimationSense;
using static SCADACreator.ItemEvent;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for ItemEventDetailWindow.xaml
    /// </summary>
    public partial class ItemEventDetailWindow : Window
    {
        private ItemEvent itemevent;
        List<string> eventTypes = new List<string>() { "Click", "Press", "Release" };
        List<string> actionTypes = new List<string>() { "Set", "Reset" };
        List<TagInfo> tagsList = DataProvider.Instance.DB.TagInfoes.ToList();
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
        public ItemEventDetailWindow()
        {
            InitializeComponent();
        }

        public ItemEventDetailWindow(ItemEvent itemevent)
        {
            InitializeComponent();
            this.itemevent = itemevent;
            this.cbbEventType.ItemsSource = eventTypes;
            this.cbbActionType.ItemsSource = actionTypes;
            this.cbbTag.ItemsSource = tagsList;
            UpdateItemEventData();
        }

        private void UpdateItemEventData()
        {
            txtName.Text = itemevent.Name;
            cbbTag.SelectedItem = itemevent.Tag;
            cbbEventType.SelectedIndex = (int)itemevent.EventType;
            cbbActionType.SelectedIndex = (int)itemevent.ActionType;
        }

        private void cbbActionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            itemevent.Name = txtName.Text;
            itemevent.Tag = cbbTag.SelectedItem as TagInfo;
            itemevent.EventType = (ItemEventType)cbbEventType.SelectedIndex;
            itemevent.ActionType = (ItemActiontype)cbbActionType.SelectedIndex;
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();

        }
    }
}
