using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class RoleRightsController : Controller
    {
        // GET: RoleRights
        public ActionResult AddRole()
        {
            return View();
        }

        public PartialViewResult NewRole(int roleId = -1)
        {
            if (roleId != -1)
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Role role = dbContext.Roles.Where(x => x.RoleId == roleId).FirstOrDefault();
                    return PartialView("_NewRole", role);
                }
            }
            else
                return PartialView("_NewRole", new Role { RoleId = -1 });
        }
        [HttpPost]
        public ActionResult AddRole(Role role)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                role.ModifiedBy = "Admin";
                role.ModifiedOn = DateTime.Now;
                role.Createdby = "Admin";
                role.CreatedOn = DateTime.Now;
                role.IsActive = true;
                dbContext.Roles.Add(role);
                dbContext.SaveChanges();
            }
            return RedirectToAction("AddRole");
        }
    }
}