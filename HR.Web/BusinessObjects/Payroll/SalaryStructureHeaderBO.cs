using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class SalaryStructureHeaderBO : BaseBO
    {
        SalaryStructureHeaderRepository salaryStructureHeaderRepository = null;
        SalaryStructureDetailBO salaryStructureDetailBO = null;
        public SalaryStructureHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            salaryStructureHeaderRepository = new SalaryStructureHeaderRepository();
            salaryStructureDetailBO = new SalaryStructureDetailBO(_sessionObj);
        }


        public void Add(SalaryStructureHeader input)
        {
            try
            {
                input.CreatedBy = sessionObj.USERID;
                input.CreatedOn = UTILITY.SINGAPORETIME;
                salaryStructureHeaderRepository.Add(input);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryStructureHeader entity)
        {
            try
            {
                salaryStructureHeaderRepository.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryStructureHeader GetById(int id)
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

        public IEnumerable<SalaryStructureHeader> GetAll()
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
        public SalaryStructureHeader GetByProperty(Func<SalaryStructureHeader, bool> predicate)
        {
            try
            {
                return salaryStructureHeaderRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryStructureHeader> GetListByProperty(Func<SalaryStructureHeader, bool> predicate)
        {
            try
            {
                return salaryStructureHeaderRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void SaveSalaryStructure(SalaryStructureVm salaryStructureVm)
        {
            try
            {
                SalaryStructureHeader structureHeader = new SalaryStructureHeader()
                {
                    StructureID = salaryStructureVm.structureHeader.StructureID,
                    Code = salaryStructureVm.structureHeader.Code,
                    EffectiveDate = salaryStructureVm.structureHeader.EffectiveDate,
                    IsActive = true,
                    Remarks = salaryStructureVm.structureHeader.Remarks,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    NetAmount = salaryStructureVm.structureHeader.NetAmount,
                    TotalGross = salaryStructureVm.structureHeader.TotalGross,
                    TotalDeductions = salaryStructureVm.structureHeader.TotalDeductions,
                    BranchId= salaryStructureVm.structureHeader.BranchId,
                    Description= salaryStructureVm.structureHeader.Description,
                    EmpContribution = salaryStructureVm.structureHeader.EmpContribution,
                    EmployeeId = salaryStructureVm.structureHeader.EmployeeId,
                };
                Add(structureHeader);
                //salaryStructureVm.structureDetail = salaryStructureVm.structureDetail.Where(x => x.IsActive == true).ToList();
                if (salaryStructureVm.structureSalaryPaymentDetail != null)
                {
                    foreach (SalaryStructureDetail item in salaryStructureVm.structureSalaryPaymentDetail)
                    {
                        SalaryStructureDetail detail = new SalaryStructureDetail()
                        {
                            Amount = item.Amount,
                            Code = item.Code,
                            ComputationCode = item.ComputationCode,
                            CreatedBy = sessionObj.USERID,
                            CreatedOn = UTILITY.SINGAPORETIME,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            RegisterCode = item.RegisterCode,
                            StructureID = structureHeader.StructureID,
                            StructureDetailID = item.StructureDetailID,
                            Total = item.Total,
                            PaymentType = item.PaymentType,
                            BranchId = structureHeader.BranchId,
                            IsVariablePay = item.IsVariablePay,
                            IsSatuatoryPay = item.IsSatuatoryPay
                        };
                        if (item.StructureDetailID > 0)
                            salaryStructureDetailBO.Delete(detail);
                        if (item.IsActive == true)
                        {
                            salaryStructureDetailBO.Add(detail);
                        }
                        else
                        {
                            detail.Amount = 0;
                            salaryStructureDetailBO.Add(detail);
                        }
                            
                    }

                }
               

                if (salaryStructureVm.structureEmployerContributionDetail != null)
                {
                    foreach (SalaryStructureDetail item in salaryStructureVm.structureEmployerContributionDetail)
                    {
                        SalaryStructureDetail detail = new SalaryStructureDetail()
                        {
                            Amount = item.Amount,
                            Code = item.Code,
                            ComputationCode = item.ComputationCode,
                            CreatedBy = sessionObj.USERID,
                            CreatedOn = UTILITY.SINGAPORETIME,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            RegisterCode = item.RegisterCode,
                            StructureID = structureHeader.StructureID,
                            StructureDetailID = item.StructureDetailID,
                            Total = item.Total,
                            PaymentType = item.PaymentType,
                            BranchId = structureHeader.BranchId,
                            IsVariablePay = item.IsVariablePay,
                            IsSatuatoryPay = item.IsSatuatoryPay
                        };
                        if (item.StructureDetailID > 0)
                            salaryStructureDetailBO.Delete(detail);
                        if (item.IsActive == true)
                        {
                            salaryStructureDetailBO.Add(detail);
                        }
                        else
                        {
                            detail.Amount = 0;
                            salaryStructureDetailBO.Add(detail);
                        }
                    }

                }

                if (salaryStructureVm.structureEmployeeContributionDetail != null)
                {
                    foreach (SalaryStructureDetail item in salaryStructureVm.structureEmployeeContributionDetail)
                    {
                        SalaryStructureDetail detail = new SalaryStructureDetail()
                        {
                            Amount = item.Amount,
                            Code = item.Code,
                            ComputationCode = item.ComputationCode,
                            CreatedBy = sessionObj.USERID,
                            CreatedOn = UTILITY.SINGAPORETIME,
                            Description = item.Description,
                            IsActive = item.IsActive,
                            RegisterCode = item.RegisterCode,
                            StructureID = structureHeader.StructureID,
                            StructureDetailID = item.StructureDetailID,
                            Total = item.Total,
                            PaymentType = item.PaymentType,
                            BranchId = structureHeader.BranchId,
                            IsVariablePay = item.IsVariablePay,
                            IsSatuatoryPay = item.IsSatuatoryPay
                        };
                        if (item.StructureDetailID > 0)
                            salaryStructureDetailBO.Delete(detail);
                        if (item.IsActive == true)
                        {
                            salaryStructureDetailBO.Add(detail);
                        }
                        else
                        {
                            detail.Amount = 0;
                            salaryStructureDetailBO.Add(detail);
                        }
                    }
                }

            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal void DeleteById(int structurId)
        {
            salaryStructureHeaderRepository.DeleteById(structurId);
        }

        internal int GetCount(int branchId)
        {
           return salaryStructureHeaderRepository.GetCount(branchId);
        }
    }
}
