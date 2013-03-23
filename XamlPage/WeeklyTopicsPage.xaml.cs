using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Topics.Data;
using Topics.Util;

namespace Topics.XamlPage
{
    public sealed partial class WeeklyTopicsPage : UserControl
    {
        public DataGroup WeeklyTopicsData { get; set; }
        public DataModel WeekData { get; set; }

        private string _selectedCommunityId;

        public WeeklyTopicsPage(string selectedCommunityId)
        {
            this.InitializeComponent();
            this._selectedCommunityId = selectedCommunityId;

            WeeklyTopicsData = new DataGroup();
            WeekData = new DataModel();
        }

        private async void WeeklyTopicsPage_Loaded(object sender, RoutedEventArgs e)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            //this._weeklyTopicsData.StorePostsData(await httpClientPostType.GetWeeklyTopicList(this._selectedCommunityId, User.Instance.Email));
            this.WeekData.StoreWeeklyTopicsData(await httpClientPostType.GetWeeklyTopicList(this._selectedCommunityId, User.Instance.Email));

            this.weekListView.ItemsSource = this.WeekData.ItemGroups;
            //this.weeklyTopicsGridView.ItemsSource = this.WeeklyTopicsData.Items;
        }

        private void WeekListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.WeeklyTopicsData.Items = ((DataGroup)this.weekListView.SelectedItem).Items;
        }
    }
}
