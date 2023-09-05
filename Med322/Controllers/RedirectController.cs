using Microsoft.AspNetCore.Mvc;

namespace Med322.Controllers
{
    public class RedirectController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("RoleName") == "Patient")
            {
                return RedirectToAction("Patient", "Index");
            }
            else if (HttpContext.Session.GetString("RoleName") == "Dokter")
            {
                return RedirectToAction("Index", "Doctor");
            }
            else if (HttpContext.Session.GetString("RoleName") == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                HttpContext.Session.SetString("RoleName", "Guest");
                return RedirectToAction("Index", "Guest");
            }
        }
    }
}
