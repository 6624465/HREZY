using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeeWorkDetailService : IRepository<EmployeeWorkDetail>
    {
        public EmployeeWorkDetailService()
        {

        }

        public void Add(EmployeeWorkDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    EmployeeWorkDetail empWorkDetail = dbContext.EmployeeWorkDetails
                        .Where(x => x.WorkDetailID == entity.WorkDetailID).FirstOrDefault();
                    if (empWorkDetail == null)
                    {
                        dbContext.EmployeeWorkDetails.Add(entity);
                    }
                    else
                    {
                        empWorkDetail.WorkDetailID = entity.WorkDetailID;
                        empWorkDetail.BranchId = entity.BranchId;
                        empWorkDetail.ConfirmationDate = entity.ConfirmationDate;
                        empWorkDetail.CreatedBy = entity.CreatedBy;
                        empWorkDetail.CreatedOn = entity.CreatedOn;
                        empWorkDetail.DepartmentId = entity.DepartmentId;
                        empWorkDetail.DesignationId = entity.DesignationId;
                        //empWorkDetail.EmployeeId = entity.EmployeeId;
                        empWorkDetail.JoiningDate = entity.JoiningDate;
                        empWorkDetail.ModifiedBy = entity.ModifiedBy;
                        empWorkDetail.ModifiedOn = entity.ModifiedOn;
                        empWorkDetail.NoticePeriod = entity.NoticePeriod;
                        empWorkDetail.ProbationPeriod = entity.NoticePeriod;
                        empWorkDetail.ResignationDate = entity.ResignationDate;

                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        internal EmployeeWorkDetail GetByProperty(Func<EmployeeWorkDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                   return dbContext.EmployeeWorkDetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(EmployeeWorkDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeWorkDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeWorkDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeWorkDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeWorkDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeWorkDetails.Where(x => x.WorkDetailID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(EmployeeWorkDetail entity)
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