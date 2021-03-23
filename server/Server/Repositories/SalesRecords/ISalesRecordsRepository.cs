using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server.Repositories.SalesRecords
{
    public interface ISalesRecordsRepository
    {
        Task<PagedResult<SalesRecord>> GetAsync(
            int page = 1,
            int pageSize = 100,
            string sortColumn = "id",
            string sortDirection = "asc",
            string country = null,
            int? year = null
        );

        Task<IEnumerable<string>> GetAllCountries();
        Task DeleteAsync(List<int> idList);
        Task InsertBulkAsync(IEnumerable<SalesRecord> salesRecords);
        Task InsertAsync(List<SalesRecord> salesRecords);
        Task<IEnumerable<SalesRecord>> FilterByYearAsync(int year);
        Task<IEnumerable<SalesRecord>> FilterByCountry(string country);
        Task<IEnumerable<SalesRecord>> FilterByYearAndCountry(string country, int year);
        Task UpdateAsync(SalesRecord entityToUpdate);
    }
}