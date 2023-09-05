using Med322.Models;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace Med322.Controllers
{
    public class CancelAppointmentController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly int pageSize;

        public CancelAppointmentController(IConfiguration _config)
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
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/GetAll/" + Id));
                }
                else
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/GetFilter/" + Page.filter + "/" + Id));
                }

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("Api Connection Error!");
                }

                List<VMListJanji> patientData = JsonConvert.DeserializeObject<List<VMListJanji>>(apiResponse.data.ToString());

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
                        patientData = patientData.OrderBy(product => product.namapasien).ToList();
                        break;
                    case "name_desc":
                        patientData = patientData.OrderByDescending(product => product.namapasien).ToList();
                        break;
                    default:
                        patientData = patientData.OrderBy(productdata => productdata.id).ToList();
                        break;
                }

                return View(Pagination<VMListJanji>.CreateAsync(patientData, Page.pageNumber ?? 1, pageSize));
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("ErrMsg", ex.Message);
                return View();
            }

        }

        public async Task<IActionResult> Edit(int id)
        {
            VMResponse? relIdResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/Get/" + id));
            VMListEdit? datarelasi = JsonConvert.DeserializeObject<VMListEdit>(relIdResponse.data.ToString());

            int relid = (int)datarelasi.idrelasi;

            List<VMDoctorTreatment> dataDT = new List<VMDoctorTreatment>();

            //VMDoctorTreatment? apiTreatNameResponse = new VMDoctorTreatment();
            try
            {
                VMResponse? apiResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/GetEdit/" + id+"/"+ relid));

                ViewBag.Title = "Edit Patient";
                ViewBag.UserId = int.Parse(HttpContext.Session.GetString("UserID"));

                VMListEdit? data = JsonConvert.DeserializeObject<VMListEdit>(apiResponse.data.ToString());

                int x = (int)data.dokid;
                //int y = (int)data.userid;

                VMResponse? apiTreatNameResponse = JsonConvert.DeserializeObject<VMResponse?>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/GetAllTreatment/"+ x));
                dataDT = JsonConvert.DeserializeObject<List<VMDoctorTreatment>>(apiTreatNameResponse.data.ToString());


                //dataTreatment= JsonConvert.DeserializeObject<List<VMDoctorTreatment>>(apiTreatResponse.data.ToString());
                ViewBag.Treatment = JsonConvert.DeserializeObject<List<VMTDoctorTreatment?>>(apiTreatNameResponse.data.ToString());


                //foreach (VMDoctorTreatment item in dataDT)
                //{
                //    if (data.DoctorTreatId == item.Id)
                //    {
                //        ViewBag.Treatment = item.Id;
                //    }
                //}

                return View(data);
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                return View();
            }
        }

        [HttpPost]
        public async Task<VMResponse> Edit(VMListEdit data)
        {
            VMResponse apiResponse = new VMResponse();
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "CancelAppointment", content)
                    ).Content.ReadAsStringAsync());

                if (apiResponse == null)
                {
                    throw new ArgumentNullException("ErrMsg", $"Failed update DATA = {0} due API Connection Error!");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Cannot update DATA = {0} because failed to get data from API!"); }
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
            apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "CancelAppointment/Get/" + Id));

            VMListJanji relData = JsonConvert.DeserializeObject<VMListJanji>(apiResponse.data.ToString());
            ViewBag.Title = "Cancel Appointment";

            return View(relData);
        }

        [HttpPost]
        public async Task<VMResponse> Delete(VMListJanji data)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                //string jsonData = JsonConvert.SerializeObject(data);

                //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.DeleteAsync(apiUrl + "CancelAppointment/Delete/" + data.id)).Content.ReadAsStringAsync());

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
        public async Task<VMResponse> MultipleDelete(List<VMListJanji> selectedData)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                foreach (var data in selectedData)
                {
                    apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.DeleteAsync(apiUrl + "CancelAppointment/Delete/" + data.id)).Content.ReadAsStringAsync());
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
