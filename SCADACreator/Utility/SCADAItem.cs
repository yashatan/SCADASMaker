using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SCADACreator.Model;

namespace SCADACreator.Utility
{
    public class SCADAItem : DesignerItem
    {
        private List<AnimationSense> _animationSenses;

        public List<AnimationSense> AnimationSenses
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


        public virtual TagInfo TagConnection { get; set; }
       // ArrayList arlist1 = new ArrayList();
        public SCADAItem()
        {
            AnimationSenses = new List<AnimationSense>();
            ItemEvents = new List<ItemEvent>();
            this.MinHeight = 5;
            this.MinWidth = 5;
            this.Loaded += new RoutedEventHandler(this.DesignerItem_Loaded);
        }
        static SCADAItem()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SCADAItem), new FrameworkPropertyMetadata(typeof(SCADAItem)));
        }
        private void DesignerItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.Template != null)
            {
                ContentPresenter contentPresenter =
                    this.Template.FindName("PART_ContentPresenter", this) as ContentPresenter;

                MoveThumb thumb =
                    this.Template.FindName("PART_MoveThumb", this) as MoveThumb;

                if (contentPresenter != null && thumb != null)
                {
                    UIElement contentVisual =
                        VisualTreeHelper.GetChild(contentPresenter, 0) as UIElement;

                    if (contentVisual != null)
                    {
                        ControlTemplate template =
                            DesignerItem.GetMoveThumbTemplate(contentVisual) as ControlTemplate;

                        if (template != null)
                        {
                            thumb.Template = template;
                        }
                    }
                }
            }
        }
    }
}
