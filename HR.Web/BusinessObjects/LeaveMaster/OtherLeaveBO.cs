using HR.Web.Controllers;
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
        public void AddLookUP(LookUp lookup)
        {
            try
            {
                OtherLeaveRepository.AddLookUP(lookup);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}