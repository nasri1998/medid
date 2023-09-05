using Microsoft.AspNetCore.Mvc;
using Med322.DataAccess;
using Med322.ViewModels;
using Med322.DataModels;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : Controller
    {
        private DADoctors doctors;
        public DoctorController(Med322_BContext _db)
        {
            doctors = new DADoctors(_db);
        }

        [HttpGet]
        public VMResponse GetAll() => doctors.GetAll();

        [HttpGet("[action]/{Id}")]
        public VMMDoctor GetId(long Id) => doctors.GetId(Id);

        [HttpGet("[action]/{id}")]
        public VMBiodata GetBiodataById(long id) => doctors.GetBiodataById(id);

        [HttpGet("{id?}")]
        public VMResponse GetById(long id) => doctors.GetById(id);

        [HttpGet("[action]/{DoctorId}")]
        public VMTCurrentDoctorSpecialization GetTSpecialization(long DoctorId) => doctors.GetTSpecialization(DoctorId);

        [HttpPost("[action]")]
        public VMResponse Createspecialization(VMTCurrentDoctorSpecialization inputSpecialization) => doctors.CreateUpdatespecialization(inputSpecialization);

        [HttpPut("[action]")]
        public VMResponse UpdateImage(VMBiodata inputData) => doctors.AddEditImage(inputData);

        [HttpPut]
        public VMResponse Updatespecialization(VMTCurrentDoctorSpecialization inputSpecialization) => doctors.CreateUpdatespecialization(inputSpecialization);

        [HttpGet("[action]")]
        public VMResponse GetByFilter(long filterTreatmentId, long filterLocationId, string? filterDoctor, string? filterSpecialization)
        {
            return doctors.GetByFilter(filterTreatmentId, filterLocationId, filterDoctor, filterSpecialization);
        }

        [HttpGet("[action]")]
        public VMResponse GetTreatmentAll() => doctors.GetTreatmentAll();

        [HttpGet("[action]/{Id}")]
        public VMResponse DetailDoctor(long Id) => doctors.DetailDoctor(Id);

        [HttpGet("[action]/{Id}")]
        public VMResponse DoctorAppointment(long Id) => doctors.DoctorAppointment(Id);
    }
}
