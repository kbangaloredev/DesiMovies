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

using Windows.UI.Xaml.Media.Imaging;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using System.Reflection;
using System.Linq;

namespace DesiMovies.Pages
{
    public sealed partial class HomePage : Page
    {
        InterstitialAd MyVideoAd;
        InterstitialAd MyBannerAd;
        NativeAd nativeAd;


        bool bannerready, videoready;

        NativeAdsManager myNativeAdsManager = null;

        public HomePage()
        {


            var MyAppID = "9wzdncrdx48s";
            // video adunit
            var MyVideoAdUnitId = "11647923";
            //var MyVideoAdUnitId = "test";

            //NativeAdsManager myNativeAdsManager = null;
            //string MyNativeAdUnitId = "test";
             string MyNativeAdUnitId = "0000000022";

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


            //Native Ad events 
            myNativeAdsManager = new NativeAdsManager(MyAppID, MyNativeAdUnitId);
            myNativeAdsManager.AdReady += MyNativeAd_AdReady;
            myNativeAdsManager.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;

            //Request Natve Ad
            myNativeAdsManager.RequestAd();

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


        private StackPanel nativeAdStackPanel;

        internal static void FindChildren<T>(List<T> results, DependencyObject startNode)
            where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().GetTypeInfo().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }
                FindChildren<T>(results, current);
            }
        }


        void MyNativeAd_AdReady(object sender, object e)
        {
            nativeAd = (NativeAd)e;

            if ((nativeAdStackPanel == null) || (nativeAd == null))
            {
                return;
            }
            else
            {
                nativeAdStackPanel.Visibility = Visibility.Visible;
            }

            var children = new List<FrameworkElement>();
            FindChildren(children, nativeAdStackPanel);

            // Show the ad title.
            var textBlock = (TextBlock)GetChildWithName(children, "TitleTextBlock");
            textBlock.Text = nativeAd.Title;

            // Show the ad description.
            if (!string.IsNullOrEmpty(nativeAd.Description))
            {
                var descriptionBlock = (TextBlock)GetChildWithName(children, "DescriptionTextBlock");

                descriptionBlock.Text = nativeAd.Description;
                descriptionBlock.Visibility = Visibility.Visible;
            }

            // Display the first main image for the ad. Note that the service
            // might provide multiple main images. 
            if (nativeAd.MainImages.Count > 0)
            {
                NativeImage mainImage = nativeAd.MainImages[0];
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(mainImage.Url);

                var mainImageElement = (Image)GetChildWithName(children, "MainImageImage");
                mainImageElement.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // main image.
                //MainImageImage.Height = mainImage.Height;
                //MainImageImage.Width = mainImage.Width;
                //MainImageImage.Visibility = Visibility.Visible;
            }

            // Add the call to action string to the button.
            if (!string.IsNullOrEmpty(nativeAd.CallToAction))
            {
                var callToActionButtonElement = (Button)GetChildWithName(children, "CallToActionButton");
                callToActionButtonElement.Content = nativeAd.CallToAction;
                callToActionButtonElement.Visibility = Visibility.Visible;
            }

            // Show the ad sponsored by value.
            if ((!string.IsNullOrEmpty(nativeAd.SponsoredBy)) && (nativeAd.IconImage != null))
            {
                {
                    var SponsoredByTextBlock = (TextBlock)GetChildWithName(children, "SponsoredByTextBlock");
                    SponsoredByTextBlock.Text = nativeAd.SponsoredBy;
                    SponsoredByTextBlock.Visibility = Visibility.Visible;
                }


                    BitmapImage bitmapImage = new BitmapImage();

                    var IconImageImage = (Image)GetChildWithName(children, "IconImageImage");
                    bitmapImage.UriSource = new Uri(nativeAd.IconImage.Url);
                    IconImageImage.Source = bitmapImage;

                    // Adjust the Image control to the height and width of the 
                    // icon image.
                    //               IconImageImage.Height = nativeAd.IconImage.Height;
                    //               IconImageImage.Width = nativeAd.IconImage.Width;
                    IconImageImage.Visibility = Visibility.Visible;
            }    

            // Register the container of the controls that display
            // the native ad elements for clicks/impressions.
            nativeAd.RegisterAdContainer(nativeAdStackPanel);
        }

        private FrameworkElement GetChildWithName(List<FrameworkElement> children, string name)
        {
            return children.First(c => c.Name == name);
        }

        private void NativeAdStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            nativeAdStackPanel = sender as StackPanel;
        }


        private void MyNativeAdsManager_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("NativeAd error " + e.ErrorMessage +
                " ErrorCode: " + e.ErrorCode.ToString());
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
