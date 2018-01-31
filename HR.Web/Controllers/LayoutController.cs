using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HR.Web.Helpers;

namespace HR.Web.Controllers
{
    [SessionFilter]
    [ErrorHandler]
    public class LayoutController : BaseController
    {
        // GET: Layout
        public PartialViewResult Links()
        {
            LayOutVM layoutVm = new LayOutVM()
            {
                RoleCode = ROLECODE,
                IsManager=ISMANAGER
            };
            return PartialView("_Links", layoutVm);
        }

        public class LayOutVM
        {
            public string RoleCode { get; set; }
            public bool IsManager { get; set; }
        }
    }
}