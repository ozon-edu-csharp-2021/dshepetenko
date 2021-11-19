using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using MerchandiseService.Domain.AggregationModels.MerchItemAggregate;
using MerchandiseService.Domain.AggregationModels.ValueObjects;
using MerchandiseService.Grpc;
using MerchandiseService.Infrastructure.Commands.InfoAboutMerch;
using MerchandiseService.Infrastructure.Commands.RequestMerch;
using MerchandiseService.Services.Interfaces;
using MerchItem = MerchandiseService.Models.MerchItem;

namespace MerchandiseService.GrpcServices
{
    public class MerchandiseGrpcService : MerchandiseServiceGrpc.MerchandiseServiceGrpcBase
    {
        private readonly IMerchandiseService _merchandiseService;

        private readonly IMediator _mediator;

        public MerchandiseGrpcService(IMerchandiseService merchandiseService, IMediator mediator)
        {
            _merchandiseService = merchandiseService;
            _mediator = mediator;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            var merch = new RequestMerchCommand()
            {
                EmployeeId = request.EmployeeId,
                MerchItems = request.Merch.Select(x => new Domain.AggregationModels.MerchItemAggregate.MerchItem(
                    new Name(x.Name),
                    Size.CreateSize(x.Size),
                    new Sku(x.Sku),
                    MerchType.GetTypeById(x.MerchType),
                    new Quantity(x.Quantity),
                    null)).ToList()
            };

            var result = await _mediator.Send(merch, context.CancellationToken); 

            return new RequestMerchResponse()
            {
                IsApproved = result
            };
        }

        public override async Task<InfoAboutMerchResponse> InfoAboutMerch(InfoAboutMerchRequest request,
            ServerCallContext context)
        {

            var result = await _mediator.Send(new InfoAboutMerchCommand()
            {
                EmployeeId = request.EmployeeId
            }, context.CancellationToken);
            return new InfoAboutMerchResponse()
            {
                Items =
                {
                    result.Select(x => new InfoAboutMerchResponse.Types.InfoAboutMerchUnit()
                    {
                        Name = x.Name.Value,
                        Quantity = x.Quantity.Value,
                        Size = x.Size.Name,
                        Sku = x.Sku.Value,
                        MerchType = x.MerchType.Id
                    }).ToList()
                }
            };
        }
    }
}