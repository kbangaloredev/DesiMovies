//---------------------------------------------------------------------------
//
// <copyright file="HomePage.xaml.cs" company="Microsoft">
//    Copyright (C) 2015 by Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <createdOn>10/4/2016 6:07:22 AM</createdOn>
//
//---------------------------------------------------------------------------

using System.Windows.Input;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp.Navigation;
using Microsoft.Advertising.WinRT.UI;

using DesiMovies.ViewModels;
using System.Diagnostics;
using System;

using Windows.Services.Store;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DesiMovies.Pages
{
    public sealed partial class HomePage : Page
    {
        InterstitialAd MyVideoAd;
        InterstitialAd MyBannerAd;

        bool bannerready, videoready;

        public HomePage()
        {


            var MyAppID = "9wzdncrdx48s";
            // video adunit
            var MyVideoAdUnitId = "11647923";
            //var MyVideoAdUnitId = "test";




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
            MyVideoAd.RequestAd(AdType.Video, MyAppID, MyVideoAdUnitId);
          //  MyBannerAd.RequestAd(AdType.Display, MyAppID, MyAdUnitId);


            ViewModel = new MainViewModel(12);            
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
			commandBar.DataContext = ViewModel;
			searchBox.SearchCommand = SearchCommand;
			this.SizeChanged += OnSizeChanged;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }		
        public MainViewModel ViewModel { get; set; }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await this.ViewModel.LoadDataAsync();
			//Page cache requires set commandBar in code
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
            ShellPage.Current.ShellControl.SelectItem("Home");
        }

		private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            searchBox.SearchWidth = e.NewSize.Width > 640 ? 230 : 190;
        }

		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(text =>
                {
                    searchBox.Reset();
                    ShellPage.Current.ShellControl.CloseLeftPane();                    
                    NavigationService.NavigateToPage("SearchPage", text, true);
                },
                SearchViewModel.CanSearch);
            }
        }
        void MyVideoAd_AdReady(object sender, object e)
        {
            // code
            Debug.WriteLine("Video Ready" + DateTime.Now);
            if (!bannerready)
            {
                MyVideoAd.Show();
                videoready = true;
            }
        }

        void MyBannerAd_AdReady(object sender, object e)
        {
            // code
            Debug.WriteLine("Banner Ready" + DateTime.Now);
            if (!videoready)
            {
             //   MyBannerAd.Show();
                bannerready = true;
            }
        }

        void MyVideoAd_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            // code

            var A = MyVideoAd.State;
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

        private void Hyperlink_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            var advertisingId = Windows.System.UserProfile.AdvertisingManager.AdvertisingId;
            sender.NavigateUri = new System.Uri("https://dpa-fwl.microsoft.com/redirect.html?referrerID="+ advertisingId +"&rurl=http://reviewsbykiran.blogspot.in/");
        }

        private async void Reviews_Click(Windows.UI.Xaml.Documents.Hyperlink sender, Windows.UI.Xaml.Documents.HyperlinkClickEventArgs args)
        {
            bool result = await Windows.System.Launcher.LaunchUriAsync(new Uri("ms-windows-store://review/?ProductId=9wzdncrdx48s"));
        }

        public async Task<bool> ShowRatingReviewDialog()
        {
            StoreSendRequestResult result = await StoreRequestHelper.SendRequestAsync(
                StoreContext.GetDefault(), 16, String.Empty);

            if (result.ExtendedError == null)
            {
                JObject jsonObject = JObject.Parse(result.Response);
                if (jsonObject.SelectToken("status").ToString() == "success")
                {
                    // The customer rated or reviewed the app.
                    return true;
                }
            }

            // There was an error with the request, or the customer chose not to
            // rate or review the app.
            return false;
        }

    }
}
