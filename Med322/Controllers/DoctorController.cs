using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using Med322.ViewModels;
using System.Text;
using System.Collections.Generic;
using XAct.Messages;

namespace Med322.Controllers
{
    public class DoctorController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly string imgFolder;

        public DoctorController(IConfiguration _config, IWebHostEnvironment _webHostEnv)
        {
            apiUrl = _config["ApiUrl"];
            webHostEnvironment = _webHostEnv;
            imgFolder = webHostEnvironment.WebRootPath + "\\" + _config["ImageFolder"];
        }

        public string UploadFile(IFormFile file)
        {
            string uniqueFileName = string.Empty;

            if (file != null)
            {
                // generate unique id
                uniqueFileName = Guid.NewGuid().ToString() + "-" + file.FileName;
                string uploadFolder = imgFolder + "\\" + uniqueFileName;

                using (FileStream fileStream = new FileStream(uploadFolder, FileMode.CreateNew))
                {
                    file.CopyTo(fileStream);
                }
            }
            else
            {
                uniqueFileName = null;
            }
            return uniqueFileName;
        }

        private void DeletedOldImage(string OldImageFileName)
        {
            try
            {
                string uploadFolder = imgFolder + "\\" + OldImageFileName;
                if (System.IO.File.Exists(uploadFolder))
                {
                    System.IO.File.Delete(uploadFolder);
                }
                //else
                //{
                //    throw new Exception("File doesn't exist");
                //}
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public IActionResult Index()
        {
            ViewBag.User = "Doctor";
            ViewBag.Sidebar = "main-panel";
            return View();
        }

        public async Task<IActionResult> ListAppointment(long Id)
        {
            VMResponse? apiResponse = new VMResponse();
            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/DoctorAppointment/" + Id));

                if (apiResponse == null)
                {
                    throw new ArgumentException("failed connect to API");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    ViewBag.InfoMsg = apiResponse.Message;
                }
                else
                {
                    ViewBag.ErrMsg = apiResponse.Message;
                }

                List<VMDoctorAppointment>? data = JsonConvert.DeserializeObject<List<VMDoctorAppointment>>(apiResponse.data.ToString());

                apiResponse.data = data;
                ViewBag.User = "Doctor";
                ViewBag.Sidebar = "main-panel";
                return View(data);
            }
            catch (Exception ex)
            {
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
                return RedirectToAction("Index", "Doctor");
            }
        }

        public async Task<IActionResult> profile(long Id)
        {
            VMResponse? apiResponse = new VMResponse();
            VMResponse? apiAppointmentResponse = new VMResponse();

            try
            {
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/" + Id));
                apiAppointmentResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/DoctorAppointment/" + Id));

                if (apiResponse == null)
                {
                    throw new ArgumentException("failed conect to API");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    ViewBag.InfoMsg = apiResponse.Message;

                    VMTCurrentDoctorSpecialization CurtSpez = JsonConvert.DeserializeObject<VMTCurrentDoctorSpecialization>(await httpClient.GetStringAsync(apiUrl + "Doctor/GetTSpecialization/" + Id));
                    if (CurtSpez != null)
                    {
                        int specId = (int)CurtSpez.SpecializationId;
                        VMResponse? apiSpecializationResponse = JsonConvert.DeserializeObject<VMResponse>(
                            await httpClient.GetStringAsync(apiUrl + "Specialization"));

                        List<VMMSpecialization> Specialization = JsonConvert.DeserializeObject<List<VMMSpecialization>>(apiSpecializationResponse.data.ToString());

                        ViewBag.CurtSpez = Specialization[specId - 1].Name;
                    }
                    else
                    {
                        ViewBag.CurtSpez = null;
                    }

                }
                else
                {
                    ViewBag.ErrMsg = apiResponse.Message;
                }

                List<VMMDoctorProfile>? data = JsonConvert.DeserializeObject<List<VMMDoctorProfile>>(apiResponse.data.ToString());
                List<VMDoctorAppointment>? appo = JsonConvert.DeserializeObject<List<VMDoctorAppointment>>(apiResponse.data.ToString());

                int count = appo.DistinctBy(p => p.DoctorId).Count();
                ViewBag.Appo = count;
                ViewBag.User = "Doctor";
                ViewBag.Profile = true;
                ViewBag.Sidebar = "main-panel";
                apiResponse.data = data;
                return View(data);
            }
            catch (Exception ex)
            {
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
                HttpContext.Session.SetString("ErrMsg", apiResponse.Message);
                return RedirectToAction("Index", "Doctor");
            }

        }

        public async Task<IActionResult> AddSpecialization(long id)
        {
            VMTCurrentDoctorSpecialization? data = new VMTCurrentDoctorSpecialization();
            VMMDoctor? doctor = new VMMDoctor();

            try
            {
                doctor = JsonConvert.DeserializeObject<VMMDoctor>(
                        await httpClient.GetStringAsync(apiUrl + "Doctor/GetId/" + id));

                VMResponse? apiSpecializationResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Specialization"));

                if (doctor == null)
                {
                    throw new ArithmeticException("doctor not exist");
                }
                else
                {
                    ViewBag.Doctor = doctor.Id;

                    if (apiSpecializationResponse != null)
                    {
                        //data = JsonConvert.DeserializeObject<VMMDoctor>(apiDoctorResponse.ToString());

                        ViewBag.Specialization = JsonConvert.DeserializeObject<List<VMMSpecialization>>(apiSpecializationResponse.data.ToString());
                    }
                    else
                    {
                        throw new ArgumentException("No connection");
                    }
                }


                ViewBag.User = "Doctor";
                ViewBag.Profile = true;
                ViewBag.Sidebar = "main-panel";
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse> AddSpecialization(VMTCurrentDoctorSpecialization specialization)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                string jsonData = JsonConvert.SerializeObject(specialization);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await
                    (await httpClient.PostAsync(apiUrl + "Doctor/Createspecialization", content))
                    .Content.ReadAsStringAsync());

                if (apiResponse == null)
                {
                    throw new ArgumentException("No connection");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", "Success");
                    apiResponse.Message = "Sukses menambah data";
                }
                else
                {
                    throw new ArgumentException("Cannot Edit data");
                }
            }
            catch (Exception ex)
            {
                apiResponse.Success = false;
                apiResponse.Message = ex.Message;
                HttpContext.Session.SetString("ErrMsg", apiResponse.Message);
            }

            return apiResponse;
        }

        public async Task<IActionResult> EditImage(long id)
        {
            VMBiodata? data = new VMBiodata();

            try
            {
                data = JsonConvert.DeserializeObject<VMBiodata>(
                       await httpClient.GetStringAsync(apiUrl + "Doctor/GetBiodataById/" + id));

                if (data == null)
                {
                    throw new ArgumentException("No connection");
                }
                else
                {
                    ViewBag.User = "Doctor";
                    ViewBag.Profile = true;
                    ViewBag.Sidebar = "main-panel";
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse> EditImage(VMBiodata biodata)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                if (biodata.ImageFile != null)
                {
                    DeletedOldImage(biodata.ImagePath);
                    biodata.ImagePath = UploadFile(biodata.ImageFile);
                    biodata.ImageFile = null;
                }

                string jsonData = JsonConvert.SerializeObject(biodata);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await
                    (await httpClient.PutAsync(apiUrl + "Doctor/UpdateImage", content))
                    .Content.ReadAsStringAsync());

                if (apiResponse == null)
                {
                    throw new ArgumentException("No connection");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", "Success");
                }
                else
                {
                    throw new ArgumentException("Cannot Edit data");
                }
            }
            catch (Exception ex)
            {
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
                HttpContext.Session.SetString("ErrMsg", apiResponse.Message);
            }

            return apiResponse;
        }

        public async Task<IActionResult> EditSpecialization(long id)
        {
            VMTCurrentDoctorSpecialization? data = new VMTCurrentDoctorSpecialization();

            try
            {
                data = JsonConvert.DeserializeObject<VMTCurrentDoctorSpecialization>(
                        await httpClient.GetStringAsync(apiUrl + "Doctor/GetTSpecialization/" + id));

                VMResponse? apiSpecializationResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Specialization"));

                if (data == null)
                {
                    throw new ArgumentException("No connection");
                }

                else if (data != null)
                {
                    //data = JsonConvert.DeserializeObject<VMMDoctor>(apiDoctorResponse.ToString());

                    if (apiSpecializationResponse.Success || apiSpecializationResponse.data != null)
                    {
                        ViewBag.Specialization = JsonConvert.DeserializeObject<List<VMMSpecialization>>(apiSpecializationResponse.data.ToString());
                    }
                    else
                    {
                        throw new ArgumentException("No data");
                    }
                }
                else
                {
                    throw new ArgumentException("Cannot Add data");
                }
                ViewBag.User = "Doctor";
                ViewBag.Profile = true;
                ViewBag.Sidebar = "main-panel";
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<VMResponse> EditSpecialization(VMTCurrentDoctorSpecialization specialization)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                string jsonData = JsonConvert.SerializeObject(specialization);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await
                    (await httpClient.PutAsync(apiUrl + "Doctor", content))
                    .Content.ReadAsStringAsync());

                if (apiResponse == null)
                {
                    throw new ArgumentException("No connection");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    HttpContext.Session.SetString("infoMsg", "Sukses mengubah data");
                    apiResponse.Message = "Sukses mengubah data";
                }
                else
                {
                    throw new ArgumentException("Cannot Edit data");
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("errMsg", ex.Message);
                apiResponse.Message = ex.Message;
                apiResponse.Success = false;
            }

            return apiResponse;
            //return RedirectToAction("profile","Doctor",doctor.Id);
        }

        public IActionResult ProfileDoctor()
        {
            ViewBag.User = "Doctor";
            ViewBag.Profile = true;
            ViewBag.Sidebar = "main-panel";
            return View("~/Views/Profile/Index.cshtml");
        }

        public async Task<IActionResult> FindDoctor(string filterDoctor, string filterSpecialization, long filterLocationId, long filterTreatmentId)
        {
            VMViewFindDoctor data = new VMViewFindDoctor();

            try
            {
                VMResponse? apiDoctorResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Doctor/"));

                VMResponse? apiTreatmentResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Doctor/GetTreatmentAll"));

                VMResponse? apiLocationResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Location/GetAll"));

                if (apiTreatmentResponse == null || apiLocationResponse == null)
                {
                    throw new ArgumentException("No connection");
                }

                else
                {
                    //data = JsonConvert.DeserializeObject<VMMDoctor>(apiDoctorResponse.ToString());
                    if (apiDoctorResponse.Success || apiDoctorResponse.data != null)
                    {
                        List<VMMDoctorProfile> specialization = JsonConvert.DeserializeObject<List<VMMDoctorProfile>>(apiDoctorResponse.data.ToString());
                        ViewBag.Specialization = specialization.DistinctBy(p => p.specializationName);
                    }
                    else
                    {
                        throw new ArgumentException("No location data");
                    }

                    if (apiTreatmentResponse.Success || apiTreatmentResponse.data != null)
                    {
                        ViewBag.Treatment = JsonConvert.DeserializeObject<List<VMTDoctorTreatment>>(apiTreatmentResponse.data.ToString());
                    }
                    else
                    {
                        throw new ArgumentException("No treatment data");
                    }

                    if (apiLocationResponse.Success || apiLocationResponse.data != null)
                    {
                        ViewBag.Location = JsonConvert.DeserializeObject<List<VMLocation>>(apiLocationResponse.data.ToString());
                    }
                    else
                    {
                        throw new ArgumentException("No location data");
                    }
                }

                ViewBag.Profile = true;
                ViewBag.Sidebar = "main-panel";
                ViewBag.Name = filterDoctor;
                ViewBag.Spez = filterSpecialization;
                ViewBag.LocId = filterLocationId;
                ViewBag.TreatId = filterTreatmentId;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> FindDoctor(long filterTreatmentId = 0, long filterLocationId = 0, string? filterDoctor = null, string? filterSpecialization = null)
        {
            VMResponse? apiResponse = new VMResponse();
            List<VMViewFindDoctor>? data = new List<VMViewFindDoctor>();
            //string jsonData = JsonConvert.SerializeObject(doctor);
            //HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            apiResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl +
                "Doctor/GetByFilter?" + $"filterTreatmentId={filterTreatmentId}&filterLocationId={filterLocationId}&filterDoctor={filterDoctor}&filterSpecialization={filterSpecialization}"));

            data = JsonConvert.DeserializeObject<List<VMViewFindDoctor>>(apiResponse.data.ToString());

            if (data.Count == 0)
            {
                ViewBag.Message = "Not find anything :'(";
            }

            ViewBag.doctorId = data.DistinctBy(p => p.DoctorId);
            ViewBag.Name = filterDoctor;
            ViewBag.Spez = filterSpecialization;
            ViewBag.LocId = filterLocationId;
            ViewBag.TreatId = filterTreatmentId;
            foreach (var x in data)
            {
                if (x.LocationId == filterLocationId && x.LocationId > 0)
                {
                    ViewBag.Loc = x.LocationName;                    
                }

                if (x.DoctorTreatmentId == filterTreatmentId && x.DoctorTreatmentId > 0)
                {
                    ViewBag.Treat = x.DoctorTreatmentName;
                }
            }

            return View("ListDoctor", data);
        }

        public IActionResult confirmation(string message)
        {
            ViewBag.Title = "Success";
            ViewBag.Body = $"berhasil {message} data";

            return View();
        }

        public async Task<IActionResult> detail(long Id)
        {
            VMResponse? apiProfileResponse = new VMResponse();
            VMResponse? apiDetailResponse = new VMResponse();            

            try
            {
                apiProfileResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/" + Id));
                apiDetailResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/DetailDoctor/" + Id));

                if (apiProfileResponse == null)
                {
                    throw new ArgumentException("failed conect to API");
                }
                List<VMMDoctorProfile>? data = JsonConvert.DeserializeObject<List<VMMDoctorProfile>>(apiProfileResponse.data.ToString());

                string name = data.FirstOrDefault().Fullname;

                //ambil data experience dari api filter
                VMResponse? apiDoctorResponse = JsonConvert.DeserializeObject<VMResponse>(await httpClient.GetStringAsync(apiUrl + "Doctor/GetByFilter?" + $"filterDoctor={name}"));
                List<VMViewFindDoctor>? doctor = JsonConvert.DeserializeObject<List<VMViewFindDoctor>>(apiDoctorResponse.data.ToString());
                ViewBag.exper = doctor.DistinctBy(y => y.Experience).OrderByDescending(y => y.Experience).FirstOrDefault().Experience;

                //ambil data jadwal praktek
                if (apiDetailResponse == null || !apiDetailResponse.Success)
                {
                    throw new ArgumentException("failed conect to API");
                }
                else
                {
                    List<VMDoctorDetail>? detail = JsonConvert.DeserializeObject<List<VMDoctorDetail>>(apiDetailResponse.data.ToString());
                    ViewBag.DetailRS = detail.DistinctBy(rs => rs.MedicalFacilityName);
                    ViewBag.Detail = detail;
                }
                
                ViewBag.User = "Doctor";
                ViewBag.Profile = true;
                ViewBag.Sidebar = "main-panel";
                apiProfileResponse.data = data;
                return View(data);
            }
            catch (Exception ex)
            {
                apiProfileResponse.Message = ex.Message;
                apiProfileResponse.Success = false;
                HttpContext.Session.SetString("ErrMsg", apiProfileResponse.Message);
                return RedirectToAction("Index", "Doctor");
            }
        }
    }
}
