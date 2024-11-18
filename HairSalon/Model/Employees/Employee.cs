using System.ComponentModel.DataAnnotations;

namespace HairSalon.Model.Employees
{
    public class Employee
    {
        [Display(Name = "Табельный номер")]
        public int Id { get; set; }

        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Должность")]
        public string Post { get; set; }//должность
    }
}
