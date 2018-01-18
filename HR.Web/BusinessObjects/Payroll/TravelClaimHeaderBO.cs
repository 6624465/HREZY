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

    }
}
