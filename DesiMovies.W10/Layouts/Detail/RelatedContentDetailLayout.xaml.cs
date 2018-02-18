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
        
        public RelatedContentDetailLayout()
        {

            //string MyNativeAdUnitId = "test";
            var MyAppID = "9wzdncrdx48s";
            string MyNativeAdUnitId = "0000000019";

            //Native Ad events 
            myNativeAdsManager = new NativeAdsManager(MyAppID, MyNativeAdUnitId);
            myNativeAdsManager.AdReady += MyNativeAd_AdReady;
            myNativeAdsManager.ErrorOccurred += MyNativeAdsManager_ErrorOccurred;

            InitializeComponent();
            //Request Natve Ad
            myNativeAdsManager.RequestAd();
           
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
    }

  
}
