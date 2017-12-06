using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects
{
    public class WeekendPolicyBO : BaseBO
    {
        WeekendPolicyService weekenPolicyService = null;
        public WeekendPolicyBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            weekenPolicyService = new WeekendPolicyService();
        }

        public void Add(WeekendPolicy entity)
        {
            try
            {
                weekenPolicyService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Update(WeekendPolicy entity)
        {
            try
            {
                weekenPolicyService.Update(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(WeekendPolicy entity)
        {
            try
            {
                weekenPolicyService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public WeekendPolicy GetById(int id)
        {
            try
            {
                return weekenPolicyService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<WeekendPolicy> GetAll()
        {
            try
            {
                return weekenPolicyService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}