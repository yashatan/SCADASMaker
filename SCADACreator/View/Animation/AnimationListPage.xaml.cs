using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
using SCADACreator.Utility;
using SCADACreator.ViewModel;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for AnimationListPage.xaml
    /// </summary>
    public partial class AnimationListPage : Page
    {

        List<AnimationSense> senses;
        SCADAItem currentItem;
        AnimationSense newAnimation;

        public AnimationListPage()
        {
            InitializeComponent();
            AnimationList.ItemsSource = senses;
        }

        public void SetCurrentItem(DesignerItem item)
        {
            currentItem = (item as SCADAItem);
            senses = currentItem.animationSenses;
            AnimationList.ItemsSource = senses;
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = AnimationList.SelectedItem as AnimationSense;
            AnimationDetailWindow animationDetail = new AnimationDetailWindow(animation);
            animationDetail.ApplyEvent += AnimationDetail_ApplyEventEdit;
            animationDetail.ShowDialog();
        }

        private void AnimationDetail_ApplyEventEdit(object sender, EventArgs e)
        {
            AnimationList.Items.Refresh();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var animation = AnimationList.SelectedItem as AnimationSense;
            currentItem.animationSenses.Remove(animation);
            AnimationList.Items.Refresh();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            newAnimation = new AnimationSense();
            AnimationDetailWindow animationDetail = new AnimationDetailWindow(newAnimation);
            animationDetail.ApplyEvent += AnimationDetail_ApplyEventAddNew;
            animationDetail.ShowDialog();
        }
        private void AnimationDetail_ApplyEventAddNew(object sender, EventArgs e)
        {
            currentItem.animationSenses.Add(newAnimation);
            AnimationList.Items.Refresh();
        }
    }
}
