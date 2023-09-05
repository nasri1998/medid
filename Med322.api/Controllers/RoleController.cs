using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private DARole Role;

        public RoleController(Med322_BContext _db)
        {
            Role = new DARole(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => Role.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return Role.Get(id);
        }

        [HttpPost("[action]")]
        public VMResponse Create(VMRole roles) => Role.CreateUpdate(roles);

        [HttpPut("[action]")]
        public VMResponse Update(VMRole roles) => Role.CreateUpdate(roles); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => Role.Delete(id, userid); // Delete record
    }
}
