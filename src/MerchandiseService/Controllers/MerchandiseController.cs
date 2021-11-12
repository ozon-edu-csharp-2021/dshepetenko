using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Infrastructure.Commands.InfoAboutMerch;
using MerchandiseService.Infrastructure.Commands.RequestMerch;
using MerchandiseService.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MerchItem = MerchandiseService.Models.MerchItem;

namespace MerchandiseService.Controllers
{
    [ApiController]
    [Route("v1/api/merch")]
    [Produces("application/json")]
    public class MerchandiseController : ControllerBase
    {
        private readonly IMerchandiseService _merchandiseService;
        private readonly IMediator _mediator;

        public MerchandiseController(IMerchandiseService merchandiseService, IMediator mediator)
        {
            _merchandiseService = merchandiseService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RequestMerchAsync([FromBody] List<MerchItem> merch, long employeeId,
            CancellationToken _)
        {
            var requestMerchCommandHandler = new RequestMerchCommand()
            {
                MerchItems = merch.Select(x => new Domain.AggregationModels.MerchItemAggregate.MerchItem(
                    new Name(x.Name),
                    Size.CreateSize(x.Size),
                    new Sku(x.Sku),
                    new Quantity(x.Quantity),
                    null
                )).ToList(),
                EmployeeId = employeeId
            };

            var result = await _mediator.Send(requestMerchCommandHandler, _);

            return Ok(result);
        }

        [HttpGet("/info/{employeeId:long}")]
        public async Task<ActionResult<List<MerchItem>>> InfoAboutMerchAsync(long employeeId, CancellationToken _)
        {
            var merchInfo = new InfoAboutMerchCommand()
            {
                EmployeeId = employeeId
            };

            var result = await _mediator.Send(merchInfo, _);

            return Ok(result);
        }
    }
}