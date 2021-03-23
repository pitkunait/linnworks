using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Repositories.SalesRecords;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
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
            [FromQuery] string sortBy,
            [FromQuery] string direction,
            [FromQuery] string country,
            [FromQuery] int? year,
            [FromQuery] int page = 1
        )
        {
            try
            {
                var salesRecords = await _salesRecordsService.ListAllRecords(sortBy, direction, country, year, page);
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

        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery]List<int> id)
        {
            Console.Write(id);
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
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                
                var result = await _salesRecordsService.GetAllCountries();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}