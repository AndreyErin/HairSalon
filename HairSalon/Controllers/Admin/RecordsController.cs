using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class RecordsController : Controller
    {
        private RecordViewModelFactory _rvmFactory;
        private RecordViewModelAdapter _rvmAdapter;
        private IRepositoryOfEmployees _employees;

        public RecordsController(IRepositoryOfRecords records,
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _employees = employees;
            _rvmFactory = new(records, employees, configuration);
            _rvmAdapter = new(records);
        }

        public ViewResult Index()
        {
            return View(_rvmFactory.CreateWorkDatesModelArray());
        }

        [HttpPost]
        public RedirectToActionResult SetDaysForRecords([FromForm] WorkDatesModel[] recordsModels)
        {
            _rvmAdapter.SetWorkDates(recordsModels);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Day(string date)
        {
            DateOnly DateDay = DateOnly.Parse(date);
            List<RecordsForEmployeeOfDayModel> model = _rvmFactory.CreateRecordsForEmployeeOfDayModelList(DateDay);

            return View(model);
        }

        [HttpGet]
        public ViewResult EditTimeOfDayForEmployee(string date, int employeeId)
        {
            DateOnly dateDay = DateOnly.Parse(date);

            TimeForRecordModel[] model = _rvmFactory.CreateTimeForRecordModelArray(dateDay, employeeId);

            ViewBag.EmployeeName = _employees.Get(employeeId)?.Name;

            return View(model);
        }

        [HttpPost]
        public RedirectToActionResult SetTimeOfDayForEmployee([FromForm] TimeForRecordModel[] recordModels)
        {
            _rvmAdapter.SetTimeTable(recordModels);

            return RedirectToAction("Index");
        }
    }
}
