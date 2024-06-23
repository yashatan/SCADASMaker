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
using static SCADACreator.ConnectDevice;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for TagInfoDetailWindow.xaml
    /// </summary>
    public partial class TagInfoDetailWindow : Window
    {
        private TagInfo currentTag;
        List<ConnectDevice> deviceAttach = DummyData.connectDevices;
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
        public TagInfoDetailWindow()
        {
            InitializeComponent();
            this.cbbDeviceAttach.ItemsSource = deviceAttach;
        }

        public TagInfoDetailWindow(TagInfo tagInfo)
        {
            InitializeComponent();
            this.cbbDeviceAttach.ItemsSource = deviceAttach;
            currentTag = tagInfo;
            UpdateData();
        }

        private void UpdateData()
        {
            txtName.Text = currentTag.Name;
            txtAddress.Text = currentTag.MemoryAddress;
            cbbDeviceAttach.SelectedItem = currentTag.DeviceAttach;
        }

        private void cbbDeviceAttach_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            currentTag.Name = txtName.Text;
            currentTag.MemoryAddress = txtAddress.Text;
            currentTag.DeviceAttach = cbbDeviceAttach.SelectedItem as ConnectDevice;

            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();
        }
    }
}
