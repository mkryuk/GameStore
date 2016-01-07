using GameStore.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Entities;

namespace GameStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IGameRepository repository;

        public AdminController(IGameRepository repo)
        {
            repository = repo;
        }

        // GET: Admin/Index
        public ViewResult Index()
        {
            return View(repository.Games);
        }

        // GET: Admin/Edit?Gameid={gameid}
        public ViewResult Edit(int gameId)
        {
            var game = repository.Games.FirstOrDefault(g => g.GameId == gameId);
            return View(game);
        }

        // POST: /Admin/Edit?Gameid={gameid}
        [HttpPost]
        public ActionResult Edit(Game game)
        {
            if (!ModelState.IsValid) return View(game);

            repository.SaveGame(game);
            TempData["message"] = $"Изменения в игре \"{game.Name}\" были сохранены";
            return RedirectToAction("Index");
        }

    }
}