using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using AppStudio.DataProviders;
using AppStudio.DataProviders.Core;
using AppStudio.DataProviders.DynamicStorage;
using AppStudio.Uwp;
using AppStudio.Uwp.Actions;
using AppStudio.Uwp.Commands;
using Windows.ApplicationModel.Appointments;
using System.Linq;
using Windows.Storage;

using DesiMovies.Navigation;
using DesiMovies.ViewModels;

namespace DesiMovies.Sections
{
    public class RelatedCollectionSection : Section<RelatedCollection1Schema>
    {
		private DynamicStorageDataProvider<RelatedCollection1Schema> _dataProvider;		

		public RelatedCollectionSection()
		{
			_dataProvider = new DynamicStorageDataProvider<RelatedCollection1Schema>();
		}

		public override async Task<IEnumerable<RelatedCollection1Schema>> GetDataAsync(SchemaBase connectedItem = null)
        {
            var config = new DynamicStorageDataConfig
            {
                Url = new Uri("http://ds.winappstudio.com/api/data/collection?dataRowListId=5ab40d10-7d35-45a9-b72e-49c314120729&appId=0e682729-f943-4eb4-81be-371d561900a8"),
                AppId = "0e682729-f943-4eb4-81be-371d561900a8",
                StoreId = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.StoreId] as string,
                DeviceType = ApplicationData.Current.LocalSettings.Values[LocalSettingNames.DeviceType] as string,
            };
            return await _dataProvider.LoadDataAsync(config, MaxRecords);
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
                    Title = "Related collection",

                    Page = typeof(Pages.RelatedCollectionListPage),

                    LayoutBindings = (viewModel, item) =>
                    {
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
                var bindings = new List<Action<ItemViewModel, RelatedCollection1Schema>>();
                bindings.Add((viewModel, item) =>
                {
                    viewModel.PageTitle = item.Key.ToSafeString();
                    viewModel.Title = "";
                    viewModel.Description = "";
                    viewModel.ImageUrl = ItemViewModel.LoadSafeUrl("");
                    viewModel.Content = null;
                });

                var actions = new List<ActionConfig<RelatedCollection1Schema>>
                {
                };

                return new DetailPageConfig<RelatedCollection1Schema>
                {
                    Title = "Related collection",
                    LayoutBindings = bindings,
                    Actions = actions
                };
            }
        }
    }
}
