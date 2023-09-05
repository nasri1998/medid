using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Text;
using Med322.ViewModels;
using System.Net.Http;

namespace Med322.Controllers
{

    public class ProfileController : Controller
    {
		private readonly string apiUrl;
		private readonly HttpClient httpClient = new HttpClient();
		public ProfileController(IConfiguration _config)
		{
			apiUrl = _config["ApiUrl"];
		}
		public IActionResult Index()
        {
            ViewBag.User = "Patient";
            ViewBag.Profile = true;
            ViewBag.Sidebar = "main-panel";
            return View();
        }
    }


}
