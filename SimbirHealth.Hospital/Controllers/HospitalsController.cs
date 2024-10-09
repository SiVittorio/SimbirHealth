using Microsoft.AspNetCore.Mvc;

namespace SimbirHealth.Hospital.Controllers
{
    public class HospitalsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
