using BLL;
using DAL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MvcContrib.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheBank.Controllers;

namespace UnitTest
{
    [TestClass]
    public class ajaxControllerTest
    {
        private DBStub db = new DBStub();

        [TestMethod]
        public void logInSuccess_Test()
        {
            // Arrange
            var sessionMock = new TestControllerBuilder();
            var controller = new AjaxController(new SecurityBLL(db));
            sessionMock.InitializeController(controller);

            string pID = "11111111111";
            string password = "pass";

            // Act
            var result = controller.logIn(pID, password);
            string expectedResult = "true";

            // Assert

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void logInFailed_Test()
        {
            // Arrange
            var controller = new AjaxController(new SecurityBLL(db));

            string pID = "22222222222";
            string password = "pass";

            // Act
            var result = controller.logIn(pID, password);
            string expectedResult = "false";

            // Assert

            Assert.AreEqual(expectedResult, result);
        }
    }
}
