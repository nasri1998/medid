using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientDetailController : Controller
    {
        private DAPatientDetail patientDetail;

        public PatientDetailController(Med322_BContext _db)
        {
            patientDetail = new DAPatientDetail(_db);
        }
        [HttpGet("[action]/{idCM?}")] // http methode
        public VMResponse GetIdCM(int idCM) => patientDetail.GetId(idCM); // ARROW FUNCTION HANYA BUTUH 1 BARIS
        [HttpGet("[action]/{id?}")] // http methode
        public VMResponse GetAll(int id) => patientDetail.GetAll(id); // ARROW FUNCTION HANYA BUTUH 1 BARIS
        [HttpGet("[action]/{customerid?}")]
        public VMResponse GetId(int customerid) => patientDetail.Get(customerid);

        [HttpGet("[action]/{GetCMid?}")] // http methode
        public VMResponse GetCMid(int GetCMid) => patientDetail.GetCustomerMember(GetCMid); // ARROW FUNCTION HANYA BUTUH 1 BARIS
        
        [HttpGet("[action]/{filter?}/{Id?}")]
        public VMResponse GetFilter(string filter, int Id)
        {
            return patientDetail.GetByFilter(filter, Id);
        }

        [HttpPost]
        public VMResponse Post(VMPatientDetail data) => patientDetail.Create(data);  // request to make a new record

        [HttpPut]
        public VMResponse Put(VMPatientDetail data) => patientDetail.Update(data);

        [HttpDelete("[action]/{customermember?}")]
        public VMResponse Delete( int customermember) {
         return patientDetail.Delete( customermember);
        }
    }
}
