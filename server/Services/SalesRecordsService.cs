using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LinnworksTechTest.Repositories.SalesRecords;

namespace LinnworksTechTest.Services
{
    public class SalesRecordsService
    {
        private readonly SalesRecordsRepository _recordsRepository;


        public SalesRecordsService(SalesRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        public async Task<IEnumerable<SalesRecord>> ListAllRecords(string country, int? year)
        {
            IEnumerable<SalesRecord> salesRecords;
            if (country != null & year != null)
            {
                salesRecords = await _recordsRepository.FilterByYearAndCountry(country, year.Value);
            }
            else if (country != null)
            {
                salesRecords = await _recordsRepository.FilterByCountry(country);
            }
            else if (year != null)
            {
                salesRecords = await _recordsRepository.FilterByYearAsync(year.Value);
            }
            else
            {
                salesRecords = await _recordsRepository.GetAllAsync();
            }

            return salesRecords;
        }

        public async Task InsertRecords(List<SalesRecord> salesRecords)
        {
            await _recordsRepository.InsertAsync(salesRecords);
        }

        public async Task DeleteRecord(int id)
        {
            await _recordsRepository.DeleteAsync(id);
        }

        public async Task UpdateRecord(SalesRecord salesRecord)
        {
            await _recordsRepository.UpdateAsync(salesRecord);
        }
    }
}