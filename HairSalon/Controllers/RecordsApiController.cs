using HairSalon.Model.Records;
using Microsoft.AspNetCore.Mvc;

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
            return Json(_records.GetAll());
        }

        [HttpGet]
        [Route("{id}")]
        public JsonResult Get(int id) 
        {
            return Json(_records.Get(id));
        }

        [HttpGet]
        [Route("forname")]           
        public JsonResult Get(string name)
        {
            return Json(_records.Get(name));
        }

        [HttpDelete]
        [Route("{id}")]
        public int Delete(int id)
        {
            return _records.Delete(id);
        }

        [HttpPost]
        public int Add(Record record)
        {
            return _records.Add(record);
        }
    }
}
