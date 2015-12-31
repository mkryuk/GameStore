using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using GameStore.Domain.Abstract;
using Moq;
using System.Collections.Generic;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;

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

        [TestMethod]
        public void CanAddToCart()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(item => item.Games).Returns(new List<Game> {
                new Game { GameId = 1, Name = "Game1", Category = "Category1"}                
            }.AsQueryable());

            var cart = new Cart();
            var controller = new CartController(mock.Object);

            controller.AddToCart(cart, 1, null);

            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines[0].Game.GameId, 1);
        }

        [TestMethod]
        public void AddingGameToCartLeadsToCartScreen()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(item => item.Games).Returns(new List<Game> {
                new Game { GameId = 1, Name = "Game1", Category = "Category1"}
            }.AsQueryable());

            var cart = new Cart();
            var controller = new CartController(mock.Object);

            var result = controller.AddToCart(cart, 1, "myUrl");

            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void CanViewCartContents()
        {
            var cart = new Cart();
            var target = new CartController(null);

            var result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }

    }
}
