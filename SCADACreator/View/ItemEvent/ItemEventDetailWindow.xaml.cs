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
        private ItemEvent currentItemEvent;
        List<string> eventTypes = new List<string>() { "Click", "Press", "Release" };
        List<string> actionTypes = new List<string>() { "SetBit", "ResetBit", "SetValue", "OpenScreen" };
        List<TagInfo> tagsList = SCADADataProvider.Instance.TagInfos;
        //List<DesignPage> pageList = SCADADataProvider.Instance.DesignPages;
        List<BaseSCADAPage> pageList;
        private event EventHandler _ApplyEvent;
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
            this.currentItemEvent = itemevent;
            this.cbbEventType.ItemsSource = eventTypes;
            this.cbbActionType.ItemsSource = actionTypes;
            this.cbbTag.ItemsSource = tagsList;
            pageList = new List<BaseSCADAPage>();
            pageList.AddRange(SCADADataProvider.Instance.DesignPages);
            pageList.AddRange(SCADADataProvider.Instance.TablePages);
            this.cbbPage.ItemsSource = pageList;

            UpdateItemEventData();
        }

        private void UpdateItemEventData()
        {
            txtName.Text = currentItemEvent.Name;
            txtValue.Text = currentItemEvent.Value.ToString();
            if(currentItemEvent.Tag != null)
            {
                cbbTag.SelectedItem = currentItemEvent.Tag;
                cbbTag.Text = currentItemEvent.Tag.Name;
            }
            if (pageList.Any(p=>p.Id == currentItemEvent.PageID && p.PageType == currentItemEvent.PageType))
            {
                var page = pageList.FirstOrDefault(p => p.Id == currentItemEvent.PageID && p.PageType == currentItemEvent.PageType);
                if (page != null) {
                    cbbPage.SelectedItem = page;
                    cbbPage.Text = page.Name;
                }
            }
            cbbEventType.SelectedIndex = (int)currentItemEvent.EventType;
            cbbActionType.SelectedIndex = (int)currentItemEvent.ActionType;
            if (currentItemEvent.ActionType == ItemEvent.ItemActiontype.emOpenScreen)
            {
                ScreenGroup.Visibility = Visibility.Visible;
                TagGroup.Visibility = Visibility.Collapsed;
            }
            else
            {
                ScreenGroup.Visibility= Visibility.Collapsed;
                TagGroup.Visibility = Visibility.Visible;
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            currentItemEvent.Name = txtName.Text;
            currentItemEvent.Tag = cbbTag.SelectedItem as TagInfo;
            if ((cbbPage.SelectedItem as BaseSCADAPage) != null) {
                currentItemEvent.PageID = (cbbPage.SelectedItem as BaseSCADAPage).Id;
                currentItemEvent.PageType = (cbbPage.SelectedItem as BaseSCADAPage).PageType;
            }
            currentItemEvent.EventType = (ItemEventType)cbbEventType.SelectedIndex;
            currentItemEvent.ActionType = (ItemActiontype)cbbActionType.SelectedIndex;
            currentItemEvent.Value = Convert.ToInt32(txtValue.Text);
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();

        }

        private void cbbActionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentConnectionType = (string)cbbActionType.SelectedItem;
            if(currentConnectionType == "SetValue")
            {
                ValueGroup.Visibility = Visibility.Visible;
            }
            else
            {
                ValueGroup.Visibility = Visibility.Collapsed;
            }
            if (currentConnectionType == "OpenScreen")
            {
                ScreenGroup.Visibility = Visibility.Visible;
                TagGroup.Visibility = Visibility.Collapsed;
            }
            else
            {
                ScreenGroup.Visibility = Visibility.Collapsed;
                TagGroup.Visibility = Visibility.Visible;
            }
        }
    }
}
