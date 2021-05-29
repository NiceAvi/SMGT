using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class LoginController : Controller
    {
        private smsEntities db = new smsEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user)
        {
            if (user != null)
            {
                using (smsEntities db = new smsEntities())
                {
                    var UserObj = db.Users.Where(u => u.email.Equals(user.email) && u.password.Equals(user.password)).FirstOrDefault();
                    if (UserObj != null)
                    {
                        Session["UserId"] = UserObj.Id.ToString();
                        Session["Email"] = UserObj.email.ToString();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "The UserName or Password Incorrect.");
                    }
                }
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Login"); 
        }


    }
}