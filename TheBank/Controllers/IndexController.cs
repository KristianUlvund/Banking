using BLL;
using System.Web.Mvc;

namespace TheBank.Controllers
{
    public class IndexController : Controller
    {
        // GET: View
        public ActionResult Index()
        {
            if (Session["loggedIn"] == null)
            {
                Session["loggedIn"] = false;
            }

            return View();
        }

        public ActionResult navbar()
        {
            return View();
        }

        public ActionResult logOut()
        {
            Session.Clear();
            Session["loggedIn"] = false;
            return View("logIn");
        }

        public ActionResult Initialize_Test_Data()
        {
            TestdataBLL.initialize_test_data();
            return View("Index");
        }
    }
}