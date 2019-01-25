using Domain.Abstract;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class ActivitysController : Controller
    {
        private IRepository repository;
       
        public ActivitysController(IRepository repo)
        {
            repository = repo;
        }

        public ViewResult List(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            
            return View(repository.Activities.OrderBy(c => c.ActivityId).ToPagedList(pageNumber, pageSize));
        }

        public ViewResult Lists()
        {
            return View(repository.Participants);
        }
    }
}