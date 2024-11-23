using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class RecordsViewComponent: ViewComponent
    {
        private IRepositoryOfRecords _records;
        private IRepositoryOfEmployees _employees;
        public RecordsViewComponent(IRepositoryOfRecords repositoryOfRecords, IRepositoryOfEmployees repositoryOfEmployees)
        {
            _records = repositoryOfRecords;
            _employees = repositoryOfEmployees;
        }

        public ViewViewComponentResult Invoke()
        {
            RecordsForEmployeeService recordsService = new(_records, _employees);

            return View(recordsService.GetRecords());
        }
    }
}
