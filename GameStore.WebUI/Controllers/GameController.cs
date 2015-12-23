﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Abstract;
using GameStore.WebUI.Models;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        private IGameRepository repository;
        public int pageSize = 4;
        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ActionResult List(int page = 1)
        {
            var model = new GamesListViewModel
            {
                Games = repository.Games
                .OrderBy(game => game.GameId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Games.Count()
                }
            };
            return View(model);
        }
    }
}