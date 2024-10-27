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
using SCADACreator.Utility;
using InTheHand.Net.Sockets;
using InTheHand.Net;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Threading.Tasks;
using System.Security.AccessControl;
using Microsoft.Win32;
using SCADACreator.Model;
using System.Text.Json.Serialization;

namespace SCADACreator
{
    /// <summary>
    /// Interaction logic for ControlPropertyView.xaml
    /// </summary>
    public partial class BluetoothSendingView : Window
    {
        private BluetoothDeviceInfo[] devices;
        private UIElement[] canvasControl;
        List<ControlData> controlDatas;
        public BluetoothSendingView()
        {
            InitializeComponent();
            this.ContentRendered += ControlPropertyView_Loaded;
        }

        private async void ControlPropertyView_Loaded(object sender, EventArgs e)
        {
            //await FindDevicesAsync();
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
            //if (this.DeviceListBox.SelectedItems.Count == 0)
            //{
            //    MessageBox.Show("Please select a device");
            //    return;
            //}
            //if (this.txtFileName.Text == string.Empty)
            //{
            //    MessageBox.Show("Please enter a file name");
            //    return;
            //}
            //var fileName = txtFileName.Text + ".json";
            //SaveFile(fileName, jsonControlData);
            //await sendfile(fileName, (DeviceListBox.SelectedItem as BluetoothDeviceInfo));
            //MessageBox.Show("Send file succesfully");
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

        private void GenerateControlDataFromCanvas(DesignerCanvas designerCanvas)
        {
            canvasControl = designerCanvas.Children.Cast<UIElement>().ToArray();
            controlDatas = new List<ControlData>();
            foreach (SCADAItem item in canvasControl)
            {

                ControlData controldata = ControlDataEncoder.Convert(item);
                controlDatas.Add(controldata);

            }
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
            GenerateControlDataFromCanvas(designerCanvas); // Convert controls to json string
            this.ContentRendered += ControlPropertyView_Loaded;
        }
        

        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {

        }

        private void btnGenStationFile_Clicked(object sender, RoutedEventArgs e)
        {

            var options = new JsonSerializerOptions
            {
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowNamedFloatingPointLiterals,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            SCADAStationConfiguration mSCADAStationConfiguration= new SCADAStationConfiguration();
            mSCADAStationConfiguration.SetConnectDevices(DataProvider.Instance.DB.ConnectDevices.ToList());
            mSCADAStationConfiguration.SetTagInfos(DataProvider.Instance.DB.TagInfoes.ToList());
            mSCADAStationConfiguration.SetControlDatas(controlDatas);

            string jsonSCADAStationConfiguration = JsonSerializer.Serialize(mSCADAStationConfiguration, options);//seriallize thành chuỗi json

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "untitled";
            saveFileDialog.Filter = "JsonString (*.json)|*.json";
            saveFileDialog.ShowDialog();
            Console.WriteLine(saveFileDialog.FileName);
            SaveFile(saveFileDialog.FileName + ".json", jsonSCADAStationConfiguration);
        }
    }
}
