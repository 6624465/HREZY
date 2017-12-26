using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.LookUpMaster
{
    public class LookUpBO : BaseBO
    {
        LookUpRepository lookUpRepository = null;
        public LookUpBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            lookUpRepository = new LookUpRepository();
        }

        public void Add(LookUp lookUp)
        {
            try
            {
                lookUp.CreatedBy = sessionObj.USERID;
                lookUp.CreatedOn = UTILITY.SINGAPORETIME;
                lookUp.ModifiedBy = sessionObj.USERID;
                lookUp.ModifiedOn = UTILITY.SINGAPORETIME;

                lookUpRepository.Add(lookUp);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

       
        public void Delete(int id)
        {
            try
            {
                lookUpRepository.Delete(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public LookUp GetById(int id)
        {
            try
            {
                return lookUpRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<LookUp> GetByAll()
        {
            try
            {
                return lookUpRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public LookUp GetByProperty(Func<LookUp,bool> predicate)
        {
            try
            {
                return lookUpRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<LookUp> GetListByProperty(Func<LookUp, bool> predicate)
        {
            try
            {
                return lookUpRepository.GetListByProperty(predicate).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}