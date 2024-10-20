using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimbirHealth.Data.SharedResponses;
using SimbirHealth.Timetable.Models.Data;

namespace SimbirHealth.Timetable.Services.ExternalApiService
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly ExternalApiRoutes _routes;
        private readonly IHttpClientFactory _httpClientFactory;
        public ExternalApiService(IOptions<ExternalApiRoutes> routes,
            IHttpClientFactory httpClientFactory){
            _routes = routes.Value;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<string?> GetDoctorByGuid(Guid doctorGuid, string accessToken){
            var addr = string.Format("{0}/api/Doctors/{1}", _routes.AccountApi, doctorGuid);

            var httpClient = _httpClientFactory
                .CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
            var response = await httpClient.GetAsync(addr);
            DoctorResponse? doctor = null;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                doctor = JsonConvert.DeserializeObject<DoctorResponse>(response.Content.ToString());
            }
            return doctor != null ? doctor.FirstName : "";
        }
    }
}