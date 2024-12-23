using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace SCADACreator
{
    /// <summary>
    /// Interaction logic for SettingSCADAServerWindow.xaml
    /// </summary>
    public partial class SettingSCADAServerWindow : Window
    {
        private string ChosenPath;
        private event EventHandler<StringEventArgs> _ApplyEvent;
        public event EventHandler<StringEventArgs> ApplyEvent
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
        public SettingSCADAServerWindow(string currentPath)
        {
            InitializeComponent();
            ChosenPath = currentPath;
            txtSCADAServerLocation.Text = ChosenPath;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Program files (*.exe)|*.exe|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                ChosenPath = openFileDialog.FileName;
                txtSCADAServerLocation.Text = ChosenPath;
            }
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new StringEventArgs(ChosenPath));
            }
            this.Close();
        }
    }
    public class StringEventArgs : EventArgs { public string ServerPath { get; } public StringEventArgs(string serverPath) { ServerPath = serverPath; } }
}
