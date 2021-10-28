using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Models;
using MerchandiseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMerchandiseService _merchandiseService;

        public MerchandiseController(IMerchandiseService merchandiseService)
        {
            _merchandiseService = merchandiseService;
        }

        [HttpPost]
        public async Task<IActionResult> AskMerch(List<MerchItem> merch, long employeeId, CancellationToken _)
        {
            await _merchandiseService.AskMerch(merch, employeeId, _);
            return Ok();
        }

        [HttpGet("employeeId:long")]
        public async Task<ActionResult<List<MerchItem>>> InfoAboutMerch(long employeeId, CancellationToken _)
        {
            var merchInfo =await _merchandiseService.InfoAboutMerch(employeeId, _);
            if (merchInfo is null)
            {
                return NotFound();
            }
            return Ok(merchInfo);
        }
    }
}