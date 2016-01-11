using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;

namespace GameStore.UnitTests
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void CanRetreiveImageData()
        {
            var game = new Game
            {
                GameId = 2,
                Name = "Game2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games)
                .Returns(new List<Game>
                {
                    new Game {GameId = 1, Name = "Game1" },
                    game,
                    new Game {GameId = 3, Name = "Game3" }
                }.AsQueryable());
            var controller = new GameController(mock.Object);

            var result = controller.GetImage(2);

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void CannotRetreiveImageDataForInvalidId()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games)
                .Returns(new List<Game>
                {
                    new Game { GameId = 1, Name = "Game1" },
                    new Game { GameId = 2, Name = "Game2" }
                }.AsQueryable());

            var controller = new GameController(mock.Object);

            var result = controller.GetImage(3);

            Assert.IsNull(result);
        }

    }
}
