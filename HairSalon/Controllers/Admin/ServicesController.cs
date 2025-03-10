﻿using HairSalon.Model.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HairSalon.Controllers.Admin
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ServicesController : Controller
    {
        private IRepositoryOfServices _services;
        private IPicturesManager _pictures;
        public ServicesController(IRepositoryOfServices repositoryOfServices, IPicturesManager picturesManager)
        {
            _services = repositoryOfServices;
            _pictures = picturesManager;
        }

        public ViewResult Index()
        {
            return View(_services.GetAll());
        }

        [HttpGet]
        public ViewResult Add()
        {
            ViewBag.Pictures = _pictures.GetAll();
            ViewBag.Title = "Добавить";
            return View("AddOrEdit", new Service());
        }

        [HttpPost]
        public RedirectToActionResult Add(Service service)
        {
            int result = _services.Add(service);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Ошибка. Услуга с таким названием уже есть в базе.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }

        [HttpGet]
        public ViewResult Edit(int id) 
        {
            ViewBag.Pictures = _pictures.GetAll();
            ViewBag.Title = "Изменить";
            Service service = _services.Get(id) ?? new();
            return View("AddOrEdit" ,service);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Service service) 
        {
            int result = _services.Update(service);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Ошибка. Услуга с указанным id не найдена.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }

        [HttpPost]
        public RedirectToActionResult Delete(int id) 
        {
            int result = _services.Delete(id);
            if (result == 1)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Ошибка. Услуги с таким id нет в базе.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }

        [HttpGet]
        public ViewResult Pictures() 
        {
            return View(_pictures.GetAll());
        }

        [HttpPost]
        public async Task<RedirectToActionResult> AddPicturesAsync(IEnumerable<IFormFile> files) 
        {
            int result = await _pictures.UploadAsync(files);
            if (result == 1)
            {
                return RedirectToAction("Pictures");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Ошибка при попытке добавить изображения.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }

        [HttpPost]
        public RedirectToActionResult DeletePicture(string fileShortPath)
        {
            int result = _pictures.Delete(fileShortPath);
            if (result == 1)
            {
                return RedirectToAction("Pictures");
            }
            else
            {
                string errorMessage = Uri.EscapeDataString("Ошибка при попытке удалить изображение.");
                return RedirectToAction("ErrorPage", "Admin", new { errorMessage });
            }
        }
    }
}
