using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using HR.Web.Services.Payroll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmpSalaryStructureHeaderDetailBO : BaseBO
    {
        EmpSalaryStructureDetailService empSalaryStructureDetailService = null;
        public EmpSalaryStructureHeaderDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            empSalaryStructureDetailService = new EmpSalaryStructureDetailService();

        }

        public void Add(EmpSalaryStructureDetail structureDetail)
        {
            try
            {
                empSalaryStructureDetailService.Add(structureDetail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Delete(EmpSalaryStructureDetail structureDetail)
        {
            try
            {
                empSalaryStructureDetailService.Delete(structureDetail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmpSalaryStructureDetail GetById(int id)
        {
            try
            {
                return empSalaryStructureDetailService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmpSalaryStructureDetail> GetAll(int id)
        {
            try
            {
                return empSalaryStructureDetailService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }




        public EmpSalaryStructureDetail GetByProperty(int BranchId, int employeeId)
        {
            try
            {
                return empSalaryStructureDetailService.GetByProperty(x => x.BranchId == BranchId && x.EmployeeId == employeeId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}