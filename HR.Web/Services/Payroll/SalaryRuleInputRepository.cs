using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Payroll
{
    public class SalaryRuleInputRepository : IRepository<SalaryRuleInput>
    {
        public void Add(SalaryRuleInput entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    SalaryRuleInput salaryRuleInput = dbContext.SalaryRuleInputs
                        .Where(x => x.RuleInputId == entity.RuleInputId).FirstOrDefault();
                    if (salaryRuleInput == null)
                    {
                        dbContext.SalaryRuleInputs.Add(salaryRuleInput);
                    }
                    else
                    {
                        salaryRuleInput.RuleInputId = salaryRuleInput.RuleInputId;
                        salaryRuleInput.Category = salaryRuleInput.Category;
                        salaryRuleInput.Code = salaryRuleInput.Code;
                        salaryRuleInput.Name = salaryRuleInput.Name;
                        salaryRuleInput.RuleId = salaryRuleInput.RuleId;
                        salaryRuleInput.ContributionRegister = salaryRuleInput.ContributionRegister;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(SalaryRuleInput entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.SalaryRuleInputs.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<SalaryRuleInput> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleInputs.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public SalaryRuleInput GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.SalaryRuleInputs.Where(x=>x.RuleInputId== id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}