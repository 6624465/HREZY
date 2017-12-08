using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class SalaryRuleDetailRepository : IRepository<SalaryRuleDetail>
    {
        public void Add(SalaryRuleDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    SalaryRuleDetail salaryRuleDetail = dbContext.SalaryRuleDetails
                        .Where(x => x.RuleDetailId == entity.RuleDetailId).FirstOrDefault();
                    if (salaryRuleDetail == null)
                    {
                        dbContext.SalaryRuleDetails.Add(entity);
                    }
                    else
                    {
                        salaryRuleDetail.RuleDetailId = entity.RuleDetailId;
                        salaryRuleDetail.AmountType = entity.AmountType;
                        salaryRuleDetail.ConditionBased = entity.ConditionBased;
                        salaryRuleDetail.ContributionRegister = entity.ContributionRegister;
                        salaryRuleDetail.PythonCode = entity.PythonCode;
                        salaryRuleDetail.RuleId = entity.RuleId;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryRuleDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.SalaryRuleDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<SalaryRuleDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public SalaryRuleDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleDetails.Where(x => x.RuleDetailId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}