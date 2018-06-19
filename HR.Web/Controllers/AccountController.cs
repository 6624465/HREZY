using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HR.Web.Helpers;

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
            try
            {

           
                using (HrDataContext dbContext = new HrDataContext())
                {
                    User userObj = dbContext.Users.Where(x => x.UserName == user.UserName && x.Password == user.Password && x.IsActive == true).FirstOrDefault();

                    if (userObj != null)
                    {
                        FormsAuthentication.SetAuthCookie(userObj.UserName, false);

                        SessionObj sessionObj = new SessionObj()
                        {
                            USERID = user.UserName,
                            BRANCHID = userObj.BranchId,
                            BRANCHNAME = dbContext.Branches.Where(x => x.BranchID == userObj.BranchId).FirstOrDefault() == null ? "" :
                             dbContext.Branches.Where(x => x.BranchID == userObj.BranchId).FirstOrDefault().BranchName,
                            ROLECODE = userObj.RoleCode,
                            EMPLOYEEID = userObj.EmployeeId,
                            ISMANAGER = dbContext.EmployeeHeaders.Where(x => x.ManagerId == userObj.EmployeeId).Count() > 0,
                            FILENAME = dbContext.EmployeeDocumentDetails
                           .Where(x => x.EmployeeId == userObj.EmployeeId && x.DocumentType == UTILITY.DOCUMENTTYPEID).FirstOrDefault() == null ? "" :
                           dbContext.EmployeeDocumentDetails
                           .Where(x => x.EmployeeId == userObj.EmployeeId).FirstOrDefault().FileName,
                            DocumentDetailID = dbContext.EmployeeDocumentDetails.Where(x => x.EmployeeId == userObj.EmployeeId && x.DocumentType == UTILITY.DOCUMENTTYPEID).FirstOrDefault() == null ? 0 : dbContext.EmployeeDocumentDetails.Where(x => x.EmployeeId == userObj.EmployeeId && x.DocumentType == UTILITY.DOCUMENTTYPEID).FirstOrDefault().DocumentDetailID,


                        };
                        if (sessionObj.ROLECODE == UTILITY.ROLE_EMPLOYEE)
                            sessionObj.FIRSTNAME = dbContext.EmployeeHeaders.Where(x => x.EmployeeId == userObj.EmployeeId)
                               .FirstOrDefault().FirstName;

                        SESSIONOBJ = sessionObj;

                        //if (ROLECODE == UTILITY.ROLE_ADMIN || ROLECODE == UTILITY.ROLE_SUPERADMIN)
                        //    return RedirectToAction("Index", "Dashboard");
                        //else if(ROLECODE == UTILITY.ROLE_EMPLOYEE)
                        //    return RedirectToAction("Index", "Dashboard");
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ViewData["message"] = "Your EmailId or Password is wrong";
                    }
                }
            }
            catch (Exception ex)
            {

                return RedirectToAction("LogOut", "Account");
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