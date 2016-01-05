using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void IndexContainsAllGames()
        {
            var mock = new Mock<IGameRepository>();
            mock
            .Setup(m => m.Games)
            .Returns(new List<Game> {
                    new Game { GameId = 1, Name = "Game1" },
                    new Game { GameId = 2, Name = "Game2" },
                    new Game { GameId = 3, Name = "Game3" },
                    new Game { GameId = 4, Name = "Game4" },
                    new Game { GameId = 5, Name = "Game5" },
            });

            var controller = new AdminController(mock.Object);

            var result = ((IEnumerable<Game>)controller.Index().ViewData.Model).ToList();

            Assert.AreEqual(result.Count, 5);
            Assert.AreEqual("Game1", result[0].Name);
            Assert.AreEqual("Game5", result[4].Name);
            Assert.AreEqual("Game3", result[2].Name);
        }
    }
}
