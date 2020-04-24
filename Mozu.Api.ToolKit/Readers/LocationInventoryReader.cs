using Mozu.Api.Contracts.ProductAdmin;
using Mozu.Api.Resources.Commerce.Catalog.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mozu.Api.ToolKit.Readers
{
    public class LocationInventoryReader : BaseReader
    {
        private LocationInventoryCollection _results;
        public string LocationCode { get; set; }
        protected async override Task<bool> GetDataAsync()
        {
            var resource = new LocationInventoryResource(Context);
            _results = await resource
                .GetLocationInventoriesAsync(LocationCode, startIndex: StartIndex, pageSize: PageSize, sortBy: SortBy,
                    filter: Filter, responseFields: ResponseFields, ct: CancellationToken).ConfigureAwait(false);
            TotalCount = _results.TotalCount;
            PageCount = _results.PageCount;
            PageSize = _results.PageSize;
            return _results.Items != null && _results.Items.Count > 0;
        }
        public async new Task<bool> ReadAsync()
        {
            if (!PageSize.HasValue) PageSize = 20;
            var hasData = await GetDataAsync();
            StartIndex = StartIndex.GetValueOrDefault(0) + PageSize;            
            return hasData;
        }
        public List<LocationInventory> Items
        {
            get { return _results.Items; }
        }
    }
}