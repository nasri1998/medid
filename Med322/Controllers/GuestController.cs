using Med322.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MiniPro322.Controllers
{
    public class GuestController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string imgFolder;

        public GuestController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            apiUrl = _config["ApiUrl"];
            webHostEnvironment = _webHostEnv;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("RoleName") != "Guest")
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            return View();
        }

    }

}