//---------------------------------------------------------------------------
//
// <copyright file="LatestBollywoodGossipListPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>10/4/2016 6:07:22 AM</createdOn>
//
//---------------------------------------------------------------------------

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using AppStudio.DataProviders.YouTube;
using DesiMovies.Sections;
using DesiMovies.ViewModels;
using AppStudio.Uwp;
using Microsoft.Advertising.WinRT.UI;

namespace DesiMovies.Pages
{
    public sealed partial class LatestBollywoodGossipListPage : Page
    {

        InterstitialAd MyVideoAd;
        InterstitialAd MyBannerAd;
        bool bannerready, videoready;

        public ListViewModel ViewModel { get; set; }
        public LatestBollywoodGossipListPage()
        {

            var MyAppID = "305189ec-650f-4f71-ac5b-f2b77cd866a2";
            // video adunit
            var MyVideoAdUnitId = "11647923";
            // Interstitial banner adunit
            var MyAdUnitId = "11673504";




            // instantiate an InterstitialAd
            MyVideoAd = new InterstitialAd();
            MyBannerAd = new InterstitialAd();

            // wire up all 4 events, see below for function templates
            MyVideoAd.AdReady += MyVideoAd_AdReady;
            MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
            MyVideoAd.Completed += MyVideoAd_Completed;
            MyVideoAd.Cancelled += MyVideoAd_Cancelled;

            MyBannerAd.AdReady += MyBannerAd_AdReady;

            // pre-fetch an ad 30-60 seconds before you need it
       //     MyVideoAd.RequestAd(AdType.Video, MyAppID, MyVideoAdUnitId);
            MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);



            ViewModel = ViewModelFactory.NewList(new LatestBollywoodGossipSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("0b5e33fb-860a-4413-b2c3-525d122ece4e");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

        void MyVideoAd_AdReady(object sender, object e)
        {
            // code
            if (!bannerready)
            {
     //           MyVideoAd.Show();
     //           videoready = true;
            }
        }

        void MyBannerAd_AdReady(object sender, object e)
        {
            // code
                MyBannerAd.Show();
                bannerready = true;
        }

        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // code
            var A = MyVideoAd.State;
            // On Error - Make a second call for a video Ad
            // instantiate an InterstitialAd
            MyVideoAd = new InterstitialAd();

        }

        void MyVideoAd_Completed(object sender, object e)
        {
            // code

            var A = MyVideoAd.State;
        }

        void MyVideoAd_Cancelled(object sender, object e)
        {
            // code

            var A = MyVideoAd.State;
        }

    }
}
