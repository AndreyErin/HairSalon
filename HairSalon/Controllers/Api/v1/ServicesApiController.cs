using HairSalon.Model.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Api.v1
{
    [ApiController]
    [Route("api/v1/services")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ServicesApiController : Controller
    {
        private IRepositoryOfServices _services;
        public ServicesApiController(IRepositoryOfServices services)
        {
            _services = services;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            if (_services.GetAll().Count > 0)
            {
                return Ok(_services.GetAll());
            }
            else
            {
                return NotFound("Ошибка. В базе нет услуг.");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0) 
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            Service? result = _services.Get(id);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Ошибка. Услуга, с таким id, не найдена.");
            }
        }

        [HttpPost]
        public IActionResult Add(Service service)
        {
            if (service.IsValid() == false)
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            int result = _services.Add(service);
            if (result == 1)
            {
                return Ok(service);
            }
            else
            {
                return Conflict("Ошибка. Услуга с таким названием уже существует.");
            }
        }

        [HttpPatch]
        public IActionResult Update(Service service)
        {
            if (service.IsValid() == false || service.Id <= 0)
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            int result = _services.Update(service);
            if (result == 1)
            {
                return Ok(service);
            }
            else
            {
                return NotFound("Ошибка. Услуга, с таким id, не найдена.");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return UnprocessableEntity("Ошибка. Некорректный id.");
            }

            int result = _services.Delete(id);
            if (result == 1)
            {
                return Ok();
            }
            else
            {
                return NotFound("Ошибка. Услуга, с таким id, не найдена.");
            }
        }
    }
}
