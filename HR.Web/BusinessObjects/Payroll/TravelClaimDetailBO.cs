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
    public class TravelClaimDetailBO : BaseBO
    {
        TravelClaimDetailRepository TravelClaimDetailRepository = null;
        public TravelClaimDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            TravelClaimDetailRepository = new TravelClaimDetailRepository();
        }
        public void Add(TravelClaimDetail entity)
        {
            try
            {
                TravelClaimDetailRepository.Add(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public TravelClaimDetail GetById(int id)
        {
            try
            {
                return TravelClaimDetailRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TravelClaimDetail> GetAll()
        {
            try
            {
                return TravelClaimDetailRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public TravelClaimDetail GetByProperty(Func<TravelClaimDetail, bool> predicate)
        {
            try
            {
                return TravelClaimDetailRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<TravelClaimDetail> GetListByProperty(Func<TravelClaimDetail, bool> predicate)
        {
            try
            {
                return TravelClaimDetailRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
