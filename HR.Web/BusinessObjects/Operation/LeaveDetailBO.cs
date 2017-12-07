using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class LeaveDetailBO : BaseBO
    {
        LeaveDetailRepository leaveDetailRepository = null;
        public LeaveDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            leaveDetailRepository = new LeaveDetailRepository();
        }

        public void Add(LeaveDetail leaveDetail)
        {
            try
            {
                leaveDetailRepository.Add(leaveDetail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(LeaveDetail leaveDetail)
        {
            try
            {
                leaveDetailRepository.Delete(leaveDetail);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LeaveDetail GetById(int id)
        {
            try
            {
                return leaveDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<LeaveDetail> GetAll(int id)
        {
            try
            {
                return leaveDetailRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}