using BLL;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using TheBank.Model;

namespace TheBank.Controllers
{
    public class AdminController : Controller
    {
        // The variable used to direct live or test environment.
        private IAdminBLL _adminBLL;
        private IAccountBLL _accBLL;

        // Actual data
        public AdminController()
        {
            _adminBLL = new AdminBLL();
            _accBLL = new AccountBLL();
        }

        // Test environment.
        public AdminController(IAdminBLL admin_stub, IAccountBLL acc_stub)
        {
            _adminBLL = admin_stub;
            _accBLL = acc_stub;
        }
        /*
         ********************************************************
         */

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public string executeTransaction(string transactionID)
        {
            return _adminBLL.executeTransaction(Int32.Parse(transactionID));
        }

        public ActionResult InspectUser(string inspectPersonalID)
        {
            var model = _accBLL.listAccountPrimitive(inspectPersonalID);
            Session["inspectPersonalID"] = _adminBLL.getUserInfo(inspectPersonalID);
            return View(model);
        }

        public List<TransactionPresentation> listTransactions(bool isTransferred)
        {
            return _adminBLL.listTransactions(isTransferred);
        }

        public ActionResult InspectAccount(string accountNumber)
        {
            var model = _accBLL.getAccountData(accountNumber, ((string[])Session["inspectPersonalID"])[0]);
            return View(model);
        }

        public bool editUser(Customer c)
        {
            if (_adminBLL.editUser(c) && c.personalIdentification == ((string[])Session["userInfo"])[0])
            {
                string[] tempUserInfo = _adminBLL.getUserInfo(c.personalIdentification);
                string[] userInfo = new string[4];
                for (int i = 0; i < userInfo.Length-1; i++)
                {
                    userInfo[i] = tempUserInfo[i]; 
                }
                userInfo[3] = "true";
                Session["userInfo"] = userInfo;
            }
            return _adminBLL.editUser(c);
        }

        public string deleteUser(string pID)
        {
            if(pID == ((string[])Session["userInfo"])[0])
            {
                return "You cannot delete yourself!";
            }
            if(_adminBLL.deleteUser(pID))
            {
                return "User deleted succesfully!";
            }
            else
            {
                return "An error occurred";
            }
        }

        public bool editAccount(string accountNumber, string accountName)
        {
            return _adminBLL.editAccount(accountNumber, accountName);
        }

        public bool deactivateAccount(string accountNumber)
        {
            return _adminBLL.deactivateAccount(accountNumber);
        }
    }
}