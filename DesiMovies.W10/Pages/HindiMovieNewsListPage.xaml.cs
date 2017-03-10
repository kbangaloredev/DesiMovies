//---------------------------------------------------------------------------
//
// <copyright file="HindiMovieNewsListPage.xaml.cs" company="Microsoft">
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

namespace DesiMovies.Pages
{
    public sealed partial class HindiMovieNewsListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public HindiMovieNewsListPage()
        {
			ViewModel = ViewModelFactory.NewList(new HindiMovieNewsSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("5702db7c-5e17-4abe-9c84-ac8b3b326dd3");
			ShellPage.Current.ShellControl.SetCommandBar(commandBar);
			if (e.NavigationMode == NavigationMode.New)
            {			
				await this.ViewModel.LoadDataAsync();
                this.ScrollToTop();
			}			
            base.OnNavigatedTo(e);
        }

    }
}
