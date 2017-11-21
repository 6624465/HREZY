﻿using HR.Web.Models;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index() {
            return View();
        }

        public JsonResult GetData()
        {
            //JsonResult result = null;

            using (HrDataContext dbContext = new HrDataContext())
            {
                var list = dbContext.EmployeeHeaders
                         .Join(dbContext.EmployeePersonalDetails,
                         a => a.EmployeeId, b => b.EmployeeId,
                         (a, b) => new { A = a, B = b })
                         .Join(dbContext.EmployeeWorkDetails,
                         c => c.A.EmployeeId, d => d.EmployeeId,
                         (c, d) => new { C = c, D = d })
                         .Join(dbContext.EmployeeAddresses,
                         e => e.C.A.EmployeeId, f => f.EmployeeId,
                         (e, f) => new { E = e, F = f })
                         .Join(dbContext.Branches, x => x.E.C.A.BranchId, y => y.BranchID,
                         (x, y) => new { X = x, Y = y })
                         .ToList().AsEnumerable();
                var regionWiseEmployees = list.GroupBy(g => g.Y.BranchName).ToList()
                 .Select(x => new
                 {
                     y = x.Count(),
                     name = x.Key
                 }).OrderByDescending(o => o.y);

                var genderWiseEmployees = list.GroupBy(g => g.X.E.C.B.Gender).ToList()
                                      .Select(n => new
                                      {
                                          y = n.Count(),
                                          name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                                      });

                var indiawiseGenders = list.Where(s => s.Y.BranchName == "INDIA")
                    .GroupBy(g => g.X.E.C.B.Gender).ToList()
                     .Select(n => new
                     {
                         y = n.Count(),
                         name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                     }).OrderByDescending(n => n.name);


                var singaporewiseGenders = list.Where(s => s.Y.BranchName == "SINGAPORE")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var hongkongwiseGenders = list.Where(s => s.Y.BranchName == "HONGKONG")
                         .GroupBy(x => x.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var mayanmarwiseGenders = list.Where(s => s.Y.BranchName == "MAYANMAR")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var pakistanwiseGenders = list.Where(s => s.Y.BranchName == "PAKISTAN")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var srilankawiseGenders = list.Where(s => s.Y.BranchName == "SRILANKA")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var cambodiawiseGenders = list.Where(s => s.Y.BranchName == "CAMBODIA")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);

                var bangladeshwiseGenders = list.Where(s => s.Y.BranchName == "BANGLADESH")
                         .GroupBy(g => g.X.E.C.B.Gender).ToList()
                          .Select(n => new
                          {
                              y = n.Count(),
                              name = n.Key == 101 ? "Male" : n.Key == 102 ? "Female" : ""
                          }).OrderByDescending(n => n.name);


                return Json(
                    new
                    {
                        sucess = true,
                        regionWiseEmployees = regionWiseEmployees,
                        genderWiseEmployees = genderWiseEmployees,
                        indiawiseGenders = indiawiseGenders,
                        bangladeshwiseGenders = bangladeshwiseGenders,
                        cambodiawiseGenders = cambodiawiseGenders,
                        srilankawiseGenders = srilankawiseGenders,
                        pakistanwiseGenders = pakistanwiseGenders,
                        mayanmarwiseGenders = mayanmarwiseGenders,
                        hongkongwiseGenders = hongkongwiseGenders,
                        singaporewiseGenders = singaporewiseGenders
                    }, JsonRequestBehavior.AllowGet
                );
            }
        }
    }

    //public class DashBoard {
    //public   regionWiseEmployees
    //}
}