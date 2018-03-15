using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class EmployeeDocumentDetailBO :BaseBO
    {

        EmployeeDocumentDetailService empDocDetailService = null;
        public EmployeeDocumentDetailBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            empDocDetailService = new EmployeeDocumentDetailService();
        }



        public void Add(EmployeeDocumentDetail entity)
        {
            try
            {
                empDocDetailService.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void AddClaimDocuments(EmployeeDocumentDetail entity)
        {
            try
            {
                empDocDetailService.AddClaimDocuments(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        

        public void Delete(EmployeeDocumentDetail entity)
        {
            try
            {
                empDocDetailService.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public EmployeeDocumentDetail GetById(int id)
        {
            try
            {
                return empDocDetailService.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<EmployeeDocumentDetail> GetAll()
        {
            try
            {
                return empDocDetailService.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public IEnumerable<EmployeeDocumentDetail> GetListByProperty(Func<EmployeeDocumentDetail, bool> predicate)
        {
            try
            {

                return empDocDetailService.GetListByProperty(predicate);
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
                 empDocDetailService.Delete(id);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

       
    }
}