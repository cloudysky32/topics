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
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Topics.XamlPage
{
    public sealed partial class CommentPage : UserControl
    {
        DataGroup _commentListItem = new DataGroup();
        string _postId = string.Empty;

        public CommentPage(string postId)
        {
            this.InitializeComponent();
            this.commentListView.ItemsSource = _commentListItem.Items;
            this._postId = postId;
        }

        private async void CommentPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.progressRing.IsActive = await InitCommentList(this._postId);

            ScrollIntoView();
        }

        private async Task<bool> InitCommentList(string postId)
        {
            HttpClientPostType httpClientPostType = new HttpClientPostType();
            this._commentListItem.StoreCommentData(await httpClientPostType.GetCommentList(postId));

            return false;
        }

        private async void submitCommentButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.commentTextBox.Text.Trim().Equals("") && !(this.commentTextBox.Text.Trim().Length < 7))
            {
                this.progressRing.IsActive = true;
                HttpClientPostType httpClientPostType = new HttpClientPostType();

                if (!await httpClientPostType.SubmitComment(this._postId, User.Instance.Email, this.commentTextBox.Text))
                    await new MessageDialog("Sorry, fail to send comment. Please try later").ShowAsync();

                this.progressRing.IsActive = await InitCommentList(this._postId);

                ScrollIntoView();
            }
            else
            {
                await new MessageDialog("Please fill the form more than 8 characters").ShowAsync();
            }
        }

        private void ScrollIntoView()
        {
            if (this.commentListView.Items.Count > 0)
                this.commentListView.ScrollIntoView(this.commentListView.Items[this.commentListView.Items.Count - 1]);
        }
    }
}
