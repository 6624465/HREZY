using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HR.Web.Controllers
{
    public class LayoutController : BaseController
    {
        // GET: Layout
        public PartialViewResult Links()
        {
            LayOutVM layoutVm = new LayOutVM()
            {
                RoleCode = ROLECODE
            };
            return PartialView("_Links", layoutVm);
        }

        public class LayOutVM
        {
            public string RoleCode { get; set; }
        }
    }
}