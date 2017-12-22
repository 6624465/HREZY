using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using HR.Web.Services.Payroll;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmpSalaryStructureHeaderBO : BaseBO
    {
        EmpSalaryStructureHeaderService salaryStructureHeaderRepository = null;
        EmpSalaryStructureHeaderDetailBO empSalaryStructureHeaderDetailBO = null;
        public EmpSalaryStructureHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryStructureHeaderRepository = new EmpSalaryStructureHeaderService();
            empSalaryStructureHeaderDetailBO = new EmpSalaryStructureHeaderDetailBO(sessionObj);
        }

        public void Add(EmpSalaryStructureHeader structureHeader)
        {
            try
            {
                salaryStructureHeaderRepository.Add(structureHeader);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Delete(EmpSalaryStructureHeader structureHeader)
        {
            try
            {
                salaryStructureHeaderRepository.Delete(structureHeader);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmpSalaryStructureHeader GetById(int id)
        {
            try
            {
                return salaryStructureHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmpSalaryStructureHeader> GetAll(int id)
        {
            try
            {
                return salaryStructureHeaderRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        internal void SaveSalaryStructure(EmpSalaryStructureVm structureVm)
        {
            EmpSalaryStructureHeader empStructure = new EmpSalaryStructureHeader()
            {
                BranchId = structureVm.employeeSalaryStructure.empSalaryStructureHeader.BranchId,
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                EmployeeId = structureVm.employeeSalaryStructure.empSalaryStructureHeader.EmployeeId,
                IsActive = structureVm.employeeSalaryStructure.empSalaryStructureHeader.IsActive,
                Remarks = structureVm.employeeSalaryStructure.empSalaryStructureHeader.Remarks,
                Salary = structureVm.employeeSalaryStructure.empSalaryStructureHeader.Salary,
                StructureID = structureVm.employeeSalaryStructure.empSalaryStructureHeader.StructureID,
                TotalGross = structureVm.employeeSalaryStructure.empSalaryStructureHeader.TotalGross,
                TotalDeductions = structureVm.employeeSalaryStructure.empSalaryStructureHeader.TotalDeductions,

            };
            Add(empStructure);

            foreach (EmpSalaryStructureDetail item in structureVm.employeeSalaryStructure.structureCompanyDeductionDetail)
            {
                EmpSalaryStructureDetail detail = new EmpSalaryStructureDetail()
                {
                    Amount = item.Amount,
                    BranchId = item.BranchId,
                    Code = item.Code,
                    Computation = item.Computation,
                    ContributionRegister = item.ContributionRegister,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    EmployeeId = empStructure.EmployeeId,
                    IsActive = item.IsActive,
                    Total = item.Total,
                    PaymentType = item.PaymentType,
                };
                empSalaryStructureHeaderDetailBO.Add(detail);
            }

            foreach (EmpSalaryStructureDetail item in structureVm.employeeSalaryStructure.structureEmployeeDeductionDetail)
            {
                EmpSalaryStructureDetail detail = new EmpSalaryStructureDetail()
                {
                    Amount = item.Amount,
                    BranchId = item.BranchId,
                    Code = item.Code,
                    Computation = item.Computation,
                    ContributionRegister = item.ContributionRegister,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    EmployeeId = empStructure.EmployeeId,
                    IsActive = item.IsActive,
                    Total = item.Total,
                    PaymentType = item.PaymentType,
                };
                empSalaryStructureHeaderDetailBO.Add(detail);
            }

        }

        public EmpSalaryStructureHeader GetByProperty(int BranchId, int employeeId)
        {
            try
            {
                return salaryStructureHeaderRepository.GetByProperty(x => x.BranchId == BranchId && x.EmployeeId == employeeId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}