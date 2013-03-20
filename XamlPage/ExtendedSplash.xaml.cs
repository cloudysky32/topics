using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using Topics.Util;
using Topics.Data;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Topics.XamlPage
{
    partial class ExtendedSplash
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;

        private SplashScreen splash; // Variable to hold the splash screen object.

        private CoreDispatcher coreDispatcher;

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            InitializeComponent();
            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);

            this.coreDispatcher = Window.Current.Dispatcher;
            dismissed = true;

            splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);

                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }

            // Create a Frame to act as the navigation context 
            rootFrame = new Frame();

            // Restore the saved session state if necessary
            RestoreStateAsync(loadState);
        }

        async void RestoreStateAsync(bool loadState)
        {
            if (loadState)
                await Topics.Common.SuspensionManager.RestoreAsync();

            // Normally you should start the time consuming task asynchronously here and 
            // dismiss the extended splash screen in the completed handler of that task
            // This sample dismisses extended splash screen  in the handler for "Learn More" button for demonstration
            await InitializeApplication();
        }

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
        {
            extendedSplashImage.SetValue(Canvas.LeftProperty, splashImageRect.X);
            extendedSplashImage.SetValue(Canvas.TopProperty, splashImageRect.Y);
            extendedSplashImage.Height = splashImageRect.Height;
            extendedSplashImage.Width = splashImageRect.Width;
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            // Safely update the extended splash screen image coordinates. This function will be fired in response to snapping, unsnapping, rotation, etc...
            if (splash != null)
            {
                // Update the coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
            }
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;

            // Navigate away from the app's extended splash screen after completing setup operations here...
            // This sample navigates away from the extended splash screen when the "Learn More" button is clicked.
        }

        private void dispatch()
        {
            rootFrame.Navigate(typeof(MainPage));
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
        }

        private async Task InitializeApplication()
        {
            MessageDialog messageDialog = null;

            if (NetworkStatus.Check())
            {
                if (RoamingSetting.InitUser())
                {
                    App.Current.Resources["UserName"] = User.Instance.Name;
                    App.Current.Resources["UserEmail"] = User.Instance.Email;
                    App.Current.Resources["UserPicture"] = new BitmapImage(new Uri(User.Instance.PictureUri));

                    HttpClientPostType httpClientPostType = new HttpClientPostType();
                    User.Instance.Subscription.StoreSubscriptionsData(await httpClientPostType.GetSubscriptionList(User.Instance.Email));

                    await this.coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(dispatch));
                }
                else
                {
                    AnimateLogo.Begin();
                    AnimateSignInBtn.Begin();
                    this.progressRing.IsActive = false;
                }
            }
            else
            {
                messageDialog = new MessageDialog("An internet connection is needed to download feeds. Please check your connection and restart the app.");
                messageDialog.Commands.Add(new UICommand("OK", (UICommandInvokedHandler) =>
                {
                    Application.Current.Exit();
                }));
            }

            if (messageDialog != null) { await messageDialog.ShowAsync(); }
        }

        private async void SignInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.signInButton.IsEnabled)
            {
                this.signInButton.IsEnabled = false;
                this.progressRing.IsActive = true;

                MessageDialog messageDialog = null;

                // Get user account from Live Connect
                if (await OAuthLiveId.GetUserAccount())
                {
                    // Sign up/in Topics
                    if (await new HttpClientPostType().SignIn())
                    {
                        if(RoamingSetting.StoreUserData())
                            await InitializeApplication();
                        else
                            messageDialog = new MessageDialog("Could not connect to server. Please try again.");                        
                    }
                    else
                    {
                        messageDialog = new MessageDialog("Could not connect to server. Please try again.");
                    }
                }
                else
                {
                    messageDialog = new MessageDialog("Sorry, service is not avauable. Please try again.");
                }

                if (messageDialog != null) { await messageDialog.ShowAsync(); }
                
                this.signInButton.IsEnabled = true;
                this.progressRing.IsActive = false;
            }
        }
    }
}
