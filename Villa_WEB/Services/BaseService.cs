﻿using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Villa_WEB.Models;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get ; set ; }
        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new APIResponse();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("VillaAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.Url);

                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data),
                        Encoding.UTF8, "applicaion/json");
                }

                switch (apiRequest.ApiType)
                {
                    case Data.Common.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                            break;
                    case Data.Common.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Data.Common.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default: 
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return APIResponse;
            }

            catch  (Exception ex) 
            {
                var dto = new APIResponse() 
                { 
                    Errors = new List<string> { Convert.ToString(ex) },
                    IsSuccess = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}