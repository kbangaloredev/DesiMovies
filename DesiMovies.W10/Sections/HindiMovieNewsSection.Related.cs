using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.DynamicStorage;
using Windows.Storage;
using AppStudio.Uwp;

using DesiMovies.Navigation;
using DesiMovies.ViewModels;

namespace DesiMovies.Sections
{
    class HindiMovieNewsSectionRelated : Section<RelatedCollection1Schema>
    {
		private DynamicStorageDataProvider<RelatedCollection1Schema> _dataProvider;	

		public HindiMovieNewsSectionRelated()
        {
            _dataProvider = new DynamicStorageDataProvider<RelatedCollection1Schema>();
        }

        public override async Task<IEnumerable<RelatedCollection1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var selected = connectedItem as AppStudio.DataProviders.Rss.RssSchema;
			if(selected == null)
			{
				return new RelatedCollection1Schema[0];
			}

            var config = new DynamicStorageDataConfig
			{
				Url = new Uri("http://ds.winappstudio.com/api/data/collection?dataRowListId=5ab40d10-7d35-45a9-b72e-49c314120729&appId=0e682729-f943-4eb4-81be-371d561900a8"),
				AppId = "0e682729-f943-4eb4-81be-371d561900a8",
				StoreId = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.StoreId] as string,
				DeviceType = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.DeviceType] as string
			};
			//avoid pagination because in memory filter
			var result = await _dataProvider.LoadDataAsync(config, int.MaxValue);
			return result
					.Where(r => r.Key.ToSafeString() == selected.Author.ToSafeString())
					.ToList();
        }

        public override async Task<IEnumerable<RelatedCollection1Schema>> GetNextPageAsync()
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

        public override ListPageConfig<RelatedCollection1Schema> ListPage
        {
            get 
            {
                return new ListPageConfig<RelatedCollection1Schema>
                {
                    Title = "",

                    LayoutBindings = (viewModel, item) =>
					{
						viewModel.Title = item.Title.ToSafeString();
						viewModel.SubTitle = item.Description.ToSafeString();
						viewModel.Description = null;
						viewModel.ImageUrl = ItemViewModel.LoadSafeUrl(item.ImageUrl.ToSafeString());
					},
                    DetailNavigation = (item) =>
                    {
						return NavInfo.FromPage<Pages.RelatedCollectionDetailPage>(true);
                    }
                };
            }
        }

        public override DetailPageConfig<RelatedCollection1Schema> DetailPage
        {
            get
            {
                return null;
            }
        }
    }
}
