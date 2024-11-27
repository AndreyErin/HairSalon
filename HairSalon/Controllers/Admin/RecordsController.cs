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
        public ViewResult Day(string date)
        {
            DateOnly DateDay = DateOnly.Parse(date);
            List<RecordsForEmployeeOfDay> model = _recordsService.GetRecordsOfDayForEmpoyees(DateDay);

            return View(model);
        }

        [HttpGet]
        public ViewResult EditTimeOfDayForEmployee(string date, int employeeId)
        {
            DateOnly dateDay = DateOnly.Parse(date);

            TimeForRecordModel[] model = _recordsService.GetTimeOfDayForEmployee(dateDay, employeeId);

            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult SetTimeOfDayForEmployee([FromForm] TimeForRecordModel[] recordModels)
        {
            _recordsService.SetTimeOfDayForEmployee(recordModels);

            return RedirectToAction("Index");
        }
    }
}
