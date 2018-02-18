//---------------------------------------------------------------------------
//
// <copyright file="LatestOnTwitterListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>10/4/2016 6:07:22 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Twitter;
using DesiMovies.Sections;
using DesiMovies.ViewModels;
using AppStudio.Uwp;
using Microsoft.Advertising.WinRT.UI;

namespace DesiMovies.Pages
{
    public sealed partial class LatestOnTwitterListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }

        InterstitialAd MyBannerAd;

        public LatestOnTwitterListPage()
        {

            var MyAppID = "9wzdncrdx48s";
            // Interstitial banner adunit
            var MyAdUnitId = "11691221";

            // instantiate an InterstitialAd
            MyBannerAd = new InterstitialAd();
            MyBannerAd.AdReady += MyBannerAd_AdReady;

            MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);

            ViewModel = ViewModelFactory.NewList(new LatestOnTwitterSection());



            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Disabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("87e7e5ee-e7fc-4266-a0ce-a292f2080671");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

        void MyBannerAd_AdReady(object sender, object e)
        {
            // Show the Interstitial Ad if ready
            MyBannerAd.Show();
        }

    }
}
