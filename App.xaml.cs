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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;

namespace Topics
{
    sealed partial class App : Application
    {
        Rect                    _windowBounds;
        double                  _settingsWidth = 346;
        Popup                   _settingsPopup;
        Popup                   _popupPage;

        public static double    menuGridWidth { get; set; }
        public static double    popupPageGridWidth { get; set; }
        public static double    commentGridWidth { get; set; }

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

            _windowBounds = Window.Current.Bounds;
            Window.Current.SizeChanged += OnWindowSizeChanged;
            SettingsPane.GetForCurrentView().CommandsRequested += AppCommandsRequested;

            Window.Current.SizeChanged += ChangeToSnapView;
            _popupPage = new Popup();
        }

        void AppCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
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

        private void ChangeToSnapView(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (ApplicationView.Value == ApplicationViewState.Snapped)
                App.PopupPage(this._popupPage, 330, new SnappedHotTopicsPage("0"), 2);
            else
                this._popupPage.IsOpen = false;
        }

        // Handle Flyout window closing.
        private void OnPopupClosed(object sender, object e)
        {
            Window.Current.Activated -= OnWindowActivated;
        }

        public static async void SignOut()
        {
            MessageDialog messageDialog;

            if (await OAuthLiveId.SignOut())
            {
                if (RoamingSetting.RemoveUserData())
                {
                    messageDialog = new MessageDialog("Successfully signed out. Thank you.");
                    messageDialog.Commands.Add(new UICommand("OK", (UICommandInvokedHandler) =>
                    {
                        Application.Current.Exit();
                    }));
                }
                else
                {
                    messageDialog = new MessageDialog("An internet connection is needed. Please check your connection and try later.");
                }
            }
            else
            {
                messageDialog = new MessageDialog("Sorry, you can't sign out. Because you've loged in Windows with your live account");
            }

            await messageDialog.ShowAsync();
        }

        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        public static void PopupPage(Popup popup, double popupSize, UserControl pane, int direction)
        {
            Rect _windowBounds = Window.Current.Bounds;
            int flyoutOffset = 0;

            popup.IsLightDismissEnabled = false;

            switch (direction)
            {
                case 0: // Direction Left & Hidden Animation
                    popup.Width = popupSize;
                    popup.Height = _windowBounds.Height;

                    pane.Width = popupSize;
                    pane.Height = _windowBounds.Height;

                    popup.SetValue(Canvas.WidthProperty, popupSize);
                    popup.SetValue(Canvas.LeftProperty, 0);

                    var transitions = new TransitionCollection();
                    transitions.Add(new PaneThemeTransition { Edge = EdgeTransitionLocation.Right });
                    popup.ChildTransitions = transitions;
                    break;

                case 1: // Direction Top
                    popup.IsLightDismissEnabled = true;

                    popup.Width = _windowBounds.Width - App.menuGridWidth;
                    popup.Height = popupSize;

                    pane.Width = _windowBounds.Width - App.menuGridWidth;
                    pane.Height = popupSize;

                    popup.SetValue(Canvas.TopProperty, 0);
                    popup.SetValue(Canvas.LeftProperty, App.menuGridWidth);

                    break;

                case 2: // Direction Right
                    popup.Width = popupSize;
                    popup.Height = _windowBounds.Height;

                    pane.Width = popupSize;
                    pane.Height = _windowBounds.Height;

                    popup.SetValue(Canvas.LeftProperty, _windowBounds.Width - popupSize);
                    popup.ChildTransitions = null;
                    break;
            }

            popup.Child = pane;
            popup.IsOpen = true;

            if (direction != 1)
            {
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().Showing += (s, args) =>
                {
                    flyoutOffset = (int)args.OccludedRect.Height;
                    popup.VerticalOffset -= flyoutOffset;
                };
                Windows.UI.ViewManagement.InputPane.GetForCurrentView().Hiding += (s, args) =>
                {
                    popup.VerticalOffset += flyoutOffset;
                };
            }
        }
    }
}
