using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeeHeaderService : IRepository<EmployeeHeader>
    {
        public EmployeeHeaderService()
        {

        }
        public void Add(EmployeeHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    EmployeeHeader empHeader = dbContext.EmployeeHeaders
                        .Where(x => x.EmployeeId == entity.EmployeeId).FirstOrDefault();
                    if (empHeader != null)
                    {
                        dbContext.EmployeeHeaders.Add(entity);
                    }
                    else
                    {
                        empHeader.BranchId = entity.BranchId;
                        empHeader.ConfirmPassword = entity.ConfirmPassword;
                        empHeader.CreatedBy = entity.CreatedBy;
                        empHeader.CreatedOn = entity.CreatedOn;
                        empHeader.EmployeeId = entity.EmployeeId;
                        empHeader.FirstName = entity.FirstName;
                        empHeader.IDNumber = entity.IDNumber;
                        empHeader.IDType = entity.IDType;
                        empHeader.IsActive = entity.IsActive;
                        empHeader.IsReportingAuthority = entity.IsReportingAuthority;
                        empHeader.LastName = entity.LastName;
                        empHeader.ManagerId = entity.ManagerId;
                        empHeader.MiddleName = entity.MiddleName;
                        empHeader.ModifiedBy = entity.ModifiedBy;
                        empHeader.ModifiedOn = entity.ModifiedOn;
                        empHeader.Nationality = entity.Nationality;
                        empHeader.Password = entity.Password;
                        empHeader.UserEmailId = entity.UserEmailId;
                        empHeader.UserId = entity.UserId;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeHeaders.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeHeaders.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EmployeeHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeHeaders.Where(x => x.EmployeeId == id).FirstOrDefault();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(EmployeeHeader entity)
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