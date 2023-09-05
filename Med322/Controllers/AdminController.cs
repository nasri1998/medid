using Med322.Models;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Med322.ViewModels;

namespace Med322WebApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        //private readonly int pageSize;

        public AdminController(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
            //pageSize = int.Parse(_config["PageSize"]);
        }
        public IActionResult Index()
        {
            ViewBag.User = "Admin";
            ViewBag.Sidebar = "main-panel";
            return View();
        }

        public async Task<IActionResult> ViewDataBlood(VMPage Page)
        {
            VMResponse apiResponse = new VMResponse();

            try
            {
                if (Page.filter == null)
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/GetAll"));
                }
                else
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/Get/" + Page.filter));
                }
                    //mengubah string json menjadi object

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("Api Connection Error!");
                }

                List<VMTblMBloodGroup> bloodData = JsonConvert.DeserializeObject<List<VMTblMBloodGroup>>(apiResponse.data.ToString());
                ViewBag.TitleModal = "Blood Group";
                ViewBag.Title = "Blood Group";
                HttpContext.Session.SetString("InfoMsg", apiResponse.Message);

                //order by
                ViewBag.OrderID = string.IsNullOrEmpty(Page.orderBy) ? "id_desc" : "";
                ViewBag.OrderCode = (Page.orderBy == "code") ? "code_desc" : "code";

                ViewBag.OrderBy = Page.orderBy;
                ViewBag.Filter = Page.filter;

                ViewBag.User = "Admin";

                switch (Page.orderBy)
                {
                    case "code":
                        bloodData = bloodData.OrderBy(product => product.Code).ToList();
                        break;
                    case "code_desc":
                        bloodData = bloodData.OrderByDescending(product => product.Code).ToList();
                        break;
                    case "id_desc":
                        bloodData = bloodData.OrderByDescending(product => product.Id).ToList();
                        break;
                    default:
                        bloodData = bloodData.OrderBy(productdata => productdata.Id).ToList();
                        break;
                }


                //HttpContext.Session.SetString("infoMsg", "Coba kirim info via Session");
                //HttpContext.Session.SetString("warnMsg", "Coba kirim warning via Session");
                //HttpContext.Session.SetString("errMsg", "Coba kirim error via Session");

                return View(PaginationModel<VMTblMBloodGroup>.CreateAsync(bloodData));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("ErrMsg", ex.Message);
                return View();
            }
            
        }

        public async Task<IActionResult> ViewDataRelation(VMPage Page)
        {

            VMResponse apiResponse = new VMResponse();

            try
            {
                if (Page.filter == null)
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/GetAll"));
                }
                else
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/Get/" + Page.filter));
                } 

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("Api Connection Error!");
                }
              
                List<VMTblMCustomerRelation> relData = JsonConvert.DeserializeObject<List<VMTblMCustomerRelation>>(apiResponse.data.ToString());

                ViewBag.TitleModal = "Customer Relation";
                ViewBag.Title = "Customer Relation";
                HttpContext.Session.SetString("InfoMsg", apiResponse.Message);

                //order by
                ViewBag.OrderID = string.IsNullOrEmpty(Page.orderBy) ? "id_desc" : "";
                ViewBag.OrderName = (Page.orderBy == "name") ? "name_desc" : "name";

                ViewBag.OrderBy = Page.orderBy;
                ViewBag.Filter = Page.filter;

                ViewBag.User = "Admin";

                switch (Page.orderBy)
                {
                    case "name":
                        relData = relData.OrderBy(product => product.Name).ToList();
                        break;
                    case "name_desc":
                        relData = relData.OrderByDescending(product => product.Name).ToList();
                        break;
                    case "id_desc":
                        relData = relData.OrderByDescending(product => product.Id).ToList();
                        break;
                    default:
                        relData = relData.OrderBy(productdata => productdata.Id).ToList();
                        break;
                }



                return View(PaginationModel<VMTblMCustomerRelation>.CreateAsync(relData));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("ErrMsg", ex.Message);
                return View();
            }

        }

        public async Task<IActionResult> DetailBlood(int id)
        {
            VMTblMBloodGroup? bloodData = new VMTblMBloodGroup();
            try
            {
                VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "BloodGroup/" + id)
                );

                if (apiResponse == null)
                {
                    throw new ArgumentNullException($"No DATA with ID {id} found!");
                }

                if (!apiResponse.Success || apiResponse.data == null)
                {
                    HttpContext.Session.SetString("errMsg", apiResponse.Message);

                    throw new ArgumentNullException($"No DATA with ID {id} found!");
                    //ViewBag.ErrMsg = apiResponse.Message;
                }
                else
                {
                    HttpContext.Session.SetString("infoMsg", $"data with id = {id} successfully fetched!");
                    bloodData = JsonConvert.DeserializeObject<VMTblMBloodGroup?>(apiResponse.data.ToString());
                    ViewBag.Title = $"Detail ID {id}";

                }
                return View(bloodData);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                return RedirectToAction("ViewDataBlood", "Admin");
            }
        }
        public async Task<IActionResult> DetailRelation(int id)
        {
            VMTblMCustomerRelation? relData = new VMTblMCustomerRelation();
            try
            {
                VMResponse apiResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "CustomerRelation/" + id)
                );

                if (apiResponse == null)
                {
                    throw new ArgumentNullException($"No DATA with ID {id} found!");
                }

                if (!apiResponse.Success || apiResponse.data == null)
                {
                    HttpContext.Session.SetString("errMsg", apiResponse.Message);

                    throw new ArgumentNullException($"No DATA with ID {id} found!");
                    //ViewBag.ErrMsg = apiResponse.Message;
                }
                else
                {
                    HttpContext.Session.SetString("infoMsg", $"data with id = {id} successfully fetched!");
                    relData = JsonConvert.DeserializeObject<VMTblMCustomerRelation?>(apiResponse.data.ToString());
                    ViewBag.Title = $"Detail ID {id}";

                }
                return View(relData);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                return RedirectToAction("ViewDataRelation", "Admin");
            }
        }

        public async Task<IActionResult> AddBlood()
        {
            VMTblMBloodGroup data = new VMTblMBloodGroup();

            ViewBag.Title = "Add New Blood Group";

            return View(data);
        }

        public async Task<IActionResult> AddRelation()
        {
            VMTblMCustomerRelation data = new VMTblMCustomerRelation();

            ViewBag.Title = "Add New Family";

            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse> AddBlood(VMTblMBloodGroup data)
        {
            VMResponse apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (await httpClient.PostAsync(apiUrl + "BloodGroup", content)).Content.ReadAsStringAsync());

                //check api connection
                if (apiResponse == null)
                {
                    throw new ArgumentNullException("API cannot be reached!");
                }

                //check API RETURN DATA
                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);

                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            return apiResponse;
        }

        [HttpPost]
        public async Task<VMResponse> AddRelation(VMTblMCustomerRelation data)
        {
            VMResponse apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (await httpClient.PostAsync(apiUrl + "CustomerRelation", content)).Content.ReadAsStringAsync());

                //check api connection
                if (apiResponse == null)
                {
                    throw new ArgumentNullException("API cannot be reached!");
                }

                //check API RETURN DATA
                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);

                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);

                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            return apiResponse;
        }

        public async Task<IActionResult> EditBlood(int id)
        {
            VMTblMBloodGroup? data = new VMTblMBloodGroup();
            try
            {
                VMResponse? apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/" + id));
                if (apiResponse == null)
                {
                    throw new ArgumentException("No Connection to API");
                }

               

                if (!apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);
                    data = JsonConvert.DeserializeObject<VMTblMBloodGroup>(apiResponse.data.ToString());
                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);
                }


            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewBag.Title = "Edit Blood Group";
            return View(data);
        }

        public async Task<IActionResult> EditRelation(int id)
        {
            VMTblMCustomerRelation? data = new VMTblMCustomerRelation();
            try
            {
                VMResponse? apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/" + id));
                if (apiResponse == null)
                {
                    throw new ArgumentException("No Connection to API");
                }

                

                if (!apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);
                    data = JsonConvert.DeserializeObject<VMTblMCustomerRelation>(apiResponse.data.ToString());
                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);
                }


            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewBag.Title = "Edit Family";
            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse> EditBlood(VMTblMBloodGroup data)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "BloodGroup", content)).Content.ReadAsStringAsync());

                //check api connection
                if (apiResponse == null)
                {
                    throw new ArgumentNullException("API cannot be reached!");
                }

                //check API RETURN DATA
                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);

                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);

                }

            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            return apiResponse;
        }

        [HttpPost]
        public async Task<VMResponse> EditRelation(VMTblMCustomerRelation data)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "CustomerRelation", content)).Content.ReadAsStringAsync());

                //check api connection
                if (apiResponse == null)
                {
                    throw new ArgumentNullException("API cannot be reached!");
                }

                //check API RETURN DATA
                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", apiResponse.Message);

                }
                else
                {
                    throw new ArgumentNullException(apiResponse.Message);

                }

            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            return apiResponse;
        }

        public async Task<IActionResult> DeleteBlood(int id)
        {
            VMResponse? apiResponse;
            apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/" + id));

            VMTblMBloodGroup bloodData = JsonConvert.DeserializeObject<VMTblMBloodGroup>(apiResponse.data.ToString());
            ViewBag.Title = "Delete Blood Group";

            return View(bloodData);
        }

        public async Task<IActionResult> DeleteRelation(int id)
        {
            VMResponse? apiResponse;
            apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/" + id));

            VMTblMCustomerRelation relData = JsonConvert.DeserializeObject<VMTblMCustomerRelation>(apiResponse.data.ToString());
            ViewBag.Title = "Delete Family Data";

            return View(relData);
        }

        [HttpPost]
        public async Task<VMResponse> DeleteBlood(int id, int deletedby)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.DeleteAsync(apiUrl + "BloodGroup/Delete/" + id + "/" + deletedby)).Content.ReadAsStringAsync());

                //HttpContext.Session.SetString("infoMsg", apiResponse.Message);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            HttpContext.Session.SetString("infoMsg", apiResponse.Message);

            return apiResponse;
        }

        [HttpPost]
        public async Task<VMResponse> DeleteRelation(int id, int deletedby)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.DeleteAsync(apiUrl + "CustomerRelation/Delete/" + id + "/" + deletedby)).Content.ReadAsStringAsync());

                //HttpContext.Session.SetString("infoMsg", apiResponse.Message);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            HttpContext.Session.SetString("infoMsg", apiResponse.Message);

            return apiResponse;
        }

        public IActionResult ProfileAdmin()
        {
            ViewBag.User = "Admin";
            ViewBag.Profile = true;
            ViewBag.Sidebar = "main-panel";
            return View("~/Views/Profile/Index.cshtml");
        }
    }
}
