using AppStudio.Uwp.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Microsoft.Advertising.WinRT.UI;
using Windows.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using System.Reflection;
using System.Linq;

namespace DesiMovies.Layouts.Detail
{
    public sealed partial class RelatedContentDetailLayout : BaseDetailLayout
    {
        #region RelatedContentTemplate
        public DataTemplate RelatedContentTemplate
        {
            get { return (DataTemplate)GetValue(RelatedContentTemplateProperty); }
            set { SetValue(RelatedContentTemplateProperty, value); }
        }

        public static readonly DependencyProperty RelatedContentTemplateProperty = DependencyProperty.Register("RelatedContentTemplate", typeof(DataTemplate), typeof(BaseDetailLayout), new PropertyMetadata(null));
        #endregion

        NativeAd nativeAd;
        NativeAdsManager myNativeAdsManager = null;
        NativeAdsManager myNativeAdsManager2 = null;
        NativeAdsManager myNativeAdsManager3 = null;
        NativeAdsManager myNativeAdsManager4 = null;

        private StackPanel nativeAdStackPanel;
        private StackPanel nativeAdStackPanel2;
        private StackPanel nativeAdStackPanel3;
        private StackPanel nativeAdStackPanel4;
                
        public RelatedContentDetailLayout()
        {

            //string MyNativeAdUnitId = "test";
            var MyAppID = "9wzdncrdx48s";
             string MyNativeAdUnitId = "0000000019";
             string MyNativeAdUnitId2 = "0000000022";
             string MyNativeAdUnitId3 = "1100018195";
             string MyNativeAdUnitId4 = "1100018194";

           // string MyNativeAdUnitId = "test";
           // string MyNativeAdUnitId2 = "test";
          //  string MyNativeAdUnitId3 = "test";
          //  string MyNativeAdUnitId4 = "test";

            //Native Ad1 events 
            myNativeAdsManager = new NativeAdsManager(MyAppID, MyNativeAdUnitId);
            myNativeAdsManager.AdReady += MyNativeAd_AdReady;
            myNativeAdsManager.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;

            //Native Ad2 events 
            myNativeAdsManager2 = new NativeAdsManager(MyAppID, MyNativeAdUnitId2);
            myNativeAdsManager2.AdReady += MyNativeAd_AdReady2;
            myNativeAdsManager2.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;

            //Native Ad3 events 
            myNativeAdsManager3 = new NativeAdsManager(MyAppID, MyNativeAdUnitId3);
            myNativeAdsManager3.AdReady += MyNativeAd_AdReady3;
            myNativeAdsManager3.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;

            //Native Ad4 events 
            myNativeAdsManager4 = new NativeAdsManager(MyAppID, MyNativeAdUnitId4);
            myNativeAdsManager4.AdReady += MyNativeAd_AdReady4;
            myNativeAdsManager4.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;


            InitializeComponent();
            //Request Natve Ad
            myNativeAdsManager.RequestAd();
            myNativeAdsManager2.RequestAd();
            myNativeAdsManager3.RequestAd();
            myNativeAdsManager4.RequestAd();
        }



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
            TitleTextBlock.Text = nativeAd.Title;

            // Show the ad description.
            if (!string.IsNullOrEmpty(nativeAd.Description))
            {
                DescriptionTextBlock.Text = nativeAd.Description;
                DescriptionTextBlock.Visibility = Visibility.Visible;
            }

            // Display the first main image for the ad. Note that the service
            // might provide multiple main images. 
            if (nativeAd.MainImages.Count > 0)
            {
                NativeImage mainImage = nativeAd.MainImages[0];
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(mainImage.Url);

                MainImageImage.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // main image.
                //MainImageImage.Height = mainImage.Height;
                //MainImageImage.Width = mainImage.Width;
                //MainImageImage.Visibility = Visibility.Visible;
            }

            // Add the call to action string to the button.
            if (!string.IsNullOrEmpty(nativeAd.CallToAction))
            {
                CallToActionButton.Content = nativeAd.CallToAction;
                CallToActionButton.Visibility = Visibility.Visible;
            }

            // Show the ad sponsored by value.
            if ((!string.IsNullOrEmpty(nativeAd.SponsoredBy)) && (nativeAd.IconImage != null))
            {
                {
                    SponsoredByTextBlock.Text = nativeAd.SponsoredBy;
                    SponsoredByTextBlock.Visibility = Visibility.Visible;
                }
                BitmapImage bitmapImage = new BitmapImage();

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

        private void NativeAdStackPanel_Loaded(object sender, RoutedEventArgs e)
        {
            nativeAdStackPanel = sender as StackPanel;
        }

        private void NativeAdStackPanel_Loaded2(object sender, RoutedEventArgs e)
        {
            nativeAdStackPanel2 = sender as StackPanel;
        }

        private void NativeAdStackPanel_Loaded3(object sender, RoutedEventArgs e)
        {
            nativeAdStackPanel3 = sender as StackPanel;
        }

        private void NativeAdStackPanel_Loaded4(object sender, RoutedEventArgs e)
        {
            nativeAdStackPanel4 = sender as StackPanel;
        }


        private void MyNativeAdsManager_ErrorOccurred(object sender, AdErrorEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("NativeAd error " + e.ErrorMessage +
                " ErrorCode: " + e.ErrorCode.ToString());
        }

        // Native Ad#2
        void MyNativeAd_AdReady2(object sender, object e)
        {
            nativeAd = (NativeAd)e;


            if ((nativeAdStackPanel2 == null) || (nativeAd == null))
            {
                return;
            }
            else
            {
                nativeAdStackPanel2.Visibility = Visibility.Visible;
            }

            var children = new List<FrameworkElement>();
            FindChildren(children, nativeAdStackPanel2);

            // Show the ad title.
            TitleTextBlock2.Text = nativeAd.Title;

            // Show the ad description.
            if (!string.IsNullOrEmpty(nativeAd.Description))
            {
                DescriptionTextBlock2.Text = nativeAd.Description;
                DescriptionTextBlock2.Visibility = Visibility.Visible;
            }

            // Display the first main image for the ad. Note that the service
            // might provide multiple main images. 
            if (nativeAd.MainImages.Count > 0)
            {
                NativeImage mainImage = nativeAd.MainImages[0];
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(mainImage.Url);

                MainImageImage2.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // main image.
                //MainImageImage.Height = mainImage.Height;
                //MainImageImage.Width = mainImage.Width;
                //MainImageImage.Visibility = Visibility.Visible;
            }

            // Add the call to action string to the button.
            if (!string.IsNullOrEmpty(nativeAd.CallToAction))
            {
                CallToActionButton2.Content = nativeAd.CallToAction;
                CallToActionButton2.Visibility = Visibility.Visible;
            }

            // Show the ad sponsored by value.
            if ((!string.IsNullOrEmpty(nativeAd.SponsoredBy)) && (nativeAd.IconImage != null))
            {
                {
                    SponsoredByTextBlock2.Text = nativeAd.SponsoredBy;
                    SponsoredByTextBlock2.Visibility = Visibility.Visible;
                }
                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.UriSource = new Uri(nativeAd.IconImage.Url);
                IconImageImage2.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // icon image.
                //               IconImageImage.Height = nativeAd.IconImage.Height;
                //               IconImageImage.Width = nativeAd.IconImage.Width;
                IconImageImage2.Visibility = Visibility.Visible;
            }

            // Register the container of the controls that display
            // the native ad elements for clicks/impressions.
            nativeAd.RegisterAdContainer(nativeAdStackPanel2);
        }

        // Native Ad#2
        void MyNativeAd_AdReady3(object sender, object e)
        {
            nativeAd = (NativeAd)e;


            if ((nativeAdStackPanel3 == null) || (nativeAd == null))
            {
                return;
            }
            else
            {
                nativeAdStackPanel3.Visibility = Visibility.Visible;
            }

            var children = new List<FrameworkElement>();
            FindChildren(children, nativeAdStackPanel3);

            // Show the ad title.
            TitleTextBlock3.Text = nativeAd.Title;

            // Show the ad description.
            if (!string.IsNullOrEmpty(nativeAd.Description))
            {
                DescriptionTextBlock3.Text = nativeAd.Description;
                DescriptionTextBlock3.Visibility = Visibility.Visible;
            }

            // Display the first main image for the ad. Note that the service
            // might provide multiple main images. 
            if (nativeAd.MainImages.Count > 0)
            {
                NativeImage mainImage = nativeAd.MainImages[0];
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(mainImage.Url);

                MainImageImage3.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // main image.
                //MainImageImage.Height = mainImage.Height;
                //MainImageImage.Width = mainImage.Width;
                //MainImageImage.Visibility = Visibility.Visible;
            }

            // Add the call to action string to the button.
            if (!string.IsNullOrEmpty(nativeAd.CallToAction))
            {
                CallToActionButton3.Content = nativeAd.CallToAction;
                CallToActionButton3.Visibility = Visibility.Visible;
            }

            // Show the ad sponsored by value.
            if ((!string.IsNullOrEmpty(nativeAd.SponsoredBy)) && (nativeAd.IconImage != null))
            {
                {
                    SponsoredByTextBlock3.Text = nativeAd.SponsoredBy;
                    SponsoredByTextBlock3.Visibility = Visibility.Visible;
                }
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(nativeAd.IconImage.Url);
                IconImageImage3.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // icon image.
                //               IconImageImage.Height = nativeAd.IconImage.Height;
                //               IconImageImage.Width = nativeAd.IconImage.Width;
                IconImageImage.Visibility = Visibility.Visible;
            }

            // Register the container of the controls that display
            // the native ad elements for clicks/impressions.
            nativeAd.RegisterAdContainer(nativeAdStackPanel3);
        }

        // Native Ad#2
        void MyNativeAd_AdReady4(object sender, object e)
        {
            nativeAd = (NativeAd)e;


            if ((nativeAdStackPanel4 == null) || (nativeAd == null))
            {
                return;
            }
            else
            {
                nativeAdStackPanel4.Visibility = Visibility.Visible;
            }

            var children = new List<FrameworkElement>();
            FindChildren(children, nativeAdStackPanel4);

            // Show the ad title.
            TitleTextBlock4.Text = nativeAd.Title;

            // Show the ad description.
            if (!string.IsNullOrEmpty(nativeAd.Description))
            {
                DescriptionTextBlock4.Text = nativeAd.Description;
                DescriptionTextBlock4.Visibility = Visibility.Visible;
            }

            // Display the first main image for the ad. Note that the service
            // might provide multiple main images. 
            if (nativeAd.MainImages.Count > 0)
            {
                NativeImage mainImage = nativeAd.MainImages[0];
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.UriSource = new Uri(mainImage.Url);
                MainImageImage4.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // main image.
                //MainImageImage.Height = mainImage.Height;
                //MainImageImage.Width = mainImage.Width;
                //MainImageImage.Visibility = Visibility.Visible;
            }

            // Add the call to action string to the button.
            if (!string.IsNullOrEmpty(nativeAd.CallToAction))
            {
                CallToActionButton4.Content = nativeAd.CallToAction;
                CallToActionButton4.Visibility = Visibility.Visible;
            }

            // Show the ad sponsored by value.
            if ((!string.IsNullOrEmpty(nativeAd.SponsoredBy)) && (nativeAd.IconImage != null))
            {
                {
                    SponsoredByTextBlock4.Text = nativeAd.SponsoredBy;
                    SponsoredByTextBlock4.Visibility = Visibility.Visible;
                }
                BitmapImage bitmapImage = new BitmapImage();

                bitmapImage.UriSource = new Uri(nativeAd.IconImage.Url);
                IconImageImage4.Source = bitmapImage;

                // Adjust the Image control to the height and width of the 
                // icon image.
                //               IconImageImage.Height = nativeAd.IconImage.Height;
                //               IconImageImage.Width = nativeAd.IconImage.Width;
                IconImageImage.Visibility = Visibility.Visible;
            }

            // Register the container of the controls that display
            // the native ad elements for clicks/impressions.
            nativeAd.RegisterAdContainer(nativeAdStackPanel4);
        }

    }

  
}
