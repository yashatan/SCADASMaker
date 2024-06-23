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
    /// Interaction logic for DeviceListWindow.xaml
    /// </summary>
    public partial class DeviceListWindow : Window
    {
        private ConnectDevice newDeviceInfo;

        public DeviceListWindow()
        {
            InitializeComponent();
            DummyData a = new DummyData();
            DeviceList.ItemsSource = DummyData.connectDevices;
            DeviceList.Items.Refresh();
        }
        
        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var device = DeviceList.SelectedItem as ConnectDevice;
            ConnectDeviceDetailWindow connectDeviceDetail = new ConnectDeviceDetailWindow(device);
            connectDeviceDetail.ApplyEvent += ConnectDeviceDetailWindow_ApplyEventEdit;
            connectDeviceDetail.ShowDialog();
        }

        private void ConnectDeviceDetailWindow_ApplyEventEdit(object sender, EventArgs e)
        {
            DeviceList.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var connectDevice = DeviceList.SelectedItem as ConnectDevice;
            DummyData.connectDevices.Remove(connectDevice);
            DeviceList.Items.Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            newDeviceInfo = new ConnectDevice();
            ConnectDeviceDetailWindow deviceDettail = new ConnectDeviceDetailWindow(newDeviceInfo);
            deviceDettail.ApplyEvent += ConnectDeviceDetailWindow_ApplyEventNew;
            deviceDettail.ShowDialog();
        }

        private void ConnectDeviceDetailWindow_ApplyEventNew(object sender, EventArgs e)
        {
            DummyData.connectDevices.Add(newDeviceInfo);
            DeviceList.Items.Refresh();
            
        }
    }
}
