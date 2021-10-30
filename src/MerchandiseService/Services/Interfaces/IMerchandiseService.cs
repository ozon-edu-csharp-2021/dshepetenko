using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.Models;

namespace MerchandiseService.Services.Interfaces
{
    public interface IMerchandiseService
    {
        Task<bool> RequestMerchAsync(List<MerchItem> merch, long employeeId,CancellationToken token);
        
        Task<List<MerchItem>> InfoAboutMerchAsync(long employeeId, CancellationToken _);
    }
}