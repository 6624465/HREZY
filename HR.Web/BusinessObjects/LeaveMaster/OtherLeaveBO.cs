﻿using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using HR.Web.Services.OtherLeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class OtherLeaveBO : BaseBO
    {
        OtherLeaveRepository OtherLeaveRepository = null;

     
        public OtherLeaveBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            OtherLeaveRepository = new OtherLeaveRepository();
        }

        public void Add(OtherLeave OtherLeave)
        {
            try
            {

                OtherLeaveRepository.Add(OtherLeave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void AddList(List<OtherLeave> OtherLeaves)
        {
            try
            {
                foreach (OtherLeave item in OtherLeaves) { 
                    OtherLeaveRepository.Add(item);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(OtherLeave OtherLeave)
        {
            try
            {
                OtherLeaveRepository.Delete(OtherLeave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<OtherLeave> GetListById(int id)
        {
            try
            {
                return OtherLeaveRepository.GetlistById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OtherLeave GetById(int id)
        {
            try
            {
                return OtherLeaveRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<OtherLeave> GetByAll()
        {
            try
            {
                return OtherLeaveRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public void AddLookUp(LookUp lookup)
        {
            try
            {

                OtherLeave Leave = new OtherLeave
                {
                    LeaveTypeId = lookup.LookUpID,
                    Description = lookup.LookUpCode,
                    IsCarryForward = lookup.IsCarryForward,
                    IsActive = lookup.IsActive,
                    BranchId=sessionObj.BRANCHID
                };
                OtherLeaveRepository.AddFromLookUp(Leave);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public void DeleteLookUp(LookUp lookup)
        {
            try
            {
                OtherLeave leave = new OtherLeave
                {
                    LeaveTypeId = lookup.LookUpID,
                    Description = lookup.LookUpCode,
                    IsCarryForward = lookup.IsCarryForward,
                    BranchId = sessionObj.BRANCHID
                };
                OtherLeaveRepository.DeleteFromLookUp(leave);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<OtherLeave> GetListByProperty(Func<OtherLeave, bool> predicate)
        {
            try
            {
                return OtherLeaveRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}