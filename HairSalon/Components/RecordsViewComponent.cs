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
        private RecordsService recordsModelService;
        public RecordsViewComponent(IRepositoryOfRecords records, 
            IRepositoryOfEmployees employees,
            IRepositoryOfConfiguration configuration)
        {
            recordsModelService = new(records, employees, configuration);
        }

        public ViewViewComponentResult Invoke()
        {
            var model = recordsModelService.GetAll();

            return View(model);
        }
    }
}
