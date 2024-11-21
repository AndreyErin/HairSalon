using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class ServicesController : Controller
    {
        private IRepositoryOfServices _services;
        public ServicesController(IRepositoryOfServices repositoryOfServices)
        {
            _services = repositoryOfServices;
        }

        public ViewResult Index()
        {
            return View(_services.GetAll());
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Pictures = Pictures.GetPictures();
            ViewBag.Title = "Добавить";
            return View("AddOrEdit", new Service());
        }

        [HttpPost]
        public RedirectToActionResult Add(Service service)
        {
            _services.Add(service);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult Edit(int id) 
        {
            ViewBag.Pictures = Pictures.GetPictures();
            ViewBag.Title = "Изменить";
            Service service = _services.Get(id);
            return View("AddOrEdit" ,service);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Service service) 
        {
            _services.Update(service);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public RedirectToActionResult Delete(int id) 
        {
            _services.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult AddPicture() 
        {
            return View(Pictures.GetPictures());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddFilePicture(IEnumerable<IFormFile> files) 
        {
            foreach (IFormFile item in files)
            {
                string fileFullPath = Directory.GetCurrentDirectory() + "/wwwroot/pictures/" + item.FileName;

                using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await item.CopyToAsync(fileStream);
                }
            }

            return RedirectToAction("AddPicture");
        }
    }
}
