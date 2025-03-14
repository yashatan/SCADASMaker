﻿using System.Windows;
using System.Windows.Controls;

namespace SCADACreator
{
    public class Toolbox : ItemsControl
    {
        private Size defaultItemSize = new Size(10, 10);
        public Size DefaultItemSize
        {
            get { return this.defaultItemSize; }
            set { this.defaultItemSize = value; }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ToolboxItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is ToolboxItem);
        }
    }
}
