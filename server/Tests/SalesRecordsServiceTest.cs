using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Moq;
using Server.Controllers;
using Server.Repositories;
using Server.Repositories.SalesRecords;
using Server.Services;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;


namespace Tests
{
    public class SalesRecordsServiceTest
    {
        private readonly ITestOutputHelper output;
        private readonly IEnumerable<SalesRecord> _records;

        public SalesRecordsServiceTest(ITestOutputHelper output)
        {
            this.output = output;
            var csv = "../../../../Server/Resources/Uploads/1000 Sales Records (1).csv";
            var reader = new StreamReader(csv);
            var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
            _records = csvReader.GetRecords<SalesRecord>();
        }

        [Fact]
        public async void TestServiceListsItems()
        {
            var mock = new Mock<ISalesRecordsRepository>();
            var response = new PagedResult<SalesRecord>
            {
                Items = _records.Take(100),
                Page = 1,
                HasNext = true,
                TotalCount = 1000,
                PageSize = 100
            };

            mock.Setup(e => e.GetAsync(1, 100, "id", "asc", "Estonia", 1234).Result).Returns(response);
            var service = new SalesRecordsService(mock.Object);
            var result = await service.ListAllRecords("id", "asc", "Estonia", 1234, 100, 1);
            
            Assert.Equal(100, result.Items.Count());
        }
    }
}