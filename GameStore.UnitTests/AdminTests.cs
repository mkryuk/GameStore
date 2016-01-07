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
using System.Web.Mvc;

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

        [TestMethod]
        public void CanEditGame()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games)
                .Returns(new List<Game>
                {
                    new Game { GameId = 1, Name = "Game1" },
                    new Game { GameId = 2, Name = "Game2" },
                    new Game { GameId = 3, Name = "Game3" },
                    new Game { GameId = 4, Name = "Game4" },
                });

            var controller = new AdminController(mock.Object);

            var game1 = controller.Edit(1).Model as Game;
            var game2 = controller.Edit(2).Model as Game;
            var game3 = controller.Edit(3).Model as Game;
            var game4 = controller.Edit(4).Model as Game;

            Assert.AreEqual(1, game1?.GameId);
            Assert.AreEqual(2, game2?.GameId);
            Assert.AreEqual(3, game3?.GameId);
            Assert.AreEqual(4, game4?.GameId);
        }

        [TestMethod]
        public void CannotEditNonExistentGame()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games)
                .Returns(new List<Game>
                {
                    new Game {GameId = 1, Name = "Game1" },
                    new Game {GameId = 2, Name = "Game2" },
                    new Game {GameId = 3, Name = "Game3" },
                });

            var controller = new AdminController(mock.Object);
            var game = controller.Edit(4).Model as Game;

            Assert.IsNull(game);
        }

        [TestMethod]
        public void CanSaveValidGame()
        {
            var mock = new Mock<IGameRepository>();           
            var controller = new AdminController(mock.Object);
            var game = new Game { Name = "Game1" };
            var result = controller.Edit(game);
            // Check that SaveGame method has been called
            mock.Verify(m => m.SaveGame(game));
            // Check that result is RedirectResult
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CanNotSaveInvalidGame()
        {
            var mock = new Mock<IGameRepository>();
            var controller = new AdminController(mock.Object);
            var game = new Game { Name = "Game1" };
            // Add error to the ModelState
            controller.ModelState.AddModelError("error", "error");
            var result = controller.Edit(game);
            // Check that SaveGame method has been called
            mock.Verify(m => m.SaveGame(It.IsAny<Game>()), Times.Never);
            // Check that result is ViewResult
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void CanDeleteValidGame()
        {
            var mock = new Mock<IGameRepository>();
            var game = new Game {GameId = 2, Name = "Game2"};
            mock.Setup(m => m.Games)
                .Returns(new List<Game>
                {
                    new Game {GameId = 1, Name = "Game1" },
                    new Game {GameId = 2, Name = "Game2" },
                    new Game {GameId = 3, Name = "Game3" },
                    new Game {GameId = 4, Name = "Game4" },
                });

            var controller = new AdminController(mock.Object);

            controller.Delete(game.GameId);

            mock.Verify(m => m.DeleteGame(game.GameId));
        }
    }
}
