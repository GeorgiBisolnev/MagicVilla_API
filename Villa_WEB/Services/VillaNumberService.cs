﻿using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Services
{
    public class VillaNumberService : BaseService, IVIllaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string _urlAPI;
        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration ) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            _urlAPI = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }

        public Task<T> CreateAsyn<T>(VillaNumberCreateDTO dto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = Data.Common.SD.ApiType.POST,
                Data = dto,
                Url = _urlAPI + "/api/VillaNumberAPI"
            });
        }

        public Task<T> DeleteAsyn<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = Data.Common.SD.ApiType.DELETE,
                Url = _urlAPI + "/api/VillaNumberAPI/" + id,
            });
        }

        public Task<T> GetAllAsyc<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = Data.Common.SD.ApiType.GET,
                Url = _urlAPI + "/api/VillaNumberAPI"
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = Data.Common.SD.ApiType.GET,
                Url = _urlAPI + "/api/VillaNumberAPI/" + id
            });
        }

        public Task<T> UpdateAsyn<T>(VillaNumberUpdateDTO dto)
        {            
            return SendAsync<T>(new APIRequest()
            {
                ApiType = Data.Common.SD.ApiType.PUT,
                Data = dto,
                Url = _urlAPI + "/api/VillaNumberAPI/" + dto.VillaNo,
            });
        }
    }
}
