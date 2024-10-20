using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimbirHealth.Timetable.Models.Data
{
    public class ExternalApiRoutes{
        public ExternalApiRoutes(){}
        public ExternalApiRoutes(string accountApi, string hospitalApi)
        {
            AccountApi = accountApi;
            HospitalApi = hospitalApi;
        }

        public string AccountApi { get; set; }
        public string HospitalApi { get; set; }
    }
}