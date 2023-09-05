using Med322.DataAccess;
using Med322.DataModels;
using Med322.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Med322.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecializationController : Controller
    {

        private DASpecialization specializations;
        public SpecializationController(Med322_BContext _db)
        {
            specializations = new DASpecialization(_db);
        }

        [HttpGet]
        public VMResponse GetAll() => specializations.GetAll();

        [HttpGet("{id?}")]
        public VMMSpecialization GetById(long id) => specializations.GetById(id);

    }
}
