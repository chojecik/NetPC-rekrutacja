using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ContactsApp.Models;

namespace ContactsApp.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseEntities db = new DatabaseEntities();
        public static string LoggedEmail;       //przechowywanie informacji o aktualnie zalogowanym użytkowniku. Pole statyczne, aby było wszędzie widoczne

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(UserModel user)
        {
            bool isLoggingSuccess = false;

            foreach (var item in db.Contact)        //z pewnoscia nie jest to bezpieczna metoda logowania jednak na dana chwile potrafie to zrobic jedynie w ten sposob
            {
                if (item.Email == user.Email)       //jesli Email zgodny to sprawdz haslo
                {
                    if (item.Password == user.Password)     //jesli haslo zgodne to uzytkownik zalogowany
                    {
                        HomeController.LoggedEmail = item.Email;
                        isLoggingSuccess = true;
                        break;
                    }
                }
            }

            if (isLoggingSuccess)
            {
                ViewBag.status = "success";
                return View("LoggedIn");
            }
            else
            {
                ViewBag.status = "error";
                return View();
            }

        }

        public ActionResult Logout()
        {
            HomeController.LoggedEmail = null;
            
            return View("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}