using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerRelationController : Controller
    {
        private DACustomerRelation customerRelation;

        public CustomerRelationController(Med322_BContext _db)
        {
            customerRelation = new DACustomerRelation(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => customerRelation.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id?}")]
        public VMResponse Get(int id)   // KALAU FUNCTION LEBIH DARI SATU BARIS GUNAKAN YANG BIASA SEPERTI INI
        {
            return customerRelation.Get(id);
        }

        [HttpGet("[action]/{filter?}")]
        public VMResponse Get(string filter)
        {
            return customerRelation.GetByFilter(filter);
        }

        [HttpPost]
        public VMResponse Post(VMTblMCustomerRelation relate) => customerRelation.CreateUpdate(relate);
        
        [HttpPut]
        public VMResponse Put(VMTblMCustomerRelation relate) => customerRelation.CreateUpdate(relate);

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => customerRelation.Delete(id, userid);
    }
}
