using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinnworksTechTest.Repositories.SalesRecords
{
    public interface ISalesRecordsRepository
    {
        public Task DeleteAsync(int id);
        public Task<PagedResult<SalesRecord>> GetAllAsync();
        public Task<SalesRecord> FindAsync(int id);
        public Task InsertAsync(SalesRecord entity);
        public Task UpdateAsync(SalesRecord entityToUpdate);
    }
}