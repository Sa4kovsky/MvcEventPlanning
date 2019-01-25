using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        IRepository repository;

        public AdminController(IRepository repo)
        {
            repository = repo;
        }

        public ViewResult Index()
        {
            return View(repository.Activities);
        }

        public ViewResult Edit(int activityId)
        {
            Activity activity = repository.Activities
                .FirstOrDefault(g => g.ActivityId == activityId);
            return View(activity);
        }

        [HttpPost]
        public ActionResult Edit(Activity activity)
        {
            if (ModelState.IsValid)
            {
                repository.SaveActivity(activity);
                TempData["message"] = string.Format("Изменения в мероприятии \"{0}\" были сохранены", activity.NameEvent);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(activity);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Activity());
        }

        [HttpPost]
        public ActionResult Delete(int activityId)
        {
            Activity deletedActivity = repository.DeleteActivity(activityId);
            if (deletedActivity != null)
            {
                TempData["message"] = string.Format("Мероприятие \"{0}\" было удалено",
                    deletedActivity.NameEvent);
            }
            return RedirectToAction("Index");
        }
    }
}
