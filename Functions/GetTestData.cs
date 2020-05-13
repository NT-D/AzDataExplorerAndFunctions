using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using CseSample.Services;

namespace CseSample
{
    public class GetTestData
    {
        private ITestTableService _testTableService;
        public GetTestData(ITestTableService testTableService)
        {
            _testTableService = testTableService;
        }

        [FunctionName("GetTestData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            if (!int.TryParse(req.Query["durationDay"], out int durationDays)) return new BadRequestObjectResult("durationDay should be positive value");

            try
            {
                var result = await _testTableService.GetLatestNDaysData(durationDays);
                return new OkObjectResult(result);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch(Exception ex)
            {
                log.LogCritical(ex.Message);
                throw;
            }
        }
    }
}
