using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HR.Web.Helpers;
using HR.Web.BusinessObjects.Security;
using HR.Web.BusinessObjects.Operation;

namespace HR.Web.Controllers
{
    
    public class AccountController : BaseController
    {
        // GET: Account
        UserBO userBo = null;
        EmployeeHeaderBO empHeaderBO = null;
        public AccountController()
        {
            userBo = new UserBO(SESSIONOBJ);
            empHeaderBO = new EmployeeHeaderBO(SESSIONOBJ);
        }

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
                            USERNUMBER = userObj.UserId,
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
            //return Redirect("http://eportal.ezy-corp.com");
        }

        [HttpGet]
        public PartialViewResult  ChangePassword(int userid)
        {
            var userobj = userBo.GetById(userid);
            return PartialView();
        }

        [HttpPost]
        public JsonResult SaveNewPassword(string Password, string newpassword)
        {
            var currentuser = userBo.GetById(USERNUMBER);
            string success = "";
            if (currentuser.Password.ToUpper() == Password.ToUpper())
            {
                currentuser.Password = newpassword;
                userBo.Add(currentuser);
            }
            else
            {
                success = "Old Password Is Incorrect";
                
            }
            return Json(new { success });
           // return RedirectToAction("Login");
        }

        [HttpGet]
        public ActionResult ForgotPassword(string emailID)
        {


            var strbody = string.Empty;
            var subject = "Reset Password - EZY HR";
			User userobj = userBo.GetByProperty(x => x.UserName.ToUpper() == emailID.ToUpper());
			var employeeid = userobj.EmployeeId;
			if (employeeid ==-1) {

				var newPassword = UTILITY.CreateRandomPassword();
				userobj.Password = newPassword;
				strbody =
					string.Format(
					"Dear Admin <BR>" +
					"As you requested, your password for EZY-HR login has now been reset. Your new login details are as follows: <BR>" +
					"Email ID :{0} <BR>" +
					"Password : {1} <BR>" +
					"To change your password to something more memorable, after logging in go to My Profile, Change Password.<BR>" +
					"<BR>" +
					"Regards<BR>" +
					"Administrator<BR>" +
					"EZY-CORP<BR>",
					emailID,
					newPassword);
			}
			else {
				EmployeeHeader empobj = empHeaderBO.GetByProperty(x => x.EmployeeId == employeeid);
			
				var newPassword = UTILITY.CreateRandomPassword();
				empobj.Password = newPassword;
				userobj.Password = newPassword;
				strbody =
					string.Format(
					"Dear {0} {1} <BR>" +
					"As you requested, your password for EZY-HR login has now been reset. Your new login details are as follows: <BR>" +
					"Email ID :{2} <BR>" +
					"Password : {3} <BR>" +
					"To change your password to something more memorable, after logging in go to My Profile, Change Password.<BR>" +
					"<BR>" +
					"Regards<BR>" +
					"Administrator<BR>" +
					"EZY-CORP<BR>",
					empobj.FirstName,
					empobj.LastName,
					emailID,
					newPassword);
				empHeaderBO.Add(empobj);
			}

			//}
			/*
             "From:" + empleavelist.FromDate.ToShortDateString() + "to"  + empleavelist.ToDate.ToShortDateString() + "<BR>" 
                + "Reason:" + empleavelist.Reason;
             */
			userBo.Add(userobj);

            EmailGenerator emailgenerator = new EmailGenerator();
            emailgenerator.ConfigMail(emailID, true,subject,strbody);


            var success = "Your new password is sent to your login email ID : " + emailID;
            return Json(new { success }, JsonRequestBehavior.AllowGet);
        }
    }
}