using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IGameRepository repository;
        public NavController(IGameRepository repo)
        {
            repository = repo;
        }
        // GET: Nav
        public PartialViewResult Menu(string category = null)
        {
            var categories = repository.Games
                .Select(item => item.Category)
                .Distinct()
                .OrderBy(item => item);
            ViewBag.SelectedCategory = category;
            return PartialView(categories);
        }
    }
}