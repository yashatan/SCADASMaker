using SCADACreator.Model;
using SCADACreator.Utility;
using System;
using System.Collections.Generic;
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
using static SCADACreator.ConnectDevice;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for TagInfoDetailWindow.xaml
    /// </summary>
    public partial class TagInfoDetailWindow : Window
    {
        private TagInfo currentTag;
        List<ConnectDevice> deviceAttachs = SCADADataProvider.Instance.ConnectDevices;
        List<string> TagTypes = new List<string>() { "Bool", "Byte", "Short", "Int", "Real", "Double" };
        private event EventHandler<TagInfoEventArgs> _ApplyEvent;//event handle when confirm button clicked
        public event EventHandler<TagInfoEventArgs> ApplyEvent
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
        public TagInfoDetailWindow()
        {
            InitializeComponent();
            this.cbbDeviceAttach.ItemsSource = deviceAttachs;
            this.cbbTagType.ItemsSource = TagTypes;
        }

        public TagInfoDetailWindow(TagInfo tagInfo)
        {
            InitializeComponent();
            this.cbbDeviceAttach.ItemsSource = deviceAttachs;
            this.cbbTagType.ItemsSource = TagTypes;
            currentTag = tagInfo;
            UpdateData();
        }

        private void UpdateData()
        {
            txtName.Text = currentTag.Name;
            txtAddress.Text = currentTag.MemoryAddress;
            txtNodeId.Text = currentTag.NodeId;
            cbbTagType.SelectedIndex = (int) currentTag.Type;
            txtBitPosition.Text = currentTag.BitPosition.ToString();
            if (currentTag.ConnectDevice != null)
            {
                cbbDeviceAttach.SelectedItem = currentTag.ConnectDevice;
                if(currentTag.ConnectDevice.ConnectionType == (int)EnumDefinition.emConnectionType.emOPCUA)
                {
                    OPCGroup.Visibility = Visibility.Visible;
                }
                cbbDeviceAttach.Text = currentTag.ConnectDevice.Name;
            }

            cbbDeviceAttach.Items.Refresh();
        }

        private void cbbDeviceAttach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((cbbDeviceAttach.SelectedItem as ConnectDevice).ConnectionType == (int)EnumDefinition.emConnectionType.emOPCUA)
            {
                OPCGroup.Visibility = Visibility.Visible;
            }
            else {
                OPCGroup.Visibility = Visibility.Collapsed;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            currentTag.Name = txtName.Text;
            currentTag.MemoryAddress = txtAddress.Text;
            currentTag.ConnectDevice = cbbDeviceAttach.SelectedItem as ConnectDevice;
            currentTag.Type= (TagInfo.TagType) cbbTagType.SelectedIndex;
            currentTag.BitPosition = Convert.ToByte(txtBitPosition.Text);
            currentTag.NodeId = txtNodeId.Text;
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new TagInfoEventArgs(currentTag));
            }
            this.Close();
        }

        public class TagInfoEventArgs : EventArgs
        {
            public TagInfo TagInfo { get; set; }
            public TagInfoEventArgs(TagInfo tagInfo)
            {
                TagInfo = tagInfo;
            }
        }

        private void cbbTagType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentTagType = (string)cbbTagType.SelectedItem;
            switch (currentTagType)
            {
                case "Bool":
                    BitPositionGroup.Visibility = Visibility.Visible; break;
                default:
                    BitPositionGroup.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }

}
