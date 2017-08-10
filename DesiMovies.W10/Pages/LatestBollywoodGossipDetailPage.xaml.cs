//---------------------------------------------------------------------------
//
// <copyright file="LatestBollywoodGossipDetailPage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>10/4/2016 6:07:22 AM</createdOn>
//
//---------------------------------------------------------------------------

using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using AppStudio.DataProviders.YouTube;
using DesiMovies.Sections;
using DesiMovies.Navigation;
using DesiMovies.ViewModels;
using AppStudio.Uwp;
using Microsoft.Advertising.WinRT.UI;

namespace DesiMovies.Pages
{
    public sealed partial class LatestBollywoodGossipDetailPage : Page
    {
        private DataTransferManager _dataTransferManager;

        InterstitialAd MyVideoAd;
        InterstitialAd MyBannerAd;
        bool bannerready, videoready;

        public LatestBollywoodGossipDetailPage()
        {

           


            //var MyAppID = "9wzdncrdx48s";
            //// video adunit
            //var MyVideoAdUnitId = "11647923";
            //// Interstitial banner adunit
            //var MyAdUnitId = "11673504";




            // instantiate an InterstitialAd
            //MyVideoAd = new InterstitialAd();
            //MyBannerAd = new InterstitialAd();

            // wire up all 4 events, see below for function templates
            //MyVideoAd.AdReady += MyVideoAd_AdReady;
            //MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
            //MyVideoAd.Completed += MyVideoAd_Completed;
            //MyVideoAd.Cancelled += MyVideoAd_Cancelled;

            //MyBannerAd.AdReady += MyBannerAd_AdReady;

            // pre-fetch an ad 30-60 seconds before you need it
            //MyVideoAd.RequestAd(AdType.Video, MyAppID, MyVideoAdUnitId);
            //MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);


            ViewModel = ViewModelFactory.NewDetail(new LatestBollywoodGossipSection());
			this.ViewModel.ShowInfo = false;
            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        public DetailViewModel ViewModel { get; set; }        

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await ViewModel.LoadStateAsync(e.Parameter as NavDetailParameter);

            _dataTransferManager = DataTransferManager.GetForCurrentView();
            _dataTransferManager.DataRequested += OnDataRequested;
            ShellPage.Current.SupportFullScreen = true;

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;
            ShellPage.Current.SupportFullScreen = false;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }

        //void MyVideoAd_AdReady(object sender, object e)
        //{
        //    // code
        //    if (!bannerready)
        //    {
        //        MyVideoAd.Show();
        //        videoready = true;
        //    }
        //}

        //void MyBannerAd_AdReady(object sender, object e)
        //{
        //    // code
        //    if (!videoready)
        //    {
        //        MyBannerAd.Show();
        //        bannerready = true;
        //    }
        //}

        //void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        //{
        //    // code

        //    var A = MyVideoAd.State;
        //}

        //void MyVideoAd_Completed(object sender, object e)
        //{
        //    // code

        //    var A = MyVideoAd.State;
        //}

        //void MyVideoAd_Cancelled(object sender, object e)
        //{
        //    // code

        //    var A = MyVideoAd.State;
        //}

    }
}
