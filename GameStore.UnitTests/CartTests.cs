using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;

namespace GameStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void CanAddNewLines()
        {
            var game1 = new Game { GameId = 1, Name = "Game1" };
            var game2 = new Game { GameId = 2, Name = "Game2" };

            var cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            var results = cart.Lines;

            Assert.AreEqual(results.Count, 2);
            Assert.AreEqual(results[0].Game, game1);
            Assert.AreEqual(results[1].Game, game2);
        }

        [TestMethod]
        public void CanAddQuantityForExistingLines()
        {
            var game1 = new Game { GameId = 1, Name = "Game1" };
            var game2 = new Game { GameId = 2, Name = "Game2" };

            var cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game1, 5);
            
            var results = cart.Lines.OrderBy(c => c.Game.GameId).ToList();

            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);
            Assert.AreEqual(results[1].Quantity, 1);
        }


        [TestMethod]
        public void CanRemoveLine()
        {
            var game1 = new Game { GameId = 1, Name = "Game1" };            
            var game2 = new Game { GameId = 2, Name = "Game2" };
            var game3 = new Game { GameId = 3, Name = "Game3" };

            var cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 1);
            cart.AddItem(game3, 1);

            cart.RemoveLine(game2);

            var result = cart.Lines.Where(it => it.Game == game2);

            Assert.AreEqual(result.Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void CalculateCartTotal()
        {
            var game1 = new Game { GameId = 1, Name = "Game1", Price = 100};
            var game2 = new Game { GameId = 2, Name = "Game2", Price = 77 };

            var cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 3);
            cart.AddItem(game1, 7);

            var result = cart.ComputeTotalValue();
            Assert.AreEqual(result, 1031);
        }

        [TestMethod]
        public void CanClearContents()
        {
            var game1 = new Game { GameId = 1, Name = "Game1", Price = 100 };
            var game2 = new Game { GameId = 2, Name = "Game2", Price = 77 };

            var cart = new Cart();
            cart.AddItem(game1, 1);
            cart.AddItem(game2, 3);
            cart.AddItem(game1, 7);

            cart.Clear();

            Assert.AreEqual(cart.Lines.Count(), 0);
        }

    }
}
