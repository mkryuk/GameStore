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
        public PartialViewResult Menu(string category = null, bool horizontalNav = false)
        {
            var categories = repository.Games
                .Select(item => item.Category)
                .Distinct()
                .OrderBy(item => item);

            //var viewName = horizontalNav ? "MenuHorizontal" : "Menu";
            //return PartialView(new NavigationViewModel
            //{
            //    Categories = categories,
            //    CurrentCategory = category
            //});

            return PartialView("FlexMenu", new NavigationViewModel {
                Categories = categories,
                CurrentCategory = category
            });
        }
    }
}