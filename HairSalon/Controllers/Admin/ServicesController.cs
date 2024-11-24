using HairSalon.Model.Services;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    public class ServicesController : Controller
    {
        private IRepositoryOfServices _services;
        private PicturesManager _pictures;
        public ServicesController(IRepositoryOfServices repositoryOfServices)
        {
            _services = repositoryOfServices;
            _pictures = new PicturesManager();
        }

        public ViewResult Index()
        {
            return View(_services.GetAll());
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Pictures = _pictures.GetPictures();
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
            ViewBag.Pictures = _pictures.GetPictures();
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
        public ViewResult Pictures() 
        {
            return View(_pictures.GetPictures());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddPicturesAsync(IEnumerable<IFormFile> files) 
        {
            await _pictures.UploadPicturesAsync(files);

            return RedirectToAction("Pictures");
        }

        [HttpPost]
        public RedirectToActionResult DeletePicture(string fileShortPath)
        {
            _pictures.DeletePicture(fileShortPath);

            return RedirectToAction("Pictures");
        }
    }
}
