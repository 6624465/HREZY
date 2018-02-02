using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{

    public class EmployeePersonalDetailBO : BaseBO
    {
        EmployeePersonalDetailService empPersonalDetailService = null;
        public EmployeePersonalDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            empPersonalDetailService = new EmployeePersonalDetailService();
        }

        public void MapPersonalDetail(EmployeePersonalDetail empPerDetail)
        {
            empPerDetail = new EmployeePersonalDetail
            {
                EmployeeId = empPerDetail.EmployeeId,
                BranchId = sessionObj.BRANCHID,
                DOB = empPerDetail.DOB,
                Gender = empPerDetail.Gender, //
                BirthCountry = empPerDetail.BirthCountry, //
                CitizenshipCountry = "", //
                FatherName = empPerDetail.FatherName,
                MaritalStatus = empPerDetail.MaritalStatus,
                SpouseName = empPerDetail.SpouseName, //
                EmergencyContactName = empPerDetail.EmergencyContactName,
                EmergencyContactNumber = empPerDetail.EmergencyContactNumber,
                MarriageDate = empPerDetail.MarriageDate, //
                ResidentialStatus = empPerDetail.ResidentialStatus, //
                EPFNO = empPerDetail.EPFNO,
                PasspostNo = empPerDetail.PasspostNo,                
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                ModifiedBy = sessionObj.USERID,
                ModifiedOn = UTILITY.SINGAPORETIME
            };

        }

        public void Add(EmployeePersonalDetail entity)
        {
            try
            {
                empPersonalDetailService.Add(entity);
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
                empPersonalDetailService.Delete(entity);
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
                return empPersonalDetailService.GetById(id);
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
                return empPersonalDetailService.GetAll();
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
                return empPersonalDetailService.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}