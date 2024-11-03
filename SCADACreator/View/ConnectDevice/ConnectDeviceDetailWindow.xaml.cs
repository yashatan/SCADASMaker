using SCADACreator.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ConnectDeviceDetailWindow.xaml
    /// </summary>
    public partial class ConnectDeviceDetailWindow : Window
    {
        private ConnectDevice deviceinfo;
        List<string> connectionTypes = new List<string>() { "S7", "ModbusTCP", "OPCUA" };
        private event EventHandler<ConnectDeviceEventArgs> _ApplyEvent;//event handle when confirm button clicked
        public event EventHandler<ConnectDeviceEventArgs> ApplyEvent
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
        public ConnectDeviceDetailWindow()
        {
            InitializeComponent();
            this.cbbConnectionType.ItemsSource = connectionTypes;
        }
        public ConnectDeviceDetailWindow(ConnectDevice connectDevice)
        {
            InitializeComponent();
            deviceinfo = connectDevice;
            this.cbbConnectionType.ItemsSource = connectionTypes;
            UpdateConnectData();
        }

        private void UpdateConnectData()
        {
            txtName.Text = deviceinfo.Name;
            if (deviceinfo.ConnectionType != null)
            {
                this.cbbConnectionType.SelectedIndex = (int)deviceinfo.ConnectionType;
            }
            txtDestination.Text = deviceinfo.Destination;
            txtS7Model.Text = deviceinfo.S7PLCType;
            txtS7Rack.Text = deviceinfo.S7PLCRack.ToString();
            txtS7Slot.Text = deviceinfo.S7PLCSlot.ToString();

            txtModbusPort.Text = deviceinfo.ModbusPort.ToString();
            //txtOPCUAPassword.Text = deviceinfo.OPCUAUserPassword.ToString();
            //txtOPCUAUsername.Text = deviceinfo.OPCUAUserName.ToString();
        }

        private void cbbConnectionType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string currentConnectionType = (string)cbbConnectionType.SelectedItem;
            switch (currentConnectionType)
            {
                case "S7":
                    S7Group.Visibility = Visibility.Visible;
                    ModbusGroup.Visibility = Visibility.Collapsed;
                    OPCUAGroup.Visibility = Visibility.Collapsed;
                    break;
                case "ModbusTCP":
                    S7Group.Visibility = Visibility.Collapsed;
                    ModbusGroup.Visibility = Visibility.Visible;
                    OPCUAGroup.Visibility = Visibility.Collapsed;
                    break;
                case "OPCUA":
                    S7Group.Visibility = Visibility.Collapsed;
                    ModbusGroup.Visibility = Visibility.Collapsed;
                    OPCUAGroup.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            deviceinfo.Name = txtName.Text;
            deviceinfo.Destination = txtDestination.Text;

            deviceinfo.ConnectionType = (int)cbbConnectionType.SelectedIndex;
            switch (deviceinfo.ConnectionType)
            {
                case (int)EnumDefinition.emConnectionType.emS7:
                    deviceinfo.S7PLCSlot = Int32.Parse(txtS7Slot.Text);
                    deviceinfo.S7PLCRack = Int32.Parse(txtS7Rack.Text);
                    deviceinfo.S7PLCType = txtS7Model.Text;
                    break;
                case (int)EnumDefinition.emConnectionType.emTCP:
                    deviceinfo.ModbusPort = Int32.Parse(txtModbusPort.Text);
                    break;
                case (int)EnumDefinition.emConnectionType.emOPCUA:
                    deviceinfo.OPCUAUserName = txtOPCUAUsername.Text;
                    deviceinfo.OPCUAUserPassword = txtOPCUAPassword.Text;
                    break;
                default: break;
            }
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new ConnectDeviceEventArgs(deviceinfo));

            }
            this.Close();
        }



        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public class ConnectDeviceEventArgs : EventArgs
        {
            public ConnectDevice connectDevice { get; set; }
            public ConnectDeviceEventArgs(ConnectDevice _connectDevice)
            {
                connectDevice = _connectDevice;
            }
        }
    }
}
