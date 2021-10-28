using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MerchandiseService.HttpModels;

namespace MerchandiseService.HttpClient
{
    public interface IMerchandiseHttpClient
    {
        Task AskMerch(List<MerchItemResponse> merch, long employeeId, CancellationToken token);
        Task<List<MerchItemResponse>> InfoAboutMerch(long employeeId, CancellationToken token);
    }

    public class MerchandiseHttpClient : IMerchandiseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public MerchandiseHttpClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AskMerch(List<MerchItemResponse> merch, long employeeId, CancellationToken token)
        {
            using (var response = await _httpClient.GetAsync("v1/api/merch", token))
                return;
        }

        public async Task<List<MerchItemResponse>> InfoAboutMerch(long employeeId, CancellationToken token)
        {
            using var response = await _httpClient.GetAsync("v1/api/merch", token);
            var body = await response.Content.ReadAsStringAsync(token);
            return JsonSerializer.Deserialize<List<MerchItemResponse>>(body);
        }
    }
}