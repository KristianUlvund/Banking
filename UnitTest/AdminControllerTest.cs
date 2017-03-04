using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using DAL;
using TheBank.Controllers;
using TheBank.Model;
using System.Collections.Generic;
using MvcContrib.TestHelper;
using System.Web.Mvc;
using TheBank.Model.DataContainer.Account;

namespace UnitTest
{
    [TestClass]
    public class AdminControllerTest
    {
        private DBStub db = new DBStub();

        [TestMethod]
        public void listTransactions_checkAllTransactions()
        {
            
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            var expectedResult = new List<TransactionPresentation>();
            var transaction1 = new TransactionPresentation();
            var transaction2 = new TransactionPresentation();
            var transaction3 = new TransactionPresentation();

            transaction1.ID = 1;
            transaction1.date = "07.11.2016";
            transaction1.amount = 100.00;
            transaction1.message = "This is the first transaction";
            transaction1.toAccount = "22222222222";
            transaction1.fromAccount = "11111111111";
            expectedResult.Add(transaction1);

            // Second transaction
            transaction2.ID = 2;
            transaction2.date = "07.11.2016";
            transaction2.amount = 200.00;
            transaction2.message = "This is the second transaction";
            transaction2.toAccount = "33333333333";
            transaction2.fromAccount = "22222222222";
            expectedResult.Add(transaction2);

            // Thirs transaction
            transaction3.ID = 3;
            transaction3.date = "07.11.2016";
            transaction3.amount = 300.00;
            transaction3.message = "This is the third transaction";
            transaction3.toAccount = "44444444444";
            transaction3.fromAccount = "33333333333";
            expectedResult.Add(transaction3);

            // Act
            List<TransactionPresentation> result = controller.listTransactions(false);

            // Assert

            for (var i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(expectedResult[i].ID, result[i].ID);
                Assert.AreEqual(expectedResult[i].date, result[i].date);
                Assert.AreEqual(expectedResult[i].amount, result[i].amount);
                Assert.AreEqual(expectedResult[i].message, result[i].message);
                Assert.AreEqual(expectedResult[i].toAccount, result[i].toAccount);
                Assert.AreEqual(expectedResult[i].fromAccount, result[i].fromAccount);
            }
        }

        [TestMethod]
        public void editUser_test()
        {
            // Arrange
            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));
            sessionMock.InitializeController(controller);

            string[] userInfo = { "11111111111" };
            controller.Session["userInfo"] = userInfo;

            // Customer object
            Customer editableCustomer = new Customer();
            editableCustomer.personalIdentification = "11111111111";
            editableCustomer.firstname = "Ola";
            editableCustomer.lastname = "Nordmann";
            editableCustomer.zipCode = "1234";
            editableCustomer.city = "Oslo";
            editableCustomer.address = "Nordmannsveien 25";
            editableCustomer.phoneNumber = "12345678";

            bool expectedResult = true;

            // Act
            bool result = controller.editUser(editableCustomer);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void editAccount_Test()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            //Account
            string accountNumber = "22222222222";
            string accountName = "SavingsAccount";

            // Act
            bool result = controller.editAccount(accountNumber, accountName);
            bool expectedResult = true;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void deactivateUserSuccess_Test()
        {
            // Arrange
            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));
            sessionMock.InitializeController(controller);

            // Creating users pID and Session
            string pID = "11111111111";
            string[] userInfo = new string[1];
            userInfo[0] = "11111111111";
            controller.Session["userInfo"] = userInfo;

            // Act
            string result = controller.deleteUser(pID);
            string expectedResult = "You cannot delete yourself!";

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void deleteUserAdmin_Test()
        {
            // Arrange
            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));
            sessionMock.InitializeController(controller);

            // Creating users pID and Session
            string pID = "11111111111";
            string[] userInfo = new string[1];
            userInfo[0] = "22222222222";
            controller.Session["userInfo"] = userInfo;

            // Act
            string result = controller.deleteUser(pID);
            string expectedResult = "User deleted succesfully!";

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        // TODO: Kan ha validering for at man må ha BrukerID
        //public void deleteUserNotSuccess_Test() 
        //{
        //    // Arrange
        //    var controller = new AdminController(new AdminBLL(new DBStub()));

        //    // Creating users pID
        //    string pID = "";

        //    // Act
        //    string result = controller.deleteUser(pID);
        //    string expectedResult = "You cannot delete yourself!";

        //    // Assert
        //    Assert.AreEqual(expectedResult, result);
        //}

        [TestMethod]
        public void deactivateAccountSuccess()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            // Creating users pID
            string aID = "22222222222";

            // Act
            bool result = controller.deactivateAccount(aID);
            bool expectedResult = true;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void executeTransactionAlreadyExecuted_Test()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            // Create transactionID
            string transactionID = "0";

            // Act
            string result = controller.executeTransaction(transactionID);
            string expectedResult = "Already Transferred!";

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void executeTransactionSuccess_Test()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            // Create transactionID
            string transactionID = "3";

            // Act
            string result = controller.executeTransaction(transactionID);
            string expectedResult = "ok";

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void executeTransactionNotEnoughMoney_Test()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));

            // Create transactionID
            string transactionID = "1";

            // Act
            string result = controller.executeTransaction(transactionID);
            string expectedResult = "Not enough money";

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void inspectUserSuccess()
        {
            // Assert
            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));
            sessionMock.InitializeController(controller);

            // Creating personalIdentification and inspectPersonalIdentification session
            string inspectPersonalIdentification = "11111111111";
            controller.Session["inspectPersonalID"] = inspectPersonalIdentification;

            // Act
            var result = (ViewResult)controller.InspectUser(inspectPersonalIdentification);

            // Assert
            Assert.AreEqual("", result.ViewName);
            CollectionAssert.AllItemsAreInstancesOfType((List<AccountPrimitive>)result.Model, 
                typeof(AccountPrimitive));

        }
        [TestMethod]
        public void inspectAccountSuccess()
        {
            // Assert
            var sessionMock = new TestControllerBuilder();
            var controller = new AdminController(new AdminBLL(db), new AccountBLL(db));
            sessionMock.InitializeController(controller);

            // Creating personalIdentification and inspectPersonalIdentification session
            string accountNumber = "11111111111";
            string inspectPersonalID = "22222222222";
            controller.Session["inspectPersonalID"] = inspectPersonalID;

            // Act
            var result = (ViewResult)controller.InspectUser(accountNumber);

            // Assert
            Assert.AreEqual("", result.ViewName);
            CollectionAssert.AllItemsAreInstancesOfType((List<AccountPrimitive>)result.Model,
                typeof(AccountPrimitive));

        }
    }
}

