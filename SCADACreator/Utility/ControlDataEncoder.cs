using FontAwesome.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace SCADACreator.Utility
{
    public class ControlDataEncoder
    {
        public ControlDataEncoder()
        {

        }

        public static ControlData Convert(SCADAItem item)
        {
            FrameworkElement content = (item.Content as FrameworkElement);
            ControlData data = new ControlData();
            data.ControlType = content.GetType().Name;
            data.X = Canvas.GetLeft(item);
            data.Y = Canvas.GetTop(item);
            data.ZIndex = Canvas.GetZIndex(item);
            data.Width = item.Width;
            data.Height = item.Height;
            data.TagConnection = item.TagConnection;
            //RotateTransform rotateTransform = element.LayoutTransform as RotateTransform;
            //if (rotateTransform == null)
            //{
            //    rotateTransform = new RotateTransform();
            //    element.LayoutTransform = rotateTransform;
            //}
            //rotateTransform.Angle = (rotateTransform.Angle + angle) % 360;


            RotateTransform rotation = content.LayoutTransform as RotateTransform;
            if (rotation != null) // Make sure the transform is actually a RotateTransform
            {
                double rotationInDegrees = rotation.Angle;
                data.Rotation = rotationInDegrees;
                // Do something with the rotationInDegrees here, if needed... 
            }
            if (content.GetType().GetProperties().Any(x => x.Name == "Text"))
            {
                data.LabelText = content.GetType().GetProperty("Text").GetValue(content, null).ToString();
            }
            if (content.GetType().Name == "Image")
            {

                    data.ImageSource = (content as Image).Source.ToString();


            }
            if (content.GetType().Name == "ImageAwesome")
            {
                data.FACode = (char)(content as ImageAwesome).Icon;
            }
            if (content.GetType().Name == "Button")
            {
                data.LabelText = (string)((content as Button).Content as TextBlock).Text;
            }
            if (content.GetType().GetProperties().Any(x => x.Name == "Fill"))
            {
                Brush b1 = (content as Shape).Fill;
                Color c1 = ((SolidColorBrush)b1).Color;
                data.BackgroundColor = new ColorRGB(c1);
            }
            if (content.GetType().GetProperties().Any(x => x.Name == "Background"))
            {
                Brush b1 = (Brush)content.GetType().GetProperty("Background").GetValue(content, null);
                if (!(b1 is null))
                {
                    Color c1 = ((SolidColorBrush)b1).Color;
                    data.BackgroundColor = new ColorRGB(c1);
                }
            }
            if (content.GetType().GetProperties().Any(x => x.Name == "Foreground"))
            {
                Brush b1 = (Brush)content.GetType().GetProperty("Foreground").GetValue(content, null);
                if (!(b1 is null))
                {
                    Color c1 = ((SolidColorBrush)b1).Color;
                    data.ForegroundColor = new ColorRGB(c1);
                }

            }
            if (content.GetType().GetProperties().Any(x => x.Name == "FontSize"))
            {
                data.FontSize = (double)content.GetType().GetProperty("FontSize").GetValue(content, null);
            }

            data.animationSenses = new List<AnimationSense>(item.AnimationSenses);
            data.ItemEvents = new List<ItemEvent>(item.ItemEvents);
            return data;
        }
    }
}
