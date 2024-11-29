using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class RecordsController : Controller
    {
        private RecordsModelManager _recordsModelManager;
        private IRepositoryOfEmployees _employees;

        public RecordsController(IRepositoryOfRecords records,
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _employees = employees;
            _recordsModelManager = new(records, employees, configuration);
        }

        public ViewResult Index()
        {
            return View(_recordsModelManager.GetWorkDates().ToArray());
        }

        [HttpPost]
        public RedirectResult SetDaysForRecords([FromForm] WorkDatesModel[] recordsModels)
        {
            _recordsModelManager.SetWorkDates(recordsModels.ToList());

            return Redirect("Index");
        }

        [HttpGet]
        public ViewResult Day(string date)
        {
            DateOnly DateDay = DateOnly.Parse(date);
            List<RecordsForEmployeeOfDay> model = _recordsModelManager.GetForDate(DateDay);

            return View(model);
        }

        [HttpGet]
        public ViewResult EditTimeOfDayForEmployee(string date, int employeeId)
        {
            DateOnly dateDay = DateOnly.Parse(date);

            TimeForRecordModel[] model = _recordsModelManager.GetTimeTable(dateDay, employeeId);

            ViewBag.EmployeeName = _employees.Get(employeeId)?.Name;

            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult SetTimeOfDayForEmployee([FromForm] TimeForRecordModel[] recordModels)
        {
            _recordsModelManager.SetTimeTable(recordModels);

            return RedirectToAction("Index");
        }
    }
}
