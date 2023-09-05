using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    public class CustomerMemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
