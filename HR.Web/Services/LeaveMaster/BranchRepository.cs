using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class BranchRepository : IRepository<Branch>
    {
        public void Add(Branch entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Branch branch = dbContext.Branches
                        .Where(x => x.BranchID == entity.BranchID).FirstOrDefault();
                    if (branch == null)
                    {
                        dbContext.Branches.Add(entity);
                    }
                    else
                    {
                        branch.BranchCode = entity.BranchCode;
                        branch.BranchName = entity.BranchName;
                        branch.RegNo = entity.RegNo;
                        branch.CompanyCode = entity.CompanyCode;
                        branch.CompanyId = entity.CompanyId;
                        branch.BranchTaxCode = entity.BranchTaxCode;
                        branch.SSFNumber = entity.SSFNumber;
                        branch.TaxIdNumber = entity.TaxIdNumber;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Branch entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Branches.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Branch> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Branches.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Branch GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Branches.Where(x => x.BranchID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}