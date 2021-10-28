using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Models;
using MerchandiseService.Services.Interfaces;

namespace MerchandiseService.Services
{
    public class MerchService : IMerchandiseService
    {
        public Task AskMerch(List<MerchItem> merch, long employeeId,CancellationToken token)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MerchItem>> InfoAboutMerch(long employeeId, CancellationToken _)
        {
            throw new System.NotImplementedException();
        }
    }
}