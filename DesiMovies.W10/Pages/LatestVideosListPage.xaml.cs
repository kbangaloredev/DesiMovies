//---------------------------------------------------------------------------
//
// <copyright file="LatestVideosListPage.xaml.cs" company="Microsoft">
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
    public sealed partial class LatestVideosListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }

        InterstitialAd MyVideoAd;
        public LatestVideosListPage()
        {

 

                var MyAppID = "305189ec-650f-4f71-ac5b-f2b77cd866a2";
                // video adunit
                var MyVideoAdUnitId = "11647923";
                
                // instantiate an InterstitialAd
                MyVideoAd = new InterstitialAd();

                // wire up all 4 events, see below for function templates
                MyVideoAd.AdReady += MyVideoAd_AdReady;
                MyVideoAd.ErrorOccurred += MyVideoAd_ErrorOccurred;
                MyVideoAd.Completed += MyVideoAd_Completed;
                MyVideoAd.Cancelled += MyVideoAd_Cancelled;

                // pre-fetch an ad 30-60 seconds before you need it
                MyVideoAd.RequestAd(AdType.Video, MyAppID, MyVideoAdUnitId);
                
                ViewModel = ViewModelFactory.NewList(new LatestVideosSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("fe210090-35c8-46ee-814e-6235e7792a04");
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
            MyVideoAd.Show();
        }


        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // code

       //     var MyAppID = "305189ec-650f-4f71-ac5b-f2b77cd866a2";
            // video adunit
       //     var MyVideoAdUnitId = "11647923";

            // pre-fetch an ad 30-60 seconds before you need it
          //  MyVideoAd.RequestAd(AdType.Video, MyAppID, MyVideoAdUnitId);
            
            //Wait

            //Show the Ad

        }

        void MyVideoAd_Completed(object sender, object e)
        {
            // code
       }

        void MyVideoAd_Cancelled(object sender, object e)
        {
            // code
        }




    }
}
