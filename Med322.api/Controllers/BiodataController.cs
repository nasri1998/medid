using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BiodataController : Controller
    {
        private DABiodata Biodata;

        public BiodataController(Med322_BContext _db)
        {
            Biodata = new DABiodata(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => Biodata.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return Biodata.Get(id);
        }

        [HttpPost("[action]")]
        public VMResponse Create(VMBiodata bdata) => Biodata.CreateUpdate(bdata);

        [HttpPut("[action]")]
        public VMResponse Update(VMBiodata bdata) => Biodata.CreateUpdate(bdata); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => Biodata.Delete(id, userid); // Delete record
    }
}
