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
        Task<bool> RequestMerchAsync(List<MerchItemResponse> merch, long employeeId, CancellationToken token);
        Task<List<MerchItemResponse>> InfoAboutMerchAsync(long employeeId, CancellationToken token);
    }

    public class MerchandiseHttpClient : IMerchandiseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        public MerchandiseHttpClient(System.Net.Http.HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> RequestMerchAsync(List<MerchItemResponse> merch, long employeeId,
            CancellationToken token)
        {
            var response = await _httpClient.GetAsync("v1/api/merch", token);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync(token);
                return JsonSerializer.Deserialize<bool>(result);
            }
            return false;
        }

        public async Task<List<MerchItemResponse>> InfoAboutMerchAsync(long employeeId, CancellationToken token)
        {
            using var response = await _httpClient.GetAsync("v1/api/merch", token);
            if (response.IsSuccessStatusCode)
            {
                var body = await response.Content.ReadAsStringAsync(token);
                return JsonSerializer.Deserialize<List<MerchItemResponse>>(body);
            }
            return null;
        }
    }
}