﻿using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.ViewModels
{
    public class SalaryStructureVm
    {
        public SalaryStructureHeader structureHeader { get; set; }
        public  List<SalaryStructureDetail> structureSalaryPaymentDetail { get; set; }

        public List<SalaryStructureDetail> structureEmployerContributionDetail { get; set; }

        public List<SalaryStructureDetail> structureEmployeeContributionDetail { get; set; }
    }
}