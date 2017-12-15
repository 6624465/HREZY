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
                    IsActive = salaryStructureVm.structureHeader.IsActive,
                    Remarks = salaryStructureVm.structureHeader.Remarks,
                    CreatedBy = sessionObj.USERID,
                    CreatedOn = UTILITY.SINGAPORETIME,
                    NetAmount= salaryStructureVm.structureHeader.NetAmount
                };
                Add(structureHeader);
                salaryStructureVm.structureDetail = salaryStructureVm.structureDetail.Where(x => x.IsActive == true).ToList();
                foreach (SalaryStructureDetail item in salaryStructureVm.structureDetail)
                {
                    SalaryStructureDetail detail = new SalaryStructureDetail() {
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
                        Total = item.Total
                    };
                    salaryStructureDetailBO.Add(detail);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
