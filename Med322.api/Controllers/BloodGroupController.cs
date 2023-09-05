using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BloodGroupController : Controller
    {
        private DABloodGroup bloodGroup;

        public BloodGroupController(Med322_BContext _db)
        {
            bloodGroup = new DABloodGroup(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => bloodGroup.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return bloodGroup.Get(id);
        }

        [HttpGet("[action]/{filter?}")]
        public VMResponse Get(string filter)
        {
            return bloodGroup.GetByFilter(filter);
        }

        [HttpPost]
        public VMResponse Post(VMTblMBloodGroup bgroup) => bloodGroup.CreateUpdate(bgroup);

        [HttpPut]
        public VMResponse Put(VMTblMBloodGroup bgroup) => bloodGroup.CreateUpdate(bgroup); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => bloodGroup.Delete(id, userid); // Delete record
    }
}
