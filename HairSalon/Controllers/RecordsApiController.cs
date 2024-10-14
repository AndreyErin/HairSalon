using HairSalon.Model;
using HairSalon.Model.Records;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace HairSalon.Controllers
{
    [ApiController]
    [Route("api/records")]
    public class RecordsApiController : Controller
    {
        IRepositoryOfRecords _records;
        public RecordsApiController(IRepositoryOfRecords records)
        {
            _records = records; 
        }

        [HttpGet]
        public JsonResult GetAll()
        {
            return Json(new PackageMessage(true, _records.GetAll()));
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id) 
        {
            PackageMessage packageMessage;

            Record? result = _records.Get(id);
            if (result != null)
            {
                packageMessage = new(true, data: result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись, с таким ID, не найдена.");
            return Json(packageMessage);
        }

        [HttpGet]
        [Route("forname")]           
        public JsonResult Get(string name)
        {
            PackageMessage packageMessage;

            Record? result = _records.Get(name);
            if (result != null)
            {
                packageMessage = new(true, data: result);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись, с таким именем, не найдена.");
            return Json(packageMessage);
        }

        [HttpDelete]
        [Route("{id}")]
        public JsonResult Delete(int id)
        {
            PackageMessage packageMessage;

            int result = _records.Delete(id);
            if (result == 1)
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Запись не была удалена.");
            return Json(packageMessage);
        }

        [HttpPost]
        public JsonResult Add(Record record)
        {
            PackageMessage packageMessage;

            int result = _records.Add(record);
            if (result == 1) 
            {
                packageMessage = new(true);
                return Json(packageMessage);
            }

            packageMessage = new(false, errorText: "Ошибка. Выбранное время уже занято.");
            return Json(packageMessage);
        }
    }
}
