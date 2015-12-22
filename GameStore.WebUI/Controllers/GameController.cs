﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Domain.Abstract;

namespace GameStore.WebUI.Controllers
{
    public class GameController : Controller
    {
        // GET: Game
        private IGameRepository repository;
        public GameController(IGameRepository repo)
        {
            repository = repo;
        }

        public ActionResult List()
        {
            return View(repository.Games);
        }
    }
}