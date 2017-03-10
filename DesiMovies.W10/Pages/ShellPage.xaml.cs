using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Media.Imaging;

using AppStudio.Uwp;
using AppStudio.Uwp.Controls;
using AppStudio.Uwp.Navigation;

using DesiMovies.Navigation;

namespace DesiMovies.Pages
{
    public sealed partial class ShellPage : Page
    {
        public static ShellPage Current { get; private set; }

        public ShellControl ShellControl
        {
            get { return shell; }
        }

        public Frame AppFrame
        {
            get { return frame; }
        }

        public ShellPage()
        {
            InitializeComponent();

            this.DataContext = this;
            ShellPage.Current = this;

            this.SizeChanged += OnSizeChanged;
            if (SystemNavigationManager.GetForCurrentView() != null)
            {
                SystemNavigationManager.GetForCurrentView().BackRequested += ((sender, e) =>
                {
                    if (SupportFullScreen && ShellControl.IsFullScreen)
                    {
                        e.Handled = true;
                        ShellControl.ExitFullScreen();
                    }
                    else if (NavigationService.CanGoBack())
                    {
                        NavigationService.GoBack();
                        e.Handled = true;
                    }
                });
				
                NavigationService.Navigated += ((sender, e) =>
                {
                    if (NavigationService.CanGoBack())
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
                    }
                    else
                    {
                        SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
                    }
                });
            }
        }

		public bool SupportFullScreen { get; set; }

		#region NavigationItems
        public ObservableCollection<NavigationItem> NavigationItems
        {
            get { return (ObservableCollection<NavigationItem>)GetValue(NavigationItemsProperty); }
            set { SetValue(NavigationItemsProperty, value); }
        }

        public static readonly DependencyProperty NavigationItemsProperty = DependencyProperty.Register("NavigationItems", typeof(ObservableCollection<NavigationItem>), typeof(ShellPage), new PropertyMetadata(new ObservableCollection<NavigationItem>()));
        #endregion

		protected override void OnNavigatedTo(NavigationEventArgs e)
        {
#if DEBUG
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size { Width = 320, Height = 500 });
#endif
            NavigationService.Initialize(typeof(ShellPage), AppFrame);
			NavigationService.NavigateToPage<HomePage>(e);

            InitializeNavigationItems();

            Bootstrap.Init();
        }		        
		
		#region Navigation
        private void InitializeNavigationItems()
        {
            NavigationItems.Add(AppNavigation.NodeFromAction(
				"Home",
                "Home",
                (ni) => NavigationService.NavigateToRoot(),
                AppNavigation.IconFromSymbol(Symbol.Home)));
            NavigationItems.Add(AppNavigation.NodeFromAction(
				"fe210090-35c8-46ee-814e-6235e7792a04",
                "latest videos",                
                AppNavigation.ActionFromPage("LatestVideosListPage"),
				AppNavigation.IconFromGlyph("\ue173")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"5702db7c-5e17-4abe-9c84-ac8b3b326dd3",
                "hindi movie news",                
                AppNavigation.ActionFromPage("HindiMovieNewsListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"0b5e33fb-860a-4413-b2c3-525d122ece4e",
                "latest bollywood gossip",                
                AppNavigation.ActionFromPage("LatestBollywoodGossipListPage"),
				AppNavigation.IconFromGlyph("\ue173")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"d19279f6-cc25-42c4-af06-819a9c100ef3",
                "music reviews",                
                AppNavigation.ActionFromPage("MusicReviewsListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"0e08646d-f292-4a1f-955e-ef1f62e00516",
                "Reviews by Kiran",                
                AppNavigation.ActionFromPage("ReviewsByKiranListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"11d60850-8880-4f44-b084-593683677980",
                "Now Running Updates",                
                AppNavigation.ActionFromPage("NowRunningUpdatesListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"0d0da0be-5470-4a9c-9667-42702e7ccb22",
                "Bharat Student Updates",                
                AppNavigation.ActionFromPage("BharatStudentUpdatesListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"87e7e5ee-e7fc-4266-a0ce-a292f2080671",
                "latest on twitter",                
                AppNavigation.ActionFromPage("LatestOnTwitterListPage"),
				AppNavigation.IconFromGlyph("\ue134")));

            NavigationItems.Add(AppNavigation.NodeFromAction(
				"97f5d53c-a88b-45a7-86e3-247333ec247b",
                "bubble",                
                AppNavigation.ActionFromPage("BubbleListPage"),
				AppNavigation.IconFromGlyph("\ue12a")));

            NavigationItems.Add(NavigationItem.Separator);

            NavigationItems.Add(AppNavigation.NodeFromControl(
				"About",
                "NavigationPaneAbout".StringResource(),
                new AboutPage(),
                AppNavigation.IconFromImage(new Uri("ms-appx:///Assets/about.png"))));
        }        
        #endregion        

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateDisplayMode(e.NewSize.Width);
        }

        private void UpdateDisplayMode(double? width = null)
        {
            if (width == null)
            {
                width = Window.Current.Bounds.Width;
            }
            this.ShellControl.DisplayMode = width > 640 ? SplitViewDisplayMode.CompactOverlay : SplitViewDisplayMode.Overlay;
            this.ShellControl.CommandBarVerticalAlignment = width > 640 ? VerticalAlignment.Top : VerticalAlignment.Bottom;
        }

        private async void OnKeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.F11)
            {
                if (SupportFullScreen)
                {
                    await ShellControl.TryEnterFullScreenAsync();
                }
            }
            else if (e.Key == Windows.System.VirtualKey.Escape)
            {
                if (SupportFullScreen && ShellControl.IsFullScreen)
                {
                    ShellControl.ExitFullScreen();
                }
                else
                {
                    NavigationService.GoBack();
                }
            }
        }
    }
}
