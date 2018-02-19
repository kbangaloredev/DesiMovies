//---------------------------------------------------------------------------
//
// <copyright file="BubbleDetailPage.xaml.cs" company="Microsoft">
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
using AppStudio.DataProviders.Rss;
using DesiMovies.Sections;
using DesiMovies.Navigation;
using DesiMovies.ViewModels;
using AppStudio.Uwp;
using Microsoft.Advertising.WinRT.UI;

namespace DesiMovies.Pages
{
    public sealed partial class BubbleDetailPage : Page
    {
        private DataTransferManager _dataTransferManager;

        InterstitialAd MyBannerAd;

        public BubbleDetailPage()
        {
            ViewModel = ViewModelFactory.NewDetail(new BubbleSection());

            var MyAppID = "9wzdncrdx48s";
            // Interstitial banner adunit
            var MyAdUnitId = "1100018164";
            //var MyAdUnitId = "test";

            // instantiate an InterstitialAd
            MyBannerAd = new InterstitialAd();
            MyBannerAd.AdReady += MyBannerAd_AdReady;

            MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);

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

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _dataTransferManager.DataRequested -= OnDataRequested;

            base.OnNavigatedFrom(e);
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            ViewModel.ShareContent(args.Request);
        }

        void MyBannerAd_AdReady(object sender, object e)
        {
            // Show the Interstitial Ad if ready
            MyBannerAd.Show();
        }
    }
}
