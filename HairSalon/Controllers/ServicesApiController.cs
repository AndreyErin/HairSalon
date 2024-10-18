using HairSalon.Model;
using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/service")]
    public class ServicesApiController : Controller
    {
        private IRepositoryOfServices _services;
        public ServicesApiController(IRepositoryOfServices services) 
        {
            _services = services;
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            PackageMessage packageMessage = new(true, _services.GetAll());
            return Json(packageMessage);
        }

        [HttpPost]
        public JsonResult Add(Service service)
        {
            PackageMessage packageMessage;
            int result = _services.Add(service);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Услуга не была добавлена.");
            return Json(packageMessage);

        }

        [HttpPatch]
        public JsonResult Update(Service service)
        {
            PackageMessage packageMessage;
            int result = _services.Update(service);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Услуга не была изменена.");
            return Json(packageMessage);
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id)
        {
            PackageMessage packageMessage;
            Service? result = _services.Get(id);

            if (result != null)
            {
                packageMessage = new(true, result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Услуга, с таким ID, не найдена.");
            return Json(packageMessage);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id) 
        {
            PackageMessage packageMessage;
            int result = _services.Delete(id);

            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Услуга не была удалена. (Услуга, с таким ID, не найдена.)");
            return Json(packageMessage);
        } 
    }
}
