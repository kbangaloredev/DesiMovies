//---------------------------------------------------------------------------
//
// <copyright file="MusicReviewsListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>10/4/2016 6:07:22 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.Rss;
using DesiMovies.Sections;
using DesiMovies.ViewModels;
using AppStudio.Uwp;
using Microsoft.Advertising.WinRT.UI;

namespace DesiMovies.Pages
{
    public sealed partial class MusicReviewsListPage : Page
    {

        InterstitialAd MyBannerAd;

        public ListViewModel ViewModel { get; set; }
        public MusicReviewsListPage()
        {
            var MyAppID = "9wzdncrdx48s";
            // Interstitial banner adunit
            var MyAdUnitId = "11691221";
            
            // instantiate an InterstitialAd
            MyBannerAd = new InterstitialAd();
            MyBannerAd.AdReady += MyBannerAd_AdReady;

            MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);

            ViewModel = ViewModelFactory.NewList(new MusicReviewsSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Disabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("d19279f6-cc25-42c4-af06-819a9c100ef3");
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
