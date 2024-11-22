using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class RecordsController : Controller
    {
        private IRepositoryOfRecords _records;
        private IRepositoryOfEmployees _employees;

        public RecordsController(IRepositoryOfRecords repositoryOfRecords,
            IRepositoryOfEmployees repositoryOfEmployees)
        {
            _records = repositoryOfRecords;
            _employees = repositoryOfEmployees;
        }

        public ViewResult Index()
        {
            RecordsForEmployeeService recordsService = new(_records.GetAll(), _employees.GetAll());

            return View(recordsService.GetRecords());
        }
    }
}
