using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.Twitter;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using AppStudio.Uwp;
using System.Linq;

using DesiMovies.Navigation;
using DesiMovies.ViewModels;

namespace DesiMovies.Sections
{
    public class LatestOnTwitterSection : Section<TwitterSchema>
    {
		private TwitterDataProvider _dataProvider;

		public LatestOnTwitterSection()
		{
			_dataProvider = new TwitterDataProvider(new TwitterOAuthTokens
			{
				ConsumerKey = "cyo86Xw85zAx12KoeUpZKd69H",
                    ConsumerSecret = "3wyMSBw9JX4mALAY4URBQ3pi0IZI71ZH7uYlxwGEXI1EHD8ESj",
                    AccessToken = "15854021-UxIN7POW2gba8Etw15hdI3YcIicbXQLpUWshPyKrp",
                    AccessTokenSecret = "cGa41buWoRofE3x6BNqGcBIjMeeTuHW9YfANQhNFrO49x"
			});
		}

		public override async Task<IEnumerable<TwitterSchema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new TwitterDataConfig
            {
                QueryType = TwitterQueryType.Search,
                Query = @"#Bollywood"
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
        }

        public override async Task<IEnumerable<TwitterSchema>> GetNextPageAsync()
        {
            return await _dataProvider.LoadMoreDataAsync();
        }

        public override bool HasMorePages
        {
            get
            {
                return _dataProvider.HasMoreItems;
            }
        }

        public override ListPageConfig<TwitterSchema> ListPage
        {
            get 
            {
                return new ListPageConfig<TwitterSchema>
                {
                    Title = "latest on Twitter",

                    Page = typeof(Pages.LatestOnTwitterListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
						viewModel.Header = item._id.ToSafeString();
                        viewModel.Title = item.UserName.ToSafeString();
                        viewModel.SubTitle = item.Text.ToSafeString();
                        viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.UserProfileImageUrl.ToSafeString());

						viewModel.GroupBy = item._id.SafeType();

						viewModel.OrderBy = item._id;
                    },
					OrderType = OrderType.Ascending,
                    DetailNavigation = (item) =>
                    {
                        return new NavInfo
                        {
                            NavigationType = NavType.DeepLink,
                            TargetUri = new Uri(item.Url)
                        };
                    }
                };
            }
        }

        public override DetailPageConfig<TwitterSchema> DetailPage
        {
            get { return null; }
        }
    }
}
