using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using SimbirHealth.Timetable.Models.Data;
using SimbirHealth.Timetable.Models.Responses;

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
            var googleAddr = string.Format("http://{0}.{1}", "google", "com");

            var httpClient = _httpClientFactory
                .CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await httpClient.GetAsync(addr);
            var rps = response.IsSuccessStatusCode;
            //var doctorResponse = JsonConvert.DeserializeObject<DoctorResponse>(response);
            return null;
        }
    }
}