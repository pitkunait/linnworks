using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CsvHelper;
using LinnworksTechTest.Repositories;
using LinnworksTechTest.Repositories.SalesRecords;
using Microsoft.AspNetCore.Http;

namespace LinnworksTechTest.Services
{
    public class SalesRecordsService
    {
        private readonly SalesRecordsRepository _recordsRepository;


        public SalesRecordsService(SalesRecordsRepository recordsRepository)
        {
            _recordsRepository = recordsRepository;
        }

        public async Task<PagedResult<SalesRecord>> ListAllRecords(
            string sortBy = "id",
            string direction = "asc",
            string country = null,
            int? year = null,
            int page = 1)
        {
            PagedResult<SalesRecord> salesRecords;

            salesRecords = await _recordsRepository.GetAsync(
                page, 
                sortColumn: sortBy, 
                sortDirection: direction,
                country:country, 
                year:year);

            return salesRecords;
        }

        public async Task InsertRecords(List<SalesRecord> salesRecords)
        {
            await _recordsRepository.InsertAsync(salesRecords);
        }
        
        public async Task<IEnumerable<string>> GetAllCountries()
        {
            return await _recordsRepository.GetAllCountries();
        }

        public async Task InsertBulkRecords(string path)
        {
            var reader = new StreamReader(path);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csvReader.GetRecords<SalesRecord>();
            await _recordsRepository.InsertBulkAsync(records);
        }

        public async Task DeleteRecord(List<int> idList)
        {
            await _recordsRepository.DeleteAsync(idList);
        }

        public async Task UpdateRecord(SalesRecord salesRecord)
        {
            await _recordsRepository.UpdateAsync(salesRecord);
        }

        public async Task<object> UploadCSV(IFormFile file)
        {
            var folderName = Path.Combine("Resources", "Uploads");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName?.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return new {dbPath};
        }
    }
}