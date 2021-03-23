using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Server.Repositories;
using Server.Repositories.SalesRecords;

namespace Server.Services
{
    public interface ISalesRecordsService
    {
        Task<PagedResult<SalesRecord>> ListAllRecords(
            string sortBy = "id",
            string direction = "asc",
            string country = null,
            int? year = null,
            int pageSize = 100,
            int page = 1);

        Task InsertRecords(List<SalesRecord> salesRecords);
        Task<IEnumerable<string>> GetAllCountries();
        Task InsertBulkRecords(string path);
        Task DeleteRecord(List<int> idList);
        Task UpdateRecord(SalesRecord salesRecord);
        Task<object> UploadCsv(IFormFile file);
    }
}