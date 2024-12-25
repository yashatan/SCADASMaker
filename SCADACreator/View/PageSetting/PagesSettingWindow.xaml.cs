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

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for PagesSettingWindowxaml.xaml
    /// </summary>
    public partial class PagesSettingWindow : Window
    {
        List<DesignPage> listDesginPage;
        public PagesSettingWindow()
        {
            InitializeComponent();
            listDesginPage =SCADADataProvider.Instance.DesignPages;
            lvPagesSetting.ItemsSource = listDesginPage;
            lvPagesSetting.Items.Refresh();
        }

        private void btnSetMainPage_Cliked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenPage = button.DataContext as DesignPage;
            SCADADataProvider.Instance.ProjectInformation.MainPageId = chosenPage.Id;
            lvPagesSetting.Items.Refresh();
        }

        private void btnOpenPage_Cliked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenPage = button.DataContext as DesignPage;
            OnOpenScreen(chosenPage.Id);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            SCADADataProvider.Instance.AddDesignPage(new DesignPage());
            lvPagesSetting.Items.Refresh();
        }
        private void btnDeletePage_Cliked(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var chosenPage = button.DataContext as DesignPage;
            if (chosenPage.IsMainPage)
            {
                MessageBox.Show("Warning: Can not delete main page. Please set other page is main page before delete this page.");
                return;
            }
            listDesginPage.Remove(chosenPage);
            lvPagesSetting.Items.Refresh();
        }


        private event EventHandler<IntEventArgs> _OpenScreen;
        public event EventHandler<IntEventArgs> OpenScreen
        {
            add
            {
                _OpenScreen += value;
            }
            remove
            {
                _OpenScreen -= value;
            }
        }
        void OnOpenScreen(int id)
        {
            if (_OpenScreen != null)
            {
                _OpenScreen(this, new IntEventArgs(id));
            }
        }


    }

    public class IntEventArgs : EventArgs { public int Value { get; } public IntEventArgs(int value) { Value = value; } }
}
