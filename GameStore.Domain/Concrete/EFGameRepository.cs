﻿using System;
using System.Collections.Generic;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using System.Linq;

namespace GameStore.Domain.Concrete
{
    public class EFGameRepository : IGameRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Game> Games
        {
            get
            { return context.Games; }
        }

        public void SaveGame(Game game)
        {
            var item = context.Games.FirstOrDefault(g => g.GameId == game.GameId);
            if (item == null)
            {
                context.Games.Add(game);
            }
            else
            {
                item.Category = game.Category;
                item.Description = game.Description;
                item.Name = game.Name;
                item.Price = game.Price;
            }
            context.SaveChanges();
        }
    }
}
