using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientProfileController : Controller
    {

        private DAPatientProfile PatientProfile;

        public PatientProfileController(Med322_BContext _db)
        {
            PatientProfile = new DAPatientProfile(_db);
        }

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return PatientProfile.Get(id);
        }

        [HttpPut("[action]")]
        public VMResponse CreateUpdate(VMPatientProfile bdata) => PatientProfile.CreateUpdate(bdata); // Update record

    }
}
