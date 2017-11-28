using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class HtmlTblVm<T>
    {
        public List<T> TableData { get; set; }

        public decimal PageLength { get; set; }

        public int TotalRows { get; set; }

        public int CurrentPage { get; set; }
    }
}