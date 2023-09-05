using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private DACustomer customer;

        public CustomerController(Med322_BContext _db)
        {
            customer = new DACustomer(_db);
        }

        [HttpGet("[action]")] // http methode
        public VMResponse GetAll() => customer.GetAll(); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("{id}")] // http methode
        public VMResponse Get(int id) => customer.Get(id); // ARROW FUNCTION HANYA BUTUH 1 BARIS

        [HttpGet("[action]/{id}")] // http methode
        public VMResponse GetCustomerId(int id) => customer.GetCustomerId(id); 

        [HttpPost("[action]")]
        public VMResponse Create(VMCustomer Cs) => customer.CreateUpdate(Cs);

        [HttpPut("[action]")]
        public VMResponse Update(VMCustomer Cs) => customer.CreateUpdate(Cs); // Update record

        [HttpDelete("[action]/{id}/{userid}")]
        public VMResponse Delete(int id, int userid) => customer.Delete(id, userid); // Delete record
    }
}
