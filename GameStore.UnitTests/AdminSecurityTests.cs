using GameStore.WebUI.Controllers;
using GameStore.WebUI.Infrastructure.Abstract;
using GameStore.WebUI.Models;
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
    public class AdminSecurityTests
    {
        [TestMethod]
        public void CanLoginWithValidCredentials()
        {
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "123456"))
                .Returns(true);

            var model = new LoginViewModel
            {
                Username = "admin",
                Password = "123456"
            };

            var controller = new AccountController(mock.Object);

            var result = controller.Login(model, "/TestUrl");

            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/TestUrl", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void CannotLoginWithValidCredentials()
        {
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("hacker", "badPass"))
                .Returns(false);

            var model = new LoginViewModel
            {
                Username = "hacker",
                Password = "badPass"
            };

            var controller = new AccountController(mock.Object);

            var result = controller.Login(model, "/TestUrl");

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
