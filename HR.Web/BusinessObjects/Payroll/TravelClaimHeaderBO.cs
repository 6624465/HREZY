using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Payroll;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Payroll
{
    public class TravelClaimHeaderBO : BaseBO
    {
        TravelClaimHeaderRepository travelClaimHeaderRepository = null;
        public TravelClaimHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            travelClaimHeaderRepository = new TravelClaimHeaderRepository();
        }
        public void Add(TravelClaimHeader entity)
        {
            try
            {
                entity.IsActive = true;
                entity.BranchId = sessionObj.BRANCHID;
                entity.CreatedBy = sessionObj.USERID;
                entity.CreatedOn = UTILITY.SINGAPORETIME;
                travelClaimHeaderRepository.Add(entity);
            }
            catch (Exception ex)
            {

            }
        }

        public TravelClaimHeader GetById(int id)
        {
            try
            {
                return travelClaimHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TravelClaimHeader> GetAll()
        {
            try
            {
                return travelClaimHeaderRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TravelClaimHeader GetByProperty(Func<TravelClaimHeader, bool> predicate)
        {
            try
            {
                return travelClaimHeaderRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TravelClaimHeader> GetListByProperty(Func<TravelClaimHeader, bool> predicate)
        {
            try
            {
                return travelClaimHeaderRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public TravelClaimHeader ApproveTravelClaim(TravelClaimHeader travelclaim)
        {
            var travelclaimobj = GetById(travelclaim.TravelClaimId);
            travelclaimobj.Status = "APPROVED";
            travelclaimobj.IsApproved = true;
            travelclaimobj.TotalAmtPaid = travelclaim.TotalAmtPaid;
            Add(travelclaimobj);
            return travelclaimobj;
        }

        public TravelClaimHeader RejectTravelClaim(TravelClaimHeader travelclaim)
        {
            var travelclaimobj = GetById(travelclaim.TravelClaimId);
            travelclaimobj.Status = "REJECTED";
            Add(travelclaimobj);
            return travelclaimobj;
        }
        public TravelClaimHeader SubmitTravelClaim(TravelClaimHeader travelclaim)
        {
            var travelobj = GetById(travelclaim.TravelClaimId);
            travelobj.Status = "SUBMITTED";
            Add(travelobj);
            return travelobj;
        }
        internal int GetCount(int branchId)
        {
            return travelClaimHeaderRepository.GetCount(branchId);
        }
        public void Delete(int id)
        {
            try
            {
                 travelClaimHeaderRepository.Delete(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public TravelClaimHeader ApproveTravelClaimSave(TravelClaimHeader travelclaim)
        {
            var travelclaimobj = GetById(travelclaim.TravelClaimId);
            travelclaimobj.Status = "PAID";
            travelclaimobj.IsApproved = true;
            travelclaimobj.TotalAmtPaid = travelclaim.TotalAmtPaid;
            Add(travelclaimobj);
            return travelclaimobj;
        }
    }
}
