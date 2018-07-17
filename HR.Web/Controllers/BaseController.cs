using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.BusinessObjects.LookUpMaster;
using HR.Web.Helpers;
namespace HR.Web.Controllers
{

    public class SessionObj
    {
        public string USERID { get; set; }
        public int EMPLOYEEID { get; set; }
        public int BRANCHID { get; set; }
        public string ROLECODE { get; set; }
        public bool ISMANAGER { get; set; }
        public string BRANCHNAME { get; set; }

        public string FILENAME { get; set; }
        public int DocumentDetailID { get; set; }

        public string FIRSTNAME { get; set; }

        public int USERNUMBER { get; set; }

   
    }
    [ErrorHandler]

    public class BaseController : Controller
    {

        public string USERID
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).USERID;
            }
            set
            {
                Session[UTILITY.SSN_USERID] = value;
            }
        }

        public string FIRSTNAME
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).FIRSTNAME;
            }
            set
            {
                Session[UTILITY.SSN_FIRSTNAME] = value;
            }
        }
        public string FILENAME
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).FILENAME;
            }
            set
            {
                Session[UTILITY.SSN_FILENAME] = value;
            }
        }

        public int DocumentDetailID
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).DocumentDetailID;
            }
            set
            {
                Session[UTILITY.SSN_DOCUMENTDETAILID]= value;
            }
        }
        public int EMPLOYEEID
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).EMPLOYEEID;
            }
            set
            {
                Session[UTILITY.SSN_EMPLOYEEID] = value;
            }
        }

        public int BRANCHID
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).BRANCHID;
            }
            set
            {
                Session[UTILITY.SSN_BRANCHID] = value;
            }
        }

        public string ROLECODE
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).ROLECODE;
            }
            set
            {
                Session[UTILITY.CONFIG_ROLECODE] = value;
            }
        }
        public bool ISMANAGER
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).ISMANAGER;
            }
            set
            {
                Session[UTILITY.CONFIG_MANAGER] = value;
            }
        }

        public SessionObj SESSIONOBJ
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]);
            }
            set
            {
                Session[UTILITY.SSN_OBJECT] = value;
            }
        }

        public string BRANCHNAME
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).BRANCHNAME;
            }
            set
            {
                Session[UTILITY.CONFIG_MANAGER] = value;
            }
        }

        public int USERNUMBER
        {
            get
            {
                return ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).USERNUMBER;
            }
            set
            {
                Session[UTILITY.SSN_USERNUMBER] = value;
            }
        }



        public string GetRoleIcon()
        {
            var cssCls = string.Empty;
            var roleCode = ((SessionObj)System.Web.HttpContext.Current.Session[UTILITY.SSN_OBJECT]).ROLECODE;
            if (roleCode == UTILITY.ROLE_EMPLOYEE)
            {
                cssCls = "fa fa-user-o";
            }
            else if (roleCode == UTILITY.ROLE_ADMIN)
            {
                cssCls = "fa fa-users";
            }
            else if (roleCode == UTILITY.ROLE_SUPERADMIN)
            {
                cssCls = "fa fa-sitemap";
            }

            return cssCls;
        }

        
    }   

    public class LeaveMaster
    {
        public int ANNUALLEAVE(int BRANCHID)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["ANNUALLEAVE_" + BRANCHID.ToString()]);
        }

        public int MEDICALLEAVE(int BRANCHID)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["MEDICALLEAVE_" + BRANCHID.ToString()]);
        }

        public int CASUALLEAVE(int BRANCHID)
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["CASUALLEAVE_" + BRANCHID.ToString()]);
        }
    }
}