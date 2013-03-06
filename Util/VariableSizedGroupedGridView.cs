using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Topics.Data;
using Windows.UI.Xaml.Input;

namespace Topics.Util
{
    public class VariableSizedGroupedGridView : GridView
    {
        private int rowVal;
        private int colVal;
        private Random _rand;
        private List<Size> _sequence;

        public VariableSizedGroupedGridView()
        {
            _rand = new Random();
            _sequence = new List<Size> { 
                LayoutSizes.PrimaryItem, 
                LayoutSizes.SecondarySmallItem, LayoutSizes.SecondarySmallItem, 
                LayoutSizes.SecondarySmallItem, 
                LayoutSizes.SecondaryTallItem, 
                LayoutSizes.OtherSmallItem, LayoutSizes.OtherSmallItem, LayoutSizes.OtherSmallItem
            };
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var sv = this.GetTemplateChild("ScrollViewer") as UIElement;
            if (sv != null)
                sv.AddHandler(UIElement.PointerWheelChangedEvent, new PointerEventHandler(OnPointerWheelChanged), true);
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            DataItem dataItem = item as DataItem;
            int index = -1;

            if (dataItem.Group.ItemTemplate == ItemTemplates.TALL_SIZE_ITEM)
            {
                colVal = (int)LayoutSizes.PrimaryTallItem.Width;
                rowVal = (int)LayoutSizes.PrimaryTallItem.Height;

                if (dataItem.ItemTemplate == ItemTemplates.EXPANDED_GRID_VIEW_ITEM)
                {
                    colVal = (int)LayoutSizes.PrimaryTallExpandedItem.Width;
                    rowVal = (int)LayoutSizes.PrimaryTallExpandedItem.Height;
                }
            }
            else
            {
                if (dataItem != null)
                {
                    index = dataItem.Group.Items.IndexOf(dataItem);
                }

                if (index >= 0 && index < _sequence.Count)
                {
                    colVal = (int)_sequence[index].Width;
                    rowVal = (int)_sequence[index].Height;
                }
                else
                {
                    colVal = (int)LayoutSizes.OtherSmallItem.Width;
                    rowVal = (int)LayoutSizes.OtherSmallItem.Height;
                }

                if (dataItem.ItemTemplate == ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM)
                {
                    colVal = (int)LayoutSizes.PrimaryWideExpandedItem.Width;
                    rowVal = (int)LayoutSizes.PrimaryWideExpandedItem.Height;
                }
            }

            VariableSizedWrapGrid.SetRowSpan(element as UIElement, rowVal);
            VariableSizedWrapGrid.SetColumnSpan(element as UIElement, colVal);
        }

        public void Invalidate(object item, DataTemplate templateSelector)
        {
            DataItem dataItem = item as DataItem;
            GridViewItem container = this.ItemContainerGenerator.ContainerFromItem(dataItem) as GridViewItem;
            PrepareContainerForItemOverride(container, item);

            //ItemTemplateSelector itemTemplateSelector = new ItemTemplateSelector();
            //itemTemplateSelector.SelectTemplate(dataItem, container);
            //container.UpdateLayout();

            VariableSizedWrapGrid vswGrid = Windows.UI.Xaml.Media.VisualTreeHelper.GetParent(container) as VariableSizedWrapGrid;
            vswGrid.InvalidateMeasure();
        }
    }

    public static class LayoutSizes {
        public static Size PrimaryItem = new Size(6, 2);
        public static Size SecondarySmallItem = new Size(3, 1);
        public static Size SecondaryTallItem = new Size(3, 2);
        public static Size OtherSmallItem = new Size(2, 1);
        
        public static Size PrimaryTallItem = new Size(3, 3);
        public static Size PrimaryTallExpandedItem = new Size(7, 3);
        public static Size PrimaryWideExpandedItem = new Size(12, 3);
    }


    public abstract class TemplateSelector : ContentControl
    {
        public abstract DataTemplate SelectTemplate(object item, DependencyObject container);

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);
            
            ContentTemplate = SelectTemplate(newContent, this);
        }
    }

    public class ItemTemplateSelector : TemplateSelector
    {
        public DataTemplate DefaultTileItem { get; set; }
        public DataTemplate ExpandedGridViewItem { get; set; }
        public DataTemplate ExpandedDetailViewItem { get; set; }
        public DataTemplate ListViewItem { get; set; }
        public DataTemplate DetailViewItem { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var dataItem = item as DataCommon;

            if (dataItem != null)
            {
                switch (dataItem.ItemTemplate)
                {
                    case ItemTemplates.EXPANDED_DETAIL_VIEW_ITEM:
                        return ExpandedDetailViewItem;

                    case ItemTemplates.EXPANDED_GRID_VIEW_ITEM:
                        return ExpandedGridViewItem;
                    
                    case ItemTemplates.LIST_VIEW_ITEM:
                        return ListViewItem;
                    
                    case ItemTemplates.DETAIL_VIEW_ITEM:
                        return DetailViewItem;

                    default:
                        return DefaultTileItem;
                }
            }

            return null;
        }
    }

    public static class ItemTemplates
    {
        public const int DEFAULT = 0;
        public const int TALL_SIZE_ITEM = 1;
        public const int EXPANDED_DETAIL_VIEW_ITEM = 2;
        public const int EXPANDED_GRID_VIEW_ITEM = 3;
        public const int LIST_VIEW_ITEM = 4;
        public const int DETAIL_VIEW_ITEM = 5;
    }
}
