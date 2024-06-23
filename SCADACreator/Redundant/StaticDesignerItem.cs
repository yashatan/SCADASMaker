using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SCADACreator
{
   public class StaticDesignerItem: ContentControl
    {
        private List<AnimationSense> _animationSenses;

        public List<AnimationSense> animationSenses
        {
            get { return _animationSenses; }
            set { _animationSenses = value; }
        }
        private List<ItemEvent> itemEvents;

        public List<ItemEvent> ItemEvents
        {
            get { return itemEvents; }
            set { itemEvents = value; }
        }
        public string TagConnection { get; set; }
        public StaticDesignerItem()
        {
            animationSenses = new List<AnimationSense>();
            ItemEvents = new List<ItemEvent>();
            this.Loaded += new RoutedEventHandler(this.DesignerItem_Loaded);
        }
        static StaticDesignerItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(StaticDesignerItem), new FrameworkPropertyMetadata(typeof(StaticDesignerItem)));
        }
        private void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                //MoveThumb thumb =
                //    this.Template.FindName("PART_MoveThumb", this) as MoveThumb;

                if (contentPresenter != null)
                {
                    UIElement contentVisual =
                        VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;

                    if (contentVisual != null)
                    {
                        ControlTemplate template =
                            DesignerItem.GetMoveThumbTemplate(contentVisual) as ControlTemplate;


                    }
                }
            }
        }
    }
}
