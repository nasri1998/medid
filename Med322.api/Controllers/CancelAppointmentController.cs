using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CancelAppointmentController : Controller
    {

        private DACancelAppointment canApp;

        public CancelAppointmentController(Med322_BContext _db)
        {
            canApp = new DACancelAppointment(_db);
        }
        [HttpGet("[action]/{id?}")] // http methode
        public VMResponse GetAll(int id) => canApp.GetAll(id); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("[action]/{filter?}/{Id?}")]
        public VMResponse GetFilter(string filter, int Id)
        {
            return canApp.GetByFilter(filter, Id);
        }
        [HttpGet("[action]/{appid?}/{relid?}")]
        public VMResponse GetEdit(int appid, int relid) => canApp.GetEdit(appid, relid);
        [HttpGet("[action]/{apoinid?}")]
        public VMResponse Get(int apoinid) => canApp.Get(apoinid);

        [HttpGet("[action]/{docid?}")] // http methode
        public VMResponse GetAllTreatment(int docid) => canApp.GetAllTreatment(docid); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpDelete("[action]/{appoinid?}")]
        public VMResponse Delete(int appoinid)
        {
            return canApp.Delete(appoinid);
        }

    }
}
