using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class RecordsController : Controller
    {
        private RecordsModelService _recordsService;

        public RecordsController(IRepositoryOfRecords records,
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _recordsService = new(records, employees, configuration);
        }

        public ViewResult Index()
        {
            return View(_recordsService.GetDaysForRecords().ToArray());
        }

        [HttpPost]
        public RedirectResult SetDaysForRecords([FromForm] DayForRecordsModel[] recordsModels)
        {
            _recordsService.SetDaysForRecords(recordsModels.ToList());

            return Redirect("Index");
        }

        [HttpGet]
        public ViewResult EditDay(int year, int month, int day)
        {
            DateOnly DateDay = new(year, month, day);
            List<RecordsForEmployeeOfDay> model = _recordsService.GetRecordsOfDayForEmpoyees(DateDay);

            return View(model);
        }
    }
}
