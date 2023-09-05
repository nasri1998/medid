using Med322.DataAccess;
using Med322.Models;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using XAct;
using XAct.Messages;

namespace Med322.Controllers
{
    public class PatientDetailController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly int pageSize;
        public PatientDetailController(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
            pageSize = int.Parse(_config["PageSize"]);
        }
        public async Task<IActionResult> Index(VMPage Page)
        {
            VMResponse apiResponse = new VMResponse();

            try
            {
                    int Id = int.Parse(HttpContext.Session.GetString("UserID"));
                if (Page.filter == null)
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "PatientDetail/GetAll/" + Id));
                }
                else
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "PatientDetail/GetFilter/" + Page.filter +"/"+ Id));
                }

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("Api Connection Error!");
                }

                List<VMPatientDetail> patientData = JsonConvert.DeserializeObject<List<VMPatientDetail>>(apiResponse.data.ToString());

                ViewBag.Title = "Daftar Pasien";
                ViewBag.User = "Patient";
                ViewBag.UserId = int.Parse(HttpContext.Session.GetString("UserID"));
                ViewBag.BiodataId = HttpContext.Session.GetString("BiodataID");
                HttpContext.Session.SetString("InfoMsg", apiResponse.Message);

                ViewBag.OrderID = string.IsNullOrEmpty(Page.orderBy) ? "id_desc" : "";
                ViewBag.OrderName = (Page.orderBy == "name") ? "name_desc" : "name";

                ViewBag.OrderBy = Page.orderBy;
                ViewBag.Filter = Page.filter;

                switch (Page.orderBy)
                {
                    case "name":
                        patientData = patientData.OrderBy(product => product.Fullname).ToList();
                        break;
                    case "name_desc":
                        patientData = patientData.OrderByDescending(product => product.Fullname).ToList();
                        break;
                    default:
                        patientData = patientData.OrderBy(productdata => productdata.BiodataId).ToList();
                        break;
                }

                return View(Pagination<VMPatientDetail>.CreateAsync(patientData, Page.pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("ErrMsg", ex.Message);
                return View();
            }

        }

        public async Task<IActionResult> Add()
        {
            try
            {
                VMResponse? apiBloodResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/GetAll"));
                VMResponse? apiRelationResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/GetAll"));

                if (apiBloodResponse == null)
                {
                    throw new ArgumentException("No Connection to Variant API");
                }

                ViewBag.BloodGroup = (apiBloodResponse.Success || apiBloodResponse.data != null) ? JsonConvert.DeserializeObject<List<VMTblMBloodGroup?>>(apiBloodResponse.data.ToString()) : null;
                ViewBag.Relation = (apiRelationResponse.Success || apiRelationResponse.data != null) ? JsonConvert.DeserializeObject<List<VMTblMCustomerRelation?>>(apiRelationResponse.data.ToString()) : null;
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }
            ViewBag.Title = "Add New Patient";
            ViewBag.UserId = int.Parse(HttpContext.Session.GetString("UserID"));
            ViewBag.BiodataId = HttpContext.Session.GetString("BiodataID");


            return View();
        }

        [HttpPost]
        public async Task<VMResponse> Add(VMPatientDetail data)
        {
            VMResponse apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (await httpClient.PostAsync(apiUrl + "PatientDetail", content)).Content.ReadAsStringAsync());

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

        public async Task<IActionResult> Edit(int Id)
        {


            VMCustomerMember? dataCM = new VMCustomerMember();
            VMBiodata dataBio = new VMBiodata();
            List<VMCustomer> dataCustomer = new List<VMCustomer>();
            List<VMTblMBloodGroup> dataBG = new List<VMTblMBloodGroup>();
            List<VMTblMCustomerRelation> dataCR = new List<VMTblMCustomerRelation>();


            try
            {
                VMResponse? apiResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "PatientDetail/GetId/" + Id));

                VMResponse? apiBloodResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "BloodGroup/GetAll"));
                VMResponse? apiRelationResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "CustomerRelation/GetAll"));

                VMResponse? apiCustomerMemberResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "PatientDetail/GetCMid/" + Id));

                VMResponse? apiCustomerResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Customer/GetAll"));

                VMResponse? apiBiodataResponse = JsonConvert.DeserializeObject<VMResponse>(
                                   await httpClient.GetStringAsync(apiUrl + "Biodata/" + Id));

                if (apiCustomerMemberResponse == null || apiCustomerResponse == null || apiBiodataResponse == null)
                {
                    throw new ArgumentNullException("API connection Error");
                }

                if (apiCustomerMemberResponse.data != null || apiCustomerMemberResponse.Success || apiCustomerResponse.data != null || apiCustomerResponse.Success || apiBiodataResponse.data != null || apiBiodataResponse.Success)
                {
                    //ViewBag.Patient = JsonConvert.DeserializeObject<List<VMPatientDetail>>(apiResponse.data.ToString());
                }
                else
                {
                    throw new ArgumentNullException("API connection error");
                }

                ViewBag.BloodGroup = (apiBloodResponse.Success || apiBloodResponse.data != null) ? JsonConvert.DeserializeObject<List<VMTblMBloodGroup?>>(apiBloodResponse.data.ToString()) : null;
                ViewBag.Relation = (apiRelationResponse.Success || apiRelationResponse.data != null) ? JsonConvert.DeserializeObject<List<VMTblMCustomerRelation?>>(apiRelationResponse.data.ToString()) : null;
                ViewBag.Title = "Edit Patient";
                ViewBag.UserId = int.Parse(HttpContext.Session.GetString("UserID"));
                VMPatientDetail? data = JsonConvert.DeserializeObject<VMPatientDetail>(apiResponse.data.ToString());

                dataBG = JsonConvert.DeserializeObject<List<VMTblMBloodGroup>>(apiBloodResponse.data.ToString());
                foreach (VMTblMBloodGroup item in dataBG)
                {
                    if (data.BloodGroupId == item.Id)
                    {
                        ViewBag.BloodId = item.Id;
                    }
                }

                dataCR = JsonConvert.DeserializeObject<List<VMTblMCustomerRelation>>(apiRelationResponse.data.ToString());
                foreach (VMTblMCustomerRelation item in dataCR)
                {
                    if (data.CustomerRelationId == item.Id)
                    {
                        ViewBag.RelationId = item.Id;
                    }
                }

                dataCustomer = JsonConvert.DeserializeObject<List<VMCustomer>>(apiCustomerResponse.data.ToString());
                foreach (VMCustomer item in dataCustomer)
                {
                    if (data.CustomerId == item.Id)
                    {
                        ViewBag.CustomerId = item.Id;
                    }
                }

                return View(data);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<VMResponse> Edit(VMPatientDetail data)
        {
            VMResponse apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "PatientDetail", content)
                    ).Content.ReadAsStringAsync());

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("ErrMsg", $"Failed update Biodata ID = {0} due API Connection Error!");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Cannot update Biodata ID = {0} because failed to get data from API!"); }

            }
            catch (Exception ex)
            {

                throw;
            }

            return apiResponse;
        }

        public async Task<IActionResult> Delete(int Id)
        {
            VMResponse? apiResponse;
            apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "PatientDetail/GetId/" + Id));

            VMPatientDetail relData = JsonConvert.DeserializeObject<VMPatientDetail>(apiResponse.data.ToString());
            ViewBag.Title = "Delete Patient Data";

            return View(relData);
        }

        [HttpPost]
        public async Task<VMResponse> Delete(VMPatientDetail data)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                //string jsonData = JsonConvert.SerializeObject(data);

                //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.DeleteAsync(apiUrl + "PatientDetail/Delete/" + data.CustomerId)).Content.ReadAsStringAsync());

                //apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                //    await httpClient.PostAsync(apiUrl + "PatientDetail/Delete/" + data.CustomerId + "/" + data.CustomerMemberId, content)
                //    ).Content.ReadAsStringAsync());
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
        public async Task<VMResponse> MultipleDelete(List<VMPatientDetail> selectedData)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                foreach (var data in selectedData)
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                        await httpClient.DeleteAsync(apiUrl + "PatientDetail/Delete/" + data.CustomerId)).Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
            }

            HttpContext.Session.SetString("infoMsg", apiResponse.Message);
            return apiResponse;
        }
    }
}
