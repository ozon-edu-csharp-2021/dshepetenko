using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Models;

namespace MerchandiseService.Services.Interfaces
{
    public interface IMerchandiseService
    {
        Task AskMerch(List<MerchItem> merch, long employeeId,CancellationToken token);
        
        Task<List<MerchItem>> InfoAboutMerch(long employeeId, CancellationToken _);
    }
}