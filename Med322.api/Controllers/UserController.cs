using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private DAUser user;

        public UserController(Med322_BContext _db)
        {
            user = new DAUser(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => user.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return user.Get(id);
        }

        [HttpPost("[action]")]
        public VMResponse Create(VMUser users) => user.CreateUpdate(users);

		[HttpPut("[action]")]
		public VMResponse Update(VMUser users) => user.CreateUpdate(users); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => user.Delete(id, userid); // Delete record
    }
}
