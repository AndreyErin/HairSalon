using System.ComponentModel.DataAnnotations;

namespace HairSalon.Model.Services
{
    public class Service
    {
        [Display(Name = "Идентификатор")]
        public int Id { get; set; }

        [Display(Name = "Название")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Изображение")]
        public string Picture { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        [Display(Name = "Время")]
        public TimeSpan TimeOfService { get; set; }

    }
}
