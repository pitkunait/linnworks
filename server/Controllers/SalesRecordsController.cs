using System;
using System.Collections.Generic;
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
            [FromQuery] int? year,
            [FromQuery] int page = 1
        )
        {
            try
            {
                var salesRecords = await _salesRecordsService.ListAllRecords(country, year, page);
                return Ok(salesRecords);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post(List<SalesRecord> salesRecords)
        {
            try
            {
                await _salesRecordsService.InsertRecords(salesRecords);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> PostUploadAsync()
        {
            try
            {
                var file = Request.Form.Files[0];
                var result = await _salesRecordsService.UploadCSV(file);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("import")]
        public async Task<IActionResult> PostImportAsync([FromQuery] string path)
        {
            try
            {
                await _salesRecordsService.InsertBulkRecords(path);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(SalesRecord salesRecord)
        {
            try
            {
                await _salesRecordsService.UpdateRecord(salesRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _salesRecordsService.DeleteRecord(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}