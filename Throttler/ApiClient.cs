namespace Throttler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class ApiClient : IApiClient
    {
        HttpClient httpClient;
        public ApiClient(string ApiUrl)
        {
            httpClient = new HttpClient();
            this.ApiUrl = ApiUrl;
        }

        public string ApiUrl { get; }

        public async Task<bool> PostAsync(int userId)
        {
            await httpClient.PostAsync(ApiUrl + $"/{userId}", new StringContent(userId.ToString()));
            return true;
        }
    }
}
