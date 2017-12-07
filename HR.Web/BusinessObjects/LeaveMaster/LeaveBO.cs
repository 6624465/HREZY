using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class LeaveBO : BaseBO
    {
        LeaveRepository leaveRepository = null;
        public LeaveBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            leaveRepository = new LeaveRepository();
        }

        public void Add(Leave leave)
        {
            try
            {
                leaveRepository.Add(leave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(Leave leave)
        {
            try
            {
                leaveRepository.Delete(leave);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Leave GetById(int id)
        {
            try
            {
                return leaveRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Leave> GetByAll()
        {
            try
            {
                return leaveRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}