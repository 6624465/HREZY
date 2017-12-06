using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Operation
{
    public class AddressBO : BaseBO
    {
        AddressService addressServices = null;

        public AddressBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            addressServices = new AddressService();
        }

        public void Add(Address entity)
        {
            try
            {
                addressServices.Add(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public void Delete(Address entity)
        {
            try
            {
                addressServices.Delete(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public Address GetById(int id)
        {
            try
            {
                return addressServices.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<Address> GetAll()
        {
            try
            {
                return addressServices.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}