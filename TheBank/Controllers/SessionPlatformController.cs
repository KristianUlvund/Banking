using BLL;
using System.Web.Mvc;
using TheBank.Model;

namespace TheBank.Controllers
{
    public class SessionPlatformController : Controller
    {
        AccountBLL accBLL = new AccountBLL();
        TransactionBLL tranBLL = new TransactionBLL();

        // GET: Overview
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadPage(string page)
        {
            if(!(bool)Session["loggedIn"])
            {
                return View("~/Views/Index/logIn.cshtml");
            }

            string[] userInfo = (string[])Session["userInfo"];
            ViewBag.UserInfo = userInfo;

            switch (page)
            {
                case "UserOverview":
                    return View(page, accBLL.listAccountPrimitive(userInfo[0]));
                case "ViewPayments":
                    return View(page, accBLL.listAccountPrimitive(userInfo[0]));
                case "AdminOverview":
                    return View("~/Views/Admin/AdminOverview.cshtml");
                default:
                    return View(page);
            }
        }

        public ActionResult ViewAccount(string accountNumber)
        {
            if (!Validate.validateAccountNmbr(accountNumber))
            {
                return null;
            }
            string[] userInfo = (string[])Session["userInfo"];
            Session["AccountInfo"] = accBLL.getAccountData(accountNumber, userInfo[0]);
            return View();
        }

        public ActionResult RegisterPayment(TransactionPresentation tp)
        {
            if(Validate.validateTransaction(tp) && tranBLL.registerTransaction(tp))
            {
                return View("Overview");
            }
            return null;

        }

    }
}