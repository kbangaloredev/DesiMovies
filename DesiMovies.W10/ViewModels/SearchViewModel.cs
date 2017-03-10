using System;
using System.Collections.Generic;
using AppStudio.Uwp;
using AppStudio.Uwp.Commands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DesiMovies.Sections;
namespace DesiMovies.ViewModels
{
    public class SearchViewModel : PageViewModelBase
    {
        public SearchViewModel() : base()
        {
			Title = "desi movies";
            LatestVideos = ViewModelFactory.NewList(new LatestVideosSection());
            HindiMovieNews = ViewModelFactory.NewList(new HindiMovieNewsSection());
            LatestBollywoodGossip = ViewModelFactory.NewList(new LatestBollywoodGossipSection());
            MusicReviews = ViewModelFactory.NewList(new MusicReviewsSection());
            ReviewsByKiran = ViewModelFactory.NewList(new ReviewsByKiranSection());
            NowRunningUpdates = ViewModelFactory.NewList(new NowRunningUpdatesSection());
            BharatStudentUpdates = ViewModelFactory.NewList(new BharatStudentUpdatesSection());
            LatestOnTwitter = ViewModelFactory.NewList(new LatestOnTwitterSection());
            Bubble = ViewModelFactory.NewList(new BubbleSection());
            RelatedCollection = ViewModelFactory.NewList(new RelatedCollectionSection());
                        
        }

        private string _searchText;
        private bool _hasItems = true;

        public string SearchText
        {
            get { return _searchText; }
            set { SetProperty(ref _searchText, value); }
        }

        public bool HasItems
        {
            get { return _hasItems; }
            set { SetProperty(ref _hasItems, value); }
        }

		public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand<string>(
                async (text) =>
                {
                    await SearchDataAsync(text);
                }, SearchViewModel.CanSearch);
            }
        }      
        public ListViewModel LatestVideos { get; private set; }
        public ListViewModel HindiMovieNews { get; private set; }
        public ListViewModel LatestBollywoodGossip { get; private set; }
        public ListViewModel MusicReviews { get; private set; }
        public ListViewModel ReviewsByKiran { get; private set; }
        public ListViewModel NowRunningUpdates { get; private set; }
        public ListViewModel BharatStudentUpdates { get; private set; }
        public ListViewModel LatestOnTwitter { get; private set; }
        public ListViewModel Bubble { get; private set; }
        public ListViewModel RelatedCollection { get; private set; }
        public async Task SearchDataAsync(string text)
        {
            this.HasItems = true;
            SearchText = text;
            var loadDataTasks = GetViewModels()
                                    .Select(vm => vm.SearchDataAsync(text));

            await Task.WhenAll(loadDataTasks);
			this.HasItems = GetViewModels().Any(vm => vm.HasItems);
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
            yield return RelatedCollection;
        }
		private void CleanItems()
        {
            foreach (var vm in GetViewModels())
            {
                vm.CleanItems();
            }
        }
		public static bool CanSearch(string text) { return !string.IsNullOrWhiteSpace(text) && text.Length >= 3; }
    }
}
