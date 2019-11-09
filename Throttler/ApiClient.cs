// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiClient.cs" company="VMware, Inc">
//  Copyright (c) 2019 VMware, Inc. All rights reserved.
//  This product is protected by copyright and intellectual property laws in the United States and other countries as well as by international treaties.
//  VMware products may be covered by one or more patents listed at http://www.vmware.com/go/patents
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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
