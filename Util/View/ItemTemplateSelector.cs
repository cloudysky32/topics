using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Topics.Util.View
{
    public class ItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultTileItem { get; set; }
        public DataTemplate ExpandedGridViewItem { get; set; }
        public DataTemplate RightSideTileItem { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            var dataItem = item as DataCommon;

            if (dataItem != null)
            {
                switch (dataItem.ItemTemplate)
                {
                    case ItemTemplates.DEFAULT:
                        return DefaultTileItem;

                    case ItemTemplates.EXPANDED_LIST_VIEW_ITEM:
                        return ExpandedGridViewItem;

                    case ItemTemplates.RIGHT_SIDE_LIST_VIEW_ITEM:
                        return RightSideTileItem;
                }
            }

            return null;
        }
    }
}
