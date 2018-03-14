using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeeDocumentDetailService : IRepository<EmployeeDocumentDetail>
    {
        public EmployeeDocumentDetailService()
        {

        }
        public void Add(EmployeeDocumentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    EmployeeDocumentDetail empDetail = dbContext.EmployeeDocumentDetails
                        .Where(x => x.EmployeeId == entity.EmployeeId && x.DocumentType==entity.DocumentType)
                        .FirstOrDefault();
                    if (empDetail == null)
                    {
                        dbContext.EmployeeDocumentDetails.Add(entity);
                    }
                    else
                    {
                        empDetail.BranchId = entity.BranchId;
                        
                        //empDetail.DocumentDetailID = entity.DocumentDetailID;
                        empDetail.DocumentType = entity.DocumentType;
                        empDetail.FileName = entity.FileName;
                        empDetail.EmployeeId = entity.EmployeeId;
                        empDetail.ModifiedBy = entity.CreatedBy;
                        empDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void AddClaimDocuments(EmployeeDocumentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    //EmployeeDocumentDetail empDetail = dbContext.EmployeeDocumentDetails
                    //    .Where(x => x.EmployeeId == entity.EmployeeId && x.DocumentType == entity.DocumentType)
                    //    .FirstOrDefault();
                    if (true)
                    {
                        dbContext.EmployeeDocumentDetails.Add(entity);
                    }
                    else
                    {
                        //empDetail.BranchId = entity.BranchId;

                        ////empDetail.DocumentDetailID = entity.DocumentDetailID;
                        //empDetail.DocumentType = entity.DocumentType;
                        //empDetail.FileName = entity.FileName;
                        //empDetail.EmployeeId = entity.EmployeeId;
                        //empDetail.ModifiedBy = entity.CreatedBy;
                        //empDetail.ModifiedOn = UTILITY.SINGAPORETIME;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(EmployeeDocumentDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeeDocumentDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<EmployeeDocumentDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeDocumentDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeeDocumentDetail GetById(int id)
        {
            using (HrDataContext dbContext = new HrDataContext())
            {
                return dbContext.EmployeeDocumentDetails.Where(x => x.DocumentDetailID == id).FirstOrDefault();
            }
        }

        public IEnumerable<EmployeeDocumentDetail> GetListByProperty(Func<EmployeeDocumentDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeeDocumentDetails.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}