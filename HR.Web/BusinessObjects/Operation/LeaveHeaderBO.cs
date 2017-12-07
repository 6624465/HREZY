using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class LeaveHeaderBO : BaseBO
    {
        LeaveHeaderRepository leaveHeaderRepository = null;
        public LeaveHeaderBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            leaveHeaderRepository = new LeaveHeaderRepository();
        }

        public void Add(LeaveHeader leaveHeader)
        {
            try
            {
                leaveHeaderRepository.Add(leaveHeader);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LeaveHeader leaveHeader)
        {
            try
            {
                leaveHeaderRepository.Delete(leaveHeader);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public LeaveHeader GetById(int id)
        {
            try
            {
                return leaveHeaderRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IEnumerable<LeaveHeader> GetAll()
        {
            try
            {
                return leaveHeaderRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}