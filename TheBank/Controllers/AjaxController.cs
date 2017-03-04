using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System;
using TheBank.Model;
using BLL;

namespace TheBank.Controllers
{
    public class AjaxController : Controller
    {
        SecurityBLL secBLL;
        TransactionBLL tranBLL = new TransactionBLL();
        AccountBLL accBLL = new AccountBLL();

        public AjaxController()
        {
            secBLL = new SecurityBLL();
        }

        public AjaxController(SecurityBLL stub)
        {
            secBLL = stub;
        }

        public string logIn(string pID, string password)
        {
            if(!Validate.validatePID(pID) && !Validate.validatePassword(password))
            {
                return "false";
            }
            string[] result =  secBLL.validateUser(pID, password);
            if (result != null)
            {
                Session["loggedIn"] = true;
                Session["userInfo"] = result;
                return "true";
            }
            else
            {
                return "false";
            }
        }

        public string jsonTransaction(string accountNumber, int inMonth, int inYear, bool isTransferred)
        {
            if(!Validate.validateAccountNmbr(accountNumber) || !Validate.validateMonthYear(inMonth, inYear))
            {
                return new JavaScriptSerializer().Serialize("error");
            }
            List<TransactionPresentation> a = tranBLL.listTransactions(accountNumber, inMonth, inYear, isTransferred);
            return new JavaScriptSerializer().Serialize(a);
        }

        public string deleteTransaction(int tID) 
        {
            if(tID >= 0)
            {
                return new JavaScriptSerializer().Serialize(tranBLL.deleteTransaction(tID));
            }
            return new JavaScriptSerializer().Serialize("error");
        }

        public string editPayment(TransactionPresentation tp)
        {
            if(Validate.validateTransaction(tp))
            {
                return new JavaScriptSerializer().Serialize(tranBLL.changeTransaction(tp));
            }
            return new JavaScriptSerializer().Serialize("error");
        }

        public bool registerTransaction(TransactionPresentation tp)
        {
            if (Validate.validateTransaction(tp) && tranBLL.registerTransaction(tp))
            {
                return true;
            }
            return false;
        }

        public string getAccounts(string pID)
        {
            if(!Validate.validatePID(pID))
            {
                return new JavaScriptSerializer().Serialize("error");
            }
            var accounts = accBLL.listAccountPrimitive(pID);
            return new JavaScriptSerializer().Serialize(accounts);
        }

        public int produceRandomNumber()
        {
            return secBLL.produceRandomNumber();
        }
    }
}