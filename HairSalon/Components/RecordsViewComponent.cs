using HairSalon.Model.Configuration;
using HairSalon.Model.Employees;
using HairSalon.Model.Records;
using HairSalon.Model.Records.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;

namespace HairSalon.Components
{
    public class RecordsViewComponent: ViewComponent
    {
        private RecordViewModelFactory _rvmFactory;
        public RecordsViewComponent(IRepositoryOfRecords records, 
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            _rvmFactory = new(records, employees, configuration);
        }

        public ViewViewComponentResult Invoke()
        {
            var model = _rvmFactory.CreateRecordsForEmployeeAllDaysModelList();

            return View(model);
        }
    }
}
