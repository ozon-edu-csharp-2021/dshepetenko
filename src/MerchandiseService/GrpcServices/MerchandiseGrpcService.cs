using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MerchandiseService.Grpc;
using MerchandiseService.Models;
using MerchandiseService.Services.Interfaces;

namespace MerchandiseService.GrpcServices
{
    public class MerchandiseGrpcService : MerchandiseServiceGrpc.MerchandiseServiceGrpcBase
    {
        private readonly IMerchandiseService _merchandiseService;

        public MerchandiseGrpcService(IMerchandiseService merchandiseService)
        {
            _merchandiseService = merchandiseService;
        }

        public override async Task<RequestMerchResponse> RequestMerch(RequestMerchRequest request,
            ServerCallContext context)
        {
            var merch = request.Merch
                .Select(x => new MerchItem
                {
                    Name = x.Name,
                    Size = x.Size,
                    Sku = x.Sku,
                    Quantity = x.Quantity
                })
                .ToList();

            var isApproved =
                await _merchandiseService.RequestMerchAsync(merch, request.EmployeeId, context.CancellationToken);
            return new RequestMerchResponse()
            {
                IsApproved = isApproved
            };
        }

        public override async Task<InfoAboutMerchResponse> InfoAboutMerch(InfoAboutMerchRequest request,
            ServerCallContext context)
        {
            var merchInfo =
                await _merchandiseService.InfoAboutMerchAsync(request.EmployeeId, context.CancellationToken);
            return new InfoAboutMerchResponse()
            {
                Items =
                {
                    merchInfo.Select(x => new InfoAboutMerchResponse.Types.InfoAboutMerchUnit()
                    {
                        Name = x.Name,
                        Quantity = x.Quantity,
                        Size = x.Size,
                        Sku = x.Sku
                    })
                }
            };
        }
    }
}