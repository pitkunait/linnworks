using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LinnworksTechTest.Repositories.SalesRecords;
using LinnworksTechTest.Services;
using Microsoft.AspNetCore.Mvc;


namespace LinnworksTechTest.Controllers
{
    [ApiController]
    [Route("api/sales")]
    public class SalesRecordsController : ControllerBase
    {
        private readonly SalesRecordsService _salesRecordsService;

        public SalesRecordsController(SalesRecordsService salesService)
        {
            _salesRecordsService = salesService;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string country,
            [FromQuery] int? year
        )
        {
            var salesRecords = await _salesRecordsService.ListAllRecords(country, year);
            return Ok(salesRecords);
        }


        [HttpPost]
        public async Task<IActionResult> Post(List<SalesRecord> salesRecords)
        {
            await _salesRecordsService.InsertRecords(salesRecords);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(SalesRecord salesRecord)
        {
            await _salesRecordsService.UpdateRecord(salesRecord);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _salesRecordsService.DeleteRecord(id);
            return Ok();
        }
    }
}