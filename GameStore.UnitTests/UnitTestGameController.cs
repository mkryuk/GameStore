using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using GameStore.Domain.Abstract;
using GameStore.Domain.Entities;
using GameStore.WebUI.Controllers;
using GameStore.WebUI.Models;
using GameStore.WebUI.HtmlHelpers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class UnitTestGameController
    {
        [TestMethod]
        public void Can_Paginate()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name="Game1" },
                new Game {GameId = 2, Name="Game2" },
                new Game {GameId = 3, Name="Game3" },
                new Game {GameId = 4, Name="Game4" },
                new Game {GameId = 5, Name="Game5" },
            });

            var controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // act
            var result = (GamesListViewModel)((ViewResult)controller.List(null, 2)).Model;

            // assert
            var games = result.Games.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Game4");
            Assert.AreEqual(games[1].Name, "Game5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            var result = myHelper.PageLinks(pagingInfo, i => "Page" + i);
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Игра1"},
                new Game { GameId = 2, Name = "Игра2"},
                new Game { GameId = 3, Name = "Игра3"},
                new Game { GameId = 4, Name = "Игра4"},
                new Game { GameId = 5, Name = "Игра5"}
            });

            var controller = new GameController(mock.Object);
            controller.pageSize = 3;

            var result = (GamesListViewModel)controller.List(null, 2).Model;

            var pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Games_By_Category()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Game1", Category = "Cat1" },
                new Game {GameId = 2, Name = "Game2", Category = "Cat2" },
                new Game {GameId = 3, Name = "Game3", Category = "Cat1" },
                new Game {GameId = 4, Name = "Game4", Category = "Cat2" },
                new Game {GameId = 5, Name = "Game5", Category = "Cat3" },
                new Game {GameId = 6, Name = "Game6", Category = "Cat1" },
                new Game {GameId = 7, Name = "Game7", Category = "Cat3" },
            });

            var controller = new GameController(mock.Object);
            controller.pageSize = 3;

            var result = ((GamesListViewModel)controller.List("Cat2", 1).Model).Games.ToList();
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Game2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Game4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Game1", Category = "Cat1" },
                new Game {GameId = 2, Name = "Game2", Category = "Cat2" },
                new Game {GameId = 3, Name = "Game3", Category = "Cat1" },
                new Game {GameId = 4, Name = "Game4", Category = "Cat2" },
                new Game {GameId = 5, Name = "Game5", Category = "Cat3" },
                new Game {GameId = 6, Name = "Game6", Category = "Cat1" },
                new Game {GameId = 7, Name = "Game7", Category = "Cat3" },
            });

            var controller = new NavController(mock.Object);
            List<string> result = ((NavigationViewModel)controller.Menu().Model).Categories.ToList();

            Assert.AreEqual(result.Count(), 3);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            var mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game {GameId = 1, Name = "Game1", Category = "Cat1" },
                new Game {GameId = 2, Name = "Game2", Category = "Cat2" }
            });

            string categoryToSelect = "Cat2";
            var controller = new NavController(mock.Object);
            var result = ((NavigationViewModel)controller.Menu(categoryToSelect).Model).CurrentCategory;                        
            Assert.AreEqual(result, "Cat2");
        }

        [TestMethod]
        public void Generate_Category_Specific_Game_Count()
        {
            /// Организация (arrange)
            Mock<IGameRepository> mock = new Mock<IGameRepository>();
            mock.Setup(m => m.Games).Returns(new List<Game>
            {
                new Game { GameId = 1, Name = "Game1", Category="Cat1"},
                new Game { GameId = 2, Name = "Game2", Category="Cat2"},
                new Game { GameId = 3, Name = "Game3", Category="Cat1"},
                new Game { GameId = 4, Name = "Game4", Category="Cat2"},
                new Game { GameId = 5, Name = "Game5", Category="Cat3"}
            });

            GameController controller = new GameController(mock.Object);
            controller.pageSize = 3;

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((GamesListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((GamesListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((GamesListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((GamesListViewModel)controller.List(null).Model).PagingInfo.TotalItems;

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }        

    }
}
