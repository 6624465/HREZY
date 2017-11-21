﻿using HR.Web.Models;
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

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                User userObj = dbContext.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefault();

                if (userObj != null)
                {
                    FormsAuthentication.SetAuthCookie(userObj.UserName, false);
                    USERID = user.UserName;
                    BRANCHID = userObj.BranchId;
                    ROLECODE = userObj.RoleCode;
                    EMPLOYEEID = 1000;
                    return RedirectToAction("Index", "Home");
                }
            }
            return View();

        }

        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}