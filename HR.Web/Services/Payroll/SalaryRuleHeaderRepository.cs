using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class SalaryRuleHeaderRepository : IRepository<SalaryRuleHeader>
    {
        public void Add(SalaryRuleHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    SalaryRuleHeader salaryRule = dbContext.SalaryRuleHeaders
                        .Where(x => x.RuleId == entity.RuleId).FirstOrDefault();
                    if (salaryRule == null)
                    {
                        dbContext.SalaryRuleHeaders.Add(salaryRule);
                    }
                    else
                    {
                        salaryRule.RuleId = entity.RuleId;
                        salaryRule.Category = entity.Category;
                        salaryRule.Code = entity.Code;
                        salaryRule.IsActive = entity.IsActive;
                        salaryRule.IsOnPayslip = entity.IsOnPayslip;
                        salaryRule.Name = entity.Name;
                        salaryRule.SequenceNo = entity.SequenceNo;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryRuleHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.SalaryRuleHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryRuleHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryRuleHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleHeaders.Where(x=>x.RuleId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}