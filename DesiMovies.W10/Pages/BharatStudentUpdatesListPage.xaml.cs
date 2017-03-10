//---------------------------------------------------------------------------
//
// <copyright file="BharatStudentUpdatesListPage.xaml.cs" company="Microsoft">
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
    public sealed partial class BharatStudentUpdatesListPage : Page
    {
	    public ListViewModel ViewModel { get; set; }
        public BharatStudentUpdatesListPage()
        {
			ViewModel = ViewModelFactory.NewList(new BharatStudentUpdatesSection());

            this.InitializeComponent();
			commandBar.DataContext = ViewModel;
			NavigationCacheMode = NavigationCacheMode.Enabled;
            Microsoft.HockeyApp.HockeyClient.Current.TrackEvent(this.GetType().FullName);
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
			ShellPage.Current.ShellControl.SelectItem("0d0da0be-5470-4a9c-9667-42702e7ccb22");
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
