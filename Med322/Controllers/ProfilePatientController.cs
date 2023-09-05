using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Text;
using Med322.ViewModels;
using System.Net.Http;
using System;

namespace Med322.Controllers
{

    public class ProfilePatientController : Controller
    {
        private readonly string apiUrl;
        private readonly HttpClient httpClient = new HttpClient();
        public ProfilePatientController(IConfiguration _config)
        {
            apiUrl = _config["ApiUrl"];
        }
        public async Task<IActionResult> Index()
        {

            try
            {
                int Id = int.Parse(HttpContext.Session.GetString("BiodataID"));

                VMResponse? apiCustomerIdResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "Customer/GetCustomerId/" + Id));


                if (apiCustomerIdResponse == null)
                {
                    throw new ArgumentNullException($"Failed get Customer data ID and API connection Error");
                }

                if (apiCustomerIdResponse.data != null || apiCustomerIdResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiCustomerIdResponse.Message);
                }
                else
                {
					HttpContext.Session.SetString("ErrMsg", apiCustomerIdResponse.Message);
					VMPatientProfile? dataNull = new VMPatientProfile();

					VMResponse? apiUserNullResponse = JsonConvert.DeserializeObject<VMResponse>(
	                await httpClient.GetStringAsync(apiUrl + "User/" + Id));

					ViewBag.Account = JsonConvert.DeserializeObject<VMUser>(apiUserNullResponse.data.ToString());

					return View(dataNull);
				}
                VMCustomer? customerId = JsonConvert.DeserializeObject<VMCustomer>(apiCustomerIdResponse.data.ToString());

                VMResponse? apiBiodataResponse = JsonConvert.DeserializeObject<VMResponse>(
                                    await httpClient.GetStringAsync(apiUrl + "PatientProfile/" + customerId.Id));

                //VMResponse? apiUserResponse = JsonConvert.DeserializeObject<VMResponse>(
                //    await httpClient.GetStringAsync(apiUrl + "User/" + Id));
                int IdU = int.Parse(HttpContext.Session.GetString("UserID"));

                VMResponse? apiUserResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "User/" + IdU));

                if (apiBiodataResponse == null || apiUserResponse == null)
                {
                    throw new ArgumentNullException($"Failed get Patient Profile data ID = {Id} and API connection Error");
                }

                if (apiBiodataResponse.data != null || apiBiodataResponse.Success || apiUserResponse != null || apiUserResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiBiodataResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get Patient Profile data ID = {Id} due no data in API or API connection error");
                }

                VMPatientProfile? data = JsonConvert.DeserializeObject<VMPatientProfile>(apiBiodataResponse.data.ToString());
                ViewBag.Account = JsonConvert.DeserializeObject<VMUser>(apiUserResponse.data.ToString());
                ViewBag.BiodataId = HttpContext.Session.GetString("BiodataID");

                return View(data);

            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", e.Message);
                return View();
            }
        }

        public async Task<IActionResult> PatientEditBio(int Id)
        {
            try
            {
                VMResponse? apiBiodataResponse = JsonConvert.DeserializeObject<VMResponse>(
                                    await httpClient.GetStringAsync(apiUrl + "PatientProfile/" + Id));

                if (apiBiodataResponse == null)
                {
                    throw new ArgumentNullException($"Failed get Biodata data ID = {Id}, and API connection Error");
                }

                if (apiBiodataResponse.data != null || apiBiodataResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiBiodataResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get variant data ID = {Id} due no data in API or API connection error");
                }
                VMPatientProfile? data = JsonConvert.DeserializeObject<VMPatientProfile>(apiBiodataResponse.data.ToString());
 

                return View(data);

            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", e.Message);
                return View();
            }

        }

        [HttpPost]
        public async Task<VMResponse> PatientEditBio(VMPatientProfile biodata)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                string jsonData = JsonConvert.SerializeObject(biodata);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "PatientProfile/CreateUpdate", content)
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

                // return View(apiResponse);

            }

            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", $"Failed Update Biodata ID = {0} due API Connection Error!");
                apiResponse.Success = false;
                apiResponse.Message = e.Message;
                apiResponse.data = null;
                return apiResponse;
			}

			HttpContext.Session.SetString("FullName", biodata.Fullname);
			return apiResponse;

        }

        public async Task<IActionResult> PatientEditEmail(int Id)
        {
            try
            {
                VMResponse? apiAccountResponse = JsonConvert.DeserializeObject<VMResponse>(
                                    await httpClient.GetStringAsync(apiUrl + "User/" + Id));

                if (apiAccountResponse == null)
                {
                    throw new ArgumentNullException($"Failed get User data ID = {Id}, and API connection Error");
                }

                if (apiAccountResponse.data != null || apiAccountResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiAccountResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get User data ID = {Id} due no data in API or API connection error");
                }
                VMUser? data = JsonConvert.DeserializeObject<VMUser>(apiAccountResponse.data.ToString());


                return View(data);

            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", e.Message);
                return View();
            }

        }

        [HttpPost]
        public async Task<VMResponse> PatientEditEmail(VMUser emailPatient)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                string jsonData = JsonConvert.SerializeObject(emailPatient);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "User/Update", content)
                    ).Content.ReadAsStringAsync());


                if (apiResponse == null)
                {
                    throw new ArgumentNullException("ErrMsg", $"Failed update User ID = {0} due API Connection Error!");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Cannot update User ID = {0} because failed to get data from API!"); }

                // return View(apiResponse);

            }

            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", $"Failed Update User ID = {0} due API Connection Error!");
                apiResponse.Success = false;
                apiResponse.Message = e.Message;
                apiResponse.data = null;
                return apiResponse;
            }
            return apiResponse;

        }


        public async Task<IActionResult> PatientEditPassword(int Id)
        {

            try
            {
                VMResponse? apiAccountResponse = JsonConvert.DeserializeObject<VMResponse>(
                                    await httpClient.GetStringAsync(apiUrl + "User/" + Id));

                if (apiAccountResponse == null)
                {
                    throw new ArgumentNullException($"Failed get User data ID = {Id}, and API connection Error");
                }

                if (apiAccountResponse.data != null || apiAccountResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiAccountResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get User data ID = {Id} due no data in API or API connection error");
                }
                VMUser? data = JsonConvert.DeserializeObject<VMUser>(apiAccountResponse.data.ToString());


                return View(data);

            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", e.Message);
                return View();
            }

        }

        //public async Task<VMResponse> PatientVerivPassword(VMUser passwordPatient)
        //{
        //    VMResponse? apiResponse = new VMResponse();

        //    try
        //    {
        //        VMResponse? apiAccountResponse = JsonConvert.DeserializeObject<VMResponse>(
        //            await httpClient.GetStringAsync(apiUrl + "User/" + passwordPatient.Id));

        //        if (apiAccountResponse == null)
        //        {
        //            throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id}, and API connection Error");
        //        }

        //        if (apiAccountResponse.data != null || apiAccountResponse.Success)
        //        {
        //            //HttpContext.Session.SetString("InfoMsg", apiAccountResponse.Message);
        //        }
        //        else
        //        {
        //            throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id} due no data in API or API connection error");
        //        }
        //        VMUser? truePassword = JsonConvert.DeserializeObject<VMUser>(apiAccountResponse.data.ToString());

        //        if (passwordPatient.Password != truePassword.Password)
        //        {
        //            apiResponse.Success = false;
        //            HttpContext.Session.SetString("ErrMsg", "Password salah");
        //            apiResponse.Message = "Password salah";
        //            apiResponse.data = null;
        //            return apiResponse;
        //        }

        //        return apiResponse;

        //    }
        //    catch (Exception e){
        //        HttpContext.Session.SetString("ErrMsg", $"Failed Update User ID = {0} due API Connection Error!");
        //        apiResponse.Success = false;
        //        apiResponse.Message = e.Message;
        //        apiResponse.data = null;
        //        return apiResponse;
        //    }
        //}


        [HttpPost]
        public async Task<VMResponse> PatientEditPassword(VMUser passwordPatient)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                VMResponse? apiAccountResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "User/" + passwordPatient.Id));

                if (apiAccountResponse == null)
                {
                    throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id}, and API connection Error");
                }

                if (apiAccountResponse.data != null || apiAccountResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiAccountResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id} due no data in API or API connection error");
                }
                VMUser? truePassword = JsonConvert.DeserializeObject<VMUser>(apiAccountResponse.data.ToString());

                if(passwordPatient.Password != truePassword.Password) 
                {
                    apiResponse.Success = false;
                    HttpContext.Session.SetString("ErrMsg", "Password salah");
                    apiResponse.Message = "Password salah";
                    apiResponse.data = null;
                    return apiResponse;
                }


                string jsonData = JsonConvert.SerializeObject(passwordPatient);

                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/Json");

                apiResponse = JsonConvert.DeserializeObject<VMResponse>(await (
                    await httpClient.PutAsync(apiUrl + "User/Update", content)
                    ).Content.ReadAsStringAsync());


                if (apiResponse == null)
                {
                    throw new ArgumentNullException("ErrMsg", $"Failed update User ID = {0} due API Connection Error!");
                }

                if (apiResponse.Success || apiResponse.data != null)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiResponse.Message);
                }
                else
                { throw new ArgumentNullException($"Cannot update User ID = {0} because failed to get data from API!"); }

                // return View(apiResponse);

            }

            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", $"Failed Update User ID = {0} due API Connection Error!");
                apiResponse.Success = false;
                apiResponse.Message = e.Message;
                apiResponse.data = null;
                return apiResponse;
            }
            return apiResponse;

        }

        public async Task<VMResponse> PatientEditNewPassword(VMUser passwordPatient)
        {
            VMResponse? apiResponse = new VMResponse();

            try
            {
                VMResponse? apiAccountResponse = JsonConvert.DeserializeObject<VMResponse>(
                    await httpClient.GetStringAsync(apiUrl + "User/" + passwordPatient.Id));

                if (apiAccountResponse == null)
                {
                    throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id}, and API connection Error");
                }

                if (apiAccountResponse.data != null || apiAccountResponse.Success)
                {
                    //HttpContext.Session.SetString("InfoMsg", apiAccountResponse.Message);
                }
                else
                {
                    throw new ArgumentNullException($"Failed get User data ID = {passwordPatient.Id} due no data in API or API connection error");
                }
                VMUser? truePassword = JsonConvert.DeserializeObject<VMUser>(apiAccountResponse.data.ToString());

                if (passwordPatient.Password != truePassword.Password)
                {
                    apiResponse.Success = false;
                    HttpContext.Session.SetString("ErrMsg", "Password salah");
                    apiResponse.Message = "Password salah";
                    apiResponse.data = null;
                    return apiResponse;
                }

                return apiResponse;

            }
            catch (Exception e)
            {
                HttpContext.Session.SetString("ErrMsg", $"Failed Update User ID = {0} due API Connection Error!");
                apiResponse.Success = false;
                apiResponse.Message = e.Message;
                apiResponse.data = null;
                return apiResponse;
            }
        }

    }


}
