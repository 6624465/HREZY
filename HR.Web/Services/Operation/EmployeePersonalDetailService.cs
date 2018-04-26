using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmployeePersonalDetailService : IRepository<EmployeePersonalDetail>
    {
        public EmployeePersonalDetailService()
        {

        }
        public void Add(EmployeePersonalDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    EmployeePersonalDetail empPersonalDetail = dbContext.EmployeePersonalDetails
                       .Where(x => x.PersonalDetailID == entity.PersonalDetailID).FirstOrDefault();
                    if (empPersonalDetail == null)
                    {
                        dbContext.EmployeePersonalDetails.Add(entity);
                    }
                    else
                    {
                        empPersonalDetail.BirthCountry = entity.BirthCountry;
                        empPersonalDetail.BranchId = entity.BranchId;
                        empPersonalDetail.CitizenshipCountry = entity.CitizenshipCountry;
                        empPersonalDetail.CreatedBy = entity.CreatedBy;
                        empPersonalDetail.CreatedOn = entity.CreatedOn;
                        empPersonalDetail.DOB = entity.DOB;
                        empPersonalDetail.EmergencyContactName = entity.EmergencyContactName;
                        empPersonalDetail.EmergencyContactNumber = entity.EmergencyContactNumber;
                        // empPersonalDetail.EmployeeId = entity.EmployeeId;
                        empPersonalDetail.FatherName = entity.FatherName;
                        empPersonalDetail.Gender = entity.Gender;
                        empPersonalDetail.MaritalStatus = entity.MaritalStatus;
                        empPersonalDetail.MarriageDate = entity.MarriageDate;
                        empPersonalDetail.ModifiedBy = entity.ModifiedBy;
                        empPersonalDetail.ModifiedOn = entity.ModifiedOn;
                        empPersonalDetail.PersonalDetailID = entity.PersonalDetailID;
                        empPersonalDetail.ResidentialStatus = entity.ResidentialStatus;
                        empPersonalDetail.SpouseName = entity.SpouseName;
                        empPersonalDetail.EPFNO = entity.EPFNO;
                        empPersonalDetail.PasspostNo = entity.PasspostNo;
                        empPersonalDetail.SocialWelfareNo = entity.SocialWelfareNo;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(EmployeePersonalDetail entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.EmployeePersonalDetails.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeePersonalDetail> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeePersonalDetails.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public EmployeePersonalDetail GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeePersonalDetails.Where(x => x.PersonalDetailID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(EmployeePersonalDetail entity)
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

        public EmployeePersonalDetail GetByProperty(Func<EmployeePersonalDetail, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.EmployeePersonalDetails.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}