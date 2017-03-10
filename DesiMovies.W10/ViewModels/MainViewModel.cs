using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Windows.Input;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Navigation;
using AppStudio.Uwp.Commands;
using AppStudio.DataProviders;

using AppStudio.DataProviders.YouTube;
using AppStudio.DataProviders.Rss;
using AppStudio.DataProviders.Twitter;
using AppStudio.DataProviders.LocalStorage;
using AppStudio.DataProviders.DynamicStorage;
using DesiMovies.Sections;


namespace DesiMovies.ViewModels
{
    public class MainViewModel : PageViewModelBase
    {
        public ListViewModel LatestVideos { get; private set; }
        public ListViewModel HindiMovieNews { get; private set; }
        public ListViewModel LatestBollywoodGossip { get; private set; }
        public ListViewModel MusicReviews { get; private set; }
        public ListViewModel ReviewsByKiran { get; private set; }
        public ListViewModel NowRunningUpdates { get; private set; }
        public ListViewModel BharatStudentUpdates { get; private set; }
        public ListViewModel LatestOnTwitter { get; private set; }
        public ListViewModel Bubble { get; private set; }
		public AdvertisingViewModel SectionAd { get; set; }

        public MainViewModel(int visibleItems) : base()
        {
            Title = "desi movies";
			this.SectionAd = new AdvertisingViewModel();
            LatestVideos = ViewModelFactory.NewList(new LatestVideosSection(), visibleItems);
            HindiMovieNews = ViewModelFactory.NewList(new HindiMovieNewsSection(), visibleItems);
            LatestBollywoodGossip = ViewModelFactory.NewList(new LatestBollywoodGossipSection(), visibleItems);
            MusicReviews = ViewModelFactory.NewList(new MusicReviewsSection(), visibleItems);
            ReviewsByKiran = ViewModelFactory.NewList(new ReviewsByKiranSection(), visibleItems);
            NowRunningUpdates = ViewModelFactory.NewList(new NowRunningUpdatesSection(), visibleItems);
            BharatStudentUpdates = ViewModelFactory.NewList(new BharatStudentUpdatesSection(), visibleItems);
            LatestOnTwitter = ViewModelFactory.NewList(new LatestOnTwitterSection(), visibleItems);
            Bubble = ViewModelFactory.NewList(new BubbleSection(), visibleItems);

            if (GetViewModels().Any(vm => !vm.HasLocalData))
            {
                Actions.Add(new ActionInfo
                {
                    Command = RefreshCommand,
                    Style = ActionKnownStyles.Refresh,
                    Name = "RefreshButton",
                    ActionType = ActionType.Primary
                });
            }
        }

		#region Commands
		public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(async () =>
                {
                    var refreshDataTasks = GetViewModels()
                        .Where(vm => !vm.HasLocalData).Select(vm => vm.LoadDataAsync(true));

                    await Task.WhenAll(refreshDataTasks);
					LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
                    OnPropertyChanged("LastUpdated");
                });
            }
        }
		#endregion

        public async Task LoadDataAsync()
        {
            var loadDataTasks = GetViewModels().Select(vm => vm.LoadDataAsync());

            await Task.WhenAll(loadDataTasks);
			LastUpdated = GetViewModels().OrderBy(vm => vm.LastUpdated, OrderType.Descending).FirstOrDefault()?.LastUpdated;
            OnPropertyChanged("LastUpdated");
        }

        private IEnumerable<ListViewModel> GetViewModels()
        {
            yield return LatestVideos;
            yield return HindiMovieNews;
            yield return LatestBollywoodGossip;
            yield return MusicReviews;
            yield return ReviewsByKiran;
            yield return NowRunningUpdates;
            yield return BharatStudentUpdates;
            yield return LatestOnTwitter;
            yield return Bubble;
        }
    }
}
