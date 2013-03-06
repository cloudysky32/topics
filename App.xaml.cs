using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Topics.XamlPage;
using Topics.Util;
using Topics.Data;
using Windows.UI.Xaml.Media.Imaging;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Topics
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        Rect            _windowBounds;
        double          _settingsWidth = 346;
        Popup           _settingsPopup;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += new SuspendingEventHandler(OnSuspending);
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            await Topics.Common.SuspensionManager.SaveAsync();
            deferral.Complete();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
                ExtendedSplash extendedSplash = new ExtendedSplash(args.SplashScreen, loadState);
                Window.Current.Content = extendedSplash;
            }

            Window.Current.Activate();

            #region Another version of OnLaunched Method
            /*
                        Frame rootFrame = Window.Current.Content as Frame;
                        MessageDialog messageDialog = null;

                        // Do not repeat app initialization when the Window already has content,
                        // just ensure that the window is active
                        if (NetworkStatus.Check())
                        {
                            if (rootFrame == null)
                            {
                                // Create a Frame to act as the navigation context and navigate to the first page
                                rootFrame = new Frame();
                                Topics.Common.SuspensionManager.RegisterFrame(rootFrame, "appFrame");

                                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                                {
                                    //TODO: Load state from previously suspended application
                                    if (RoamingSetting.InitUser())
                                    {
                                        await Topics.Common.SuspensionManager.RestoreAsync();
                                    }
                                    else
                                    {
                                        if (!rootFrame.Navigate(typeof(SignInPage), args.Arguments))
                                        {
                                            messageDialog = new MessageDialog("Failed to create initial page");
                                            throw new Exception("Failed to create initial page");
                                        }
                                    }
                                }

                                // Place the frame in the current Window
                                Window.Current.Content = rootFrame;
                            }

                            if (rootFrame.Content == null)
                            {
                                // When the navigation stack isn't restored navigate to the first page,
                                // configuring the new page by passing required information as a navigation
                                // parameter
                                if (RoamingSetting.InitUser())
                                {
                                    HttpClientPostType httpClientPostType = new HttpClientPostType();
                                    List<Category> categoryList = await httpClientPostType.GetCategoryList("0");
                                    httpClientPostType.Dispose();

                                    if (categoryList != null)
                                    {
                                        DataSource dataSource = new DataSource();
                                        dataSource.StoreHotIssues(categoryList);
                                        dataSource.StoreMyTopics(categoryList);
                                        dataSource.StoreCategories(categoryList);

                                        // to MainPage 
                                        if (!rootFrame.Navigate(typeof(MainPage), dataSource.ItemGroups))
                                        {
                                            messageDialog = new MessageDialog("Failed to create initial page");
                                            throw new Exception("Failed to create initial page");
                                        }
                                    }
                                }
                                else
                                {
                                    // to SignInPage
                                    if (!rootFrame.Navigate(typeof(SignInPage), args.Arguments))
                                    {
                                        messageDialog = new MessageDialog("Failed to create initial page");
                                        throw new Exception("Failed to create initial page");
                                    }
                                }
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

                        // Ensure the current window is active
                        Window.Current.Activate();
            */
            #endregion

            _windowBounds = Window.Current.Bounds;
            Window.Current.SizeChanged += OnWindowSizeChanged;
            SettingsPane.GetForCurrentView().CommandsRequested += AppCommandsRequested;

        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>

        // Define the event handler to add the new commands to the Settings pane
        //  when it opens.
        void AppCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            // If your app already has some settings defined, add these blocks of code
            //  to your existing event handler in the order you want them to appear
            //  in the Settings pane.

            // Privacy settings command.
            SettingsCommand cmd = new SettingsCommand("privacy", "Privacy", (x) =>
            {
                _settingsPopup = new Popup();
                _settingsPopup.Closed += OnPopupClosed;
                Window.Current.Activated += OnWindowActivated;
                _settingsPopup.IsLightDismissEnabled = true;
                _settingsPopup.Width = _settingsWidth;
                _settingsPopup.Height = _windowBounds.Height;

                Privacy mypane = new Privacy();
                mypane.Width = _settingsWidth;
                mypane.Height = _windowBounds.Height;

                _settingsPopup.Child = mypane;
                _settingsPopup.SetValue(Canvas.LeftProperty, _windowBounds.Width - _settingsWidth);
                _settingsPopup.SetValue(Canvas.TopProperty, 0);
                _settingsPopup.IsOpen = true;
            });
            args.Request.ApplicationCommands.Add(cmd);
        }

        // Catch resize events to update the Flyout window size.
        void OnWindowSizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            _windowBounds = Window.Current.Bounds;
        }

        // Handle Flyout window activation. 
        private void OnWindowActivated(object sender, Windows.UI.Core.WindowActivatedEventArgs e)
        {
            if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated)
            {
                _settingsPopup.IsOpen = false;
            }
        }

        // Handle Flyout window closing.
        private void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }
    }
}
