using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;
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
            return PartialView(new NavigationViewModel
            {
                Categories = categories,
                CurrentCategory = category
            });
        }
    }
}