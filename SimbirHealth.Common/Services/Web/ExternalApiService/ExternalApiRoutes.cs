namespace SimbirHealth.Common.Services.Web.ExternalApiService
{
    /// <summary>
    /// Модель для хранения адресов внешних API
    /// </summary>
    public class ExternalApiRoutes
    {
        public ExternalApiRoutes() { }
        public ExternalApiRoutes(string accountApi, string hospitalApi)
        {
            AccountApi = accountApi;
            HospitalApi = hospitalApi;
        }

        public string AccountApi { get; set; }
        public string HospitalApi { get; set; }
    }
}