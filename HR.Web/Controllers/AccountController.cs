using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HR.Web.Controllers
{
    public class AccountController : BaseController
    {
        HrDataContext dbContext = new HrDataContext();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            User userObj = dbContext.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();

            if (userObj != null) {
                FormsAuthentication.SetAuthCookie(userObj.UserName,false);
                return RedirectToAction("Index", "Home");
            }
            return View();
            
        }
    }
}