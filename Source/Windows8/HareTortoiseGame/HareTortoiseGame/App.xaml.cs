using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace HareTortoiseGame
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            var gamePage = Window.Current.Content as GamePage;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (gamePage == null)
            {
                // Create a main GamePage
                gamePage = new GamePage(args.Arguments);

                SettingsPane.GetForCurrentView().CommandsRequested += App_CommandsRequested;

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the GamePage in the current Window
                Window.Current.Content = gamePage;
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        void App_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            Color backgroundcolor = Color.FromArgb(255, 0, 77, 96);
            var setting = new SettingsCommand("setting", "遊玩設定", (handler) =>
                {
                    var settings = new Callisto.Controls.SettingsFlyout();
                    settings.Content = new HareTortoiseGameXaml.Setting();
                    settings.HeaderText = "遊玩設定";
                    settings.IsOpen = true;
                    settings.ContentForegroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.White);
                    settings.HeaderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
                    settings.ContentBackgroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
                });
            var privateRule = new SettingsCommand("private", "隱私權條款", (handler) =>
                {
                    var settings = new Callisto.Controls.SettingsFlyout();
                    settings.Content = new HareTortoiseGameXaml.PrivateRule();
                    settings.HeaderText = "隱私權條款";
                    settings.IsOpen = true;
                    settings.ContentForegroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.White);
                    settings.HeaderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
                    settings.ContentBackgroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
                });
            var rule = new SettingsCommand("rule", "遊戲說明", (handler) =>
            {
                var settings = new Callisto.Controls.SettingsFlyout();
                settings.Content = new HareTortoiseGameXaml.Rule();
                settings.HeaderText = "遊戲說明";
                settings.IsOpen = true;
                settings.ContentForegroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(Colors.White);
                settings.HeaderBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
                settings.ContentBackgroundBrush = new Windows.UI.Xaml.Media.SolidColorBrush(backgroundcolor);
            });
            args.Request.ApplicationCommands.Add(rule);
            args.Request.ApplicationCommands.Add(setting);
            args.Request.ApplicationCommands.Add(privateRule);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            // TODO: Save application state and stop any background activity
			
            deferral.Complete();
        }
    }
}
