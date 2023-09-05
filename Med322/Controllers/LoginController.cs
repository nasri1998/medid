using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace MiniPro322.Controllers
{
    public class LoginController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        public LoginController(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }

        public IActionResult Index()
        {
            VMUser data = new VMUser();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> LoginVerification(VMUser inputLogin)
        {
            try
            {
                int userLoginId = 0;

                VMResponse? apiUserResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "User/GetAll"));

                if (apiUserResponse == null)
                {
                    HttpContext.Session.SetString("ErrMsg", "Api Connection Error!");
                    throw new ArgumentNullException($"Failed to get user data due API connection error");
                }

                if (apiUserResponse.Success || apiUserResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiUserResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Failed to get user data"); }

                List<VMUser>? dataUsers = JsonConvert.DeserializeObject<List<VMUser>>(apiUserResponse.data.ToString());

                foreach (VMUser dataUser in dataUsers)
                {
                    if ((inputLogin.Email == dataUser.Email) && (inputLogin.Password == dataUser.Password))
                    {
                        userLoginId = (int)dataUser.Id;
                    }

                }

                if (userLoginId == 0)
                {
                    HttpContext.Session.SetString("ErrMsg", "Email atau Password salah");
                    return Json(new { success = false });
                }

                VMResponse? apiLoginResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Userdata/" + userLoginId));

                if (apiLoginResponse == null)
                {
                    HttpContext.Session.SetString("ErrMsg", "Api Connection Error!");
                    throw new ArgumentNullException($"Failed to get user data due API connection error");
                }

                if (apiLoginResponse.Success || apiLoginResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiLoginResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Failed to get user data"); }

                VMUserData? SessionData = JsonConvert.DeserializeObject<VMUserData>(apiLoginResponse.data.ToString());

                HttpContext.Session.SetString("UserID", SessionData.UserId.ToString() ?? "0");
                HttpContext.Session.SetString("BiodataID", SessionData.BiodataId.ToString() ?? "0");
                HttpContext.Session.SetString("RoleID", SessionData.RoleId.ToString() ?? "0");
                HttpContext.Session.SetString("RoleName", SessionData.RoleName ?? "Guest");
                HttpContext.Session.SetString("FullName", SessionData.Fullname ?? "Guest");
                HttpContext.Session.SetString("ImagePath", SessionData.ImagePath ?? "");
                HttpContext.Session.SetString("DoctorId", SessionData.DoctorId.ToString() ?? "0");

                //if (HttpContext.Session.GetString("RoleName") == "Patient")
                //{
                //    return RedirectToAction("Index", "Patient");
                //}
                //else if (HttpContext.Session.GetString("RoleName") == "Dokter")
                //{
                //    return RedirectToAction("Index", "Doctor");
                //}
                //else if (HttpContext.Session.GetString("RoleName") == "Admin")
                //{
                //    return RedirectToAction("Index", "Admin");
                //}
                //else
                //{
                //    return RedirectToAction("Index", "Guest");
                //}
                return Json(new { success = true, redirectToUrl = Url.Action("Index", SessionData.RoleName) });
            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", e.Message);
                return Json(new { success = false });
            }

        }
        public IActionResult SignIn()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("UserID", string.Empty);
            HttpContext.Session.SetString("BiodataID", string.Empty);
            HttpContext.Session.SetString("RoleID", string.Empty);
            HttpContext.Session.SetString("RoleName", string.Empty);
            HttpContext.Session.SetString("FullName", string.Empty);
            HttpContext.Session.SetString("ImagePath", string.Empty);
            HttpContext.Session.SetString("DoctorId", string.Empty);

            return RedirectToAction("Index", "Redirect");
        }
    }
}
