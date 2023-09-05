using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationLevelController : Controller
    {
        private DALocationLevel locationlevel;

        public LocationLevelController(Med322_BContext _db)
        {
            locationlevel = new DALocationLevel(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => locationlevel.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return locationlevel.Get(id);
        }

        [HttpPost]
        public VMResponse Post(VMLocationLevel Llevel) => locationlevel.CreateUpdate(Llevel);

        [HttpPut]
        public VMResponse Put(VMLocationLevel Llevel) => locationlevel.CreateUpdate(Llevel); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => locationlevel.Delete(id, userid); // Delete record
    }
}
