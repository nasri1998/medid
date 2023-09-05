using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDataController : Controller
    {
        private DAUserData userData;

        public UserDataController(Med322_BContext _db)
        {
            userData = new DAUserData(_db);
        }

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return userData.Get(id);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => userData.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS
    }
}
