using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topics.Data;

namespace Topics.Util.View
{
    public class ItemExpander
    {
        private DataItem _expandedItem = null;
        private int _expandedItemIndex;

        public DataItem ExpandedItem
        {
            get { return this._expandedItem; }
        }

        public void Swap(DataItem clickedDataItem, int defaultItemTemplate, int expandedItemTemplate)
        {
            if (_expandedItem == null)
            {
                _expandedItem = clickedDataItem;
                _expandedItemIndex = clickedDataItem.Group.Items.IndexOf(clickedDataItem);

                clickedDataItem.Group.Items.Remove(clickedDataItem);
                _expandedItem.ItemTemplate = expandedItemTemplate;

                clickedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);
            }
            else
            {
                if (_expandedItem.Equals(clickedDataItem))
                {
                    clickedDataItem.Group.Items.Remove(clickedDataItem);
                    _expandedItem.ItemTemplate = defaultItemTemplate;

                    clickedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                    _expandedItem = null;
                }
                else
                {
                    clickedDataItem.Group.Items.Remove(_expandedItem);
                    _expandedItem.ItemTemplate = defaultItemTemplate;

                    clickedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);

                    _expandedItem = clickedDataItem;
                    _expandedItemIndex = clickedDataItem.Group.Items.IndexOf(clickedDataItem);

                    clickedDataItem.Group.Items.Remove(clickedDataItem);
                    _expandedItem.ItemTemplate = expandedItemTemplate;

                    clickedDataItem.Group.Items.Insert(_expandedItemIndex, _expandedItem);
                }
            }
        }
    }
}
