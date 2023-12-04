using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Xml;
using System.Text.Json;
using System.Windows.Media;
using DiagramDesigner.Utility;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace DiagramDesigner
{
    /// <summary>
    /// Interaction logic for ControlPropertyView.xaml
    /// </summary>
    public partial class BluetoothSendingView : Window
    {
        private BluetoothDeviceInfo[] devices;
        private UIElement[] canvasControl;
        private string jsonControlData;
        public BluetoothSendingView()
        {
            InitializeComponent();
            //this.Loaded += ControlPropertyView_Loaded;
            this.ContentRendered += ControlPropertyView_Loaded;
        }

        private async void ControlPropertyView_Loaded(object sender, EventArgs e)
        {
            jsonControlData = ConvertToJasonString(canvasControl); // Convert controls to json string
            await FindDevicesAsync();
            txtblockLoading.Visibility = Visibility.Hidden;
            Loadingcircle.Visibility = Visibility.Hidden;
            DeviceListBox.ItemsSource = devices;
            btnSend.IsEnabled = true;
        }



        private void SaveFile(string filePath, string controldata)
        {
            File.WriteAllText(filePath, controldata);//Save Json string to file
        }
        private async void btnSend_Clicked(object sender, EventArgs e)
        {
            if (this.DeviceListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a device");
                return;
            }
            if (this.txtFileName.Text == string.Empty)
            {
                MessageBox.Show("Please enter a file name");
                return;
            }
            var fileName = txtFileName.Text + ".json";
            SaveFile(fileName, jsonControlData);
            await sendfile(fileName, (DeviceListBox.SelectedItem as BluetoothDeviceInfo));
            MessageBox.Show("Send file succesfully");
        }

        private async Task sendfile(string filename, BluetoothDeviceInfo device)
        {

            await Task.Run(() =>
            {
                BluetoothAddress address = device.DeviceAddress;
                Uri uri = new Uri("obex://" + address.ToString() + "/" + filename);  //Change it to your file name
                ObexWebRequest request = new ObexWebRequest(uri);
                request.ReadFile(filename); // Chnage it to your File Path
                ObexWebResponse response = (ObexWebResponse)request.GetResponse();
                response.Close();
            });
        }

        private string ConvertToJasonString(UIElement[] controls)
        {
            //var children = canvasToEncode.Children.Cast<UIElement>().ToArray();
            List<ControlData> controlDatas = new List<ControlData>();
            // designerCanvas
            foreach (SCADAItem item in controls)
            {

                ControlData controldata = ControlDataEncoder.Convert(item);
                controlDatas.Add(controldata);

            }
            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            string jsonString = JsonSerializer.Serialize(controlDatas, options);//seriallize thành chuỗi json
            return jsonString;
        }

        private async Task FindDevicesAsync()
        {
           await Task.Run(() =>
            {
                BluetoothClient client = new BluetoothClient(); //Create an Instance 
                devices = client.DiscoverDevices();         // find all devices in vicinity + also previosusly remembered device
            });
        }


        public BluetoothSendingView(DesignerCanvas designerCanvas)
        {
            InitializeComponent();
            canvasControl = designerCanvas.Children.Cast<UIElement>().ToArray();

            this.ContentRendered += ControlPropertyView_Loaded;
        }
        

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
