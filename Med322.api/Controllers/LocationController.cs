using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        private DALocation location;

        public LocationController(Med322_BContext _db)
        {
            location = new DALocation(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => location.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return location.Get(id);
        }

        [HttpPost]
        public VMResponse Post(VMLocation loc) => location.CreateUpdate(loc);

        [HttpPut]
        public VMResponse Put(VMLocation loc) => location.CreateUpdate(loc); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => location.Delete(id, userid); // Delete record
    }
}
