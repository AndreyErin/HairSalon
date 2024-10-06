using HairSalon.Model;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServicesApiController : Controller
    {
        private IRepositoryOfServices<Service> _services;
        public ServicesApiController(IRepositoryOfServices<Service> services) 
        {
            _services = services;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            return Json(_services.GetAll());
        }

        [HttpPost]
        public int Add(Service service)
        {
            return _services.Add(service);
        }

        [HttpPatch]
        public int Update(Service service)
        {
            return _services.Update(service);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            return Json(_services.Get(id));
        }

        [HttpDelete]
        [Route("{id}")]
        public int Delete(int id) 
        {
            return _services.Delete(id); 
        }
    }
}
