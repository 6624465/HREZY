using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HR.Web.Models;

namespace HR.Web.Services.Operation
{
    public class EmployeeBankDetailService : IRepository<EmployeeBankdetail>
    {
        public EmployeeBankDetailService()
        {

        }
        public void Add(EmployeeBankdetail entity)
        {
            try
            {
                using(var dbcntx=new HrDataContext())
                {
                    EmployeeBankdetail employeebankdetail = dbcntx.EmployeeBankdetails.
                                                            Where(x => x.BankDetailId == entity.BankDetailId).FirstOrDefault();
                    if (employeebankdetail == null)
                    {
                        dbcntx.EmployeeBankdetails.Add(entity);
                    }
                    else
                    {
                        employeebankdetail.EmployeeId = entity.EmployeeId;
                        employeebankdetail.BranchId = entity.BranchId;
                        employeebankdetail.BankName = entity.BankName;
                        employeebankdetail.AccountNo = entity.AccountNo;
                        employeebankdetail.AccountType = entity.AccountType;
                        employeebankdetail.BankBranchCode = entity.BankBranchCode;
                        employeebankdetail.SwiftCode = entity.SwiftCode;
                    }
                    dbcntx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(EmployeeBankdetail entity)
        {
            try
            {
                using (var dbcntx = new HrDataContext())
                {
                    dbcntx.EmployeeBankdetails.Remove(entity);
                    dbcntx.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public IEnumerable<EmployeeBankdetail> GetAll()
        {
            try
            {
                using(var dbcntx=new HrDataContext())
                {
                   return dbcntx.EmployeeBankdetails.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeBankdetail GetById(int id)
        {
            try
            {
                using(var dbcntx=new HrDataContext())
                {
                    return dbcntx.EmployeeBankdetails.Where(x => x.EmployeeId == id).FirstOrDefault();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public EmployeeBankdetail GetByProperty(Func<EmployeeBankdetail, bool> predicate)
        {

            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeBankdetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void Update(EmployeeBankdetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}