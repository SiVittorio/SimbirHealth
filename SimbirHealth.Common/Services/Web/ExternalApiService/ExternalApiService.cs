using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimbirHealth.Data.SharedResponses.Account;
using SimbirHealth.Data.SharedResponses.Hospital;

namespace SimbirHealth.Common.Services.Web.ExternalApiService
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly ExternalApiRoutes _routes;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExternalApiService> _logger;
        public ExternalApiService(IOptions<ExternalApiRoutes> routes,
            IHttpClientFactory httpClientFactory,
            ILogger<ExternalApiService> logger)
        {
            _routes = routes.Value;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }
        public async Task<DoctorResponse?> GetDoctorByGuid(Guid doctorGuid, string accessToken)
        {
            var addr = string.Format("{0}/api/Doctors/{1}", _routes.AccountApi, doctorGuid.ToString());

            DoctorResponse? doctor = await GetTFromExternalApiAsync<DoctorResponse>(addr, accessToken);

            return doctor;
        }

        public async Task<HospitalResponse?> GetHospitalByGuid(Guid hospitalGuid, string accessToken)
        {
            var addr = string.Format("{0}/api/Hospitals/{1}", _routes.HospitalApi, hospitalGuid.ToString());

            HospitalResponse? hospital = await GetTFromExternalApiAsync<HospitalResponse>(addr, accessToken);

            return hospital;
        }

        public async Task<List<RoomResponse>?> GetHospitalRoomsByGuid(Guid hospitalGuid, string accessToken)
        {
            var addr = string.Format("{0}/api/Hospitals/{1}/Rooms", _routes.HospitalApi, hospitalGuid.ToString());

            var rooms = await GetTFromExternalApiAsync<List<RoomResponse>>(addr, accessToken);

            return rooms;
        }

        public async Task<IDictionary<string, object>?> ValidateToken(string accessToken){
            var addr = string.Format("{0}/api/Authentication/Validate?accessToken={1}", _routes.AccountApi, accessToken);

            IDictionary<string, object>? result = await _httpClient.GetFromJsonAsync<IDictionary<string, object>>(addr);
            return result;
        }
        private async Task<T?> GetTFromExternalApiAsync<T>(string uri, string accessToken)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            T? result = default;
            try { result = await _httpClient.GetFromJsonAsync<T>(uri); }
            catch (HttpRequestException httpEx) { _logger.LogError(httpEx.Message); }
            return result;
        }
    }
}