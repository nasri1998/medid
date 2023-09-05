using Med322.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MiniPro322.Controllers
{
    public class PatientController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();

        public PatientController(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("RoleName") != "Patient")
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }
            return View();
        }

        public IActionResult ProfilePatient()
        {
            return View("~/Views/Profile/Index.cshtml");

        } 
    }
}
