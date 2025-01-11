using SCADACreator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static SCADACreator.AnimationSense;

namespace SCADACreator.View
{
    /// <summary>
    /// Interaction logic for AnimationDetailWindow.xaml
    /// </summary>
    public partial class AnimationDetailWindow : Window
    {
        private AnimationSense animation;
        List<string> animationProperties = new List<string>() { "Visibility", "Color", "Height", "Width", "Enable", "Text" };
        List<string> boolList = new List<string>() { "False", "True" };
        List<TagInfo> tagsList = SCADADataProvider.Instance.TagInfos;
        string currentProperty;
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
        public AnimationDetailWindow()
        {
            InitializeComponent();
        }

        public AnimationDetailWindow(AnimationSense animation)
        {
            InitializeComponent();
            this.animation = animation;
            this.cbbProperty.ItemsSource = animationProperties;
            this.cbbBool.ItemsSource = boolList;
            this.cbbTag.ItemsSource = tagsList;
            UpdateAnimationData();
        }

        private void UpdateAnimationData()
        {
            txtName.Text = animation.Name;
            if (animation.Tag != null)
            {
                cbbTag.SelectedItem = animation.Tag;
                cbbTag.Text = animation.Tag.Name;
            }

            txtTagMaxValue.Text = animation.Tagvaluemax.ToString();
            txtTagMinValue.Text = animation.Tagvaluemin.ToString();
            this.cbbProperty.SelectedIndex = (int)animation.PropertyNeedChange;
            txtColorR.Text = animation.ColorWhenTagInRange.R.ToString();
            txtColorG.Text = animation.ColorWhenTagInRange.G.ToString();
            txtColorB.Text = animation.ColorWhenTagInRange.B.ToString();
            btnBackground.Background = new SolidColorBrush(Color.FromRgb((byte)animation.ColorWhenTagInRange.R, (byte)animation.ColorWhenTagInRange.G, (byte)animation.ColorWhenTagInRange.B));
            txtStringvalue.Text = animation.TextWhenTagInRange.ToString();
            txtIntValue.Text = animation.PropertyValueWhenTagInRange.ToString();
            if (animation.PropertyBoolValueWhenTagInRange)
            {
                cbbBool.SelectedIndex = 1;
            }
            else
            {
                cbbBool.SelectedIndex = 0;
            }
        }

        private void txtColor_LostFocus(object sender, RoutedEventArgs e)
        {
            Color c1 = new Color();

            ColorRGB newColor = new ColorRGB(c1);
            try
            {
                newColor.R = Convert.ToByte(txtColorR.Text);
                newColor.G = Convert.ToByte(txtColorG.Text);
                newColor.B = Convert.ToByte(txtColorB.Text);
            }
            catch { }
            Brush bg = new SolidColorBrush(Color.FromRgb((byte)newColor.R, (byte)newColor.G, (byte)newColor.B));
            btnBackground.Background = bg;
        }

        private void cbbProperty_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentProperty = (string)cbbProperty.SelectedItem;
            switch (currentProperty)
            {
                case "Visibility":
                case "Enable":
                    VisibleGroup.Visibility = Visibility.Visible;
                    ColorGroup.Visibility = Visibility.Collapsed;
                    ValueGroup.Visibility = Visibility.Collapsed;
                    StringGroup.Visibility = Visibility.Collapsed;
                    break;
                case "Color":
                    VisibleGroup.Visibility = Visibility.Collapsed;
                    ColorGroup.Visibility = Visibility.Visible;
                    ValueGroup.Visibility = Visibility.Collapsed;
                    StringGroup.Visibility = Visibility.Collapsed;
                    break;
                case "Height":
                case "Width":
                    VisibleGroup.Visibility = Visibility.Collapsed;
                    ColorGroup.Visibility = Visibility.Collapsed;
                    ValueGroup.Visibility = Visibility.Visible;
                    StringGroup.Visibility = Visibility.Collapsed;
                    break;
                case "Text":
                    VisibleGroup.Visibility = Visibility.Collapsed;
                    ColorGroup.Visibility = Visibility.Collapsed;
                    ValueGroup.Visibility = Visibility.Collapsed;
                    StringGroup.Visibility = Visibility.Visible;
                    break;
                default: break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            animation.Name = txtName.Text;
            animation.Tag = cbbTag.SelectedItem as TagInfo;
            animation.Tagvaluemax = Convert.ToDouble(txtTagMaxValue.Text);
            animation.Tagvaluemin = Convert.ToDouble(txtTagMinValue.Text);
            animation.PropertyNeedChange = (PropertyType)cbbProperty.SelectedIndex;
            switch (animation.PropertyNeedChange)
            {
                case PropertyType.emIsVisible:
                case PropertyType.emIsEnable:
                    if (cbbBool.Text == "True")
                        animation.PropertyBoolValueWhenTagInRange = true;
                    else animation.PropertyBoolValueWhenTagInRange = false;
                    break;
                case PropertyType.emBackgroundColor:
                    animation.ColorWhenTagInRange.R = Convert.ToByte(txtColorR.Text);
                    animation.ColorWhenTagInRange.G = Convert.ToByte(txtColorG.Text);
                    animation.ColorWhenTagInRange.B = Convert.ToByte(txtColorB.Text);
                    break;
                case PropertyType.emHeight:
                    animation.PropertyValueWhenTagInRange = Convert.ToInt32(txtIntValue.Text);
                    break;
                case PropertyType.emWidth:
                    animation.PropertyValueWhenTagInRange = Convert.ToInt32(txtIntValue.Text);
                    break;
                case PropertyType.emText:
                    animation.TextWhenTagInRange = txtStringvalue.Text;
                    break;
                default: break;
            }
            if (_ApplyEvent != null)
            {
                _ApplyEvent(this, new EventArgs());
            }
            this.Close();

        }

        private void btnBackground_Clicked(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Brush bg = new SolidColorBrush(Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B));
                btnBackground.Background = bg;
                if (!(bg is null))
                {
                    Color c1 = ((SolidColorBrush)bg).Color;
                    var colorRGB = new ColorRGB(c1);

                    txtColorR.Text = colorRGB.R.ToString();
                    txtColorG.Text = colorRGB.G.ToString();
                    txtColorB.Text = colorRGB.B.ToString();
                }
            }
        }
    }
}
