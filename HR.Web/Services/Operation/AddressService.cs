using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class AddressService : IRepository<Address>
    {
        public AddressService()
        {

        }
        public void Add(Address entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    Address address = dbContext.Addresses
                        .Where(x => x.LinkID == entity.LinkID).FirstOrDefault();
                    if (address == null)
                    {
                        dbContext.Addresses.Add(entity);
                    }
                    else
                    {
                        //address.AddressId = entity.AddressId;
                        address.LinkID = entity.LinkID;
                        address.BranchId = entity.BranchId;
                        address.SeqNo = entity.SeqNo;
                        address.AddressType = entity.AddressType;
                        address.Address1 = entity.Address1;
                        address.Address2 = entity.Address2;
                        address.Address3 = entity.Address3;
                        address.Address4 = entity.Address4;
                        address.CityName = entity.CityName;
                        address.TelNo = entity.TelNo;
                        address.FaxNo = entity.FaxNo;
                        address.Contact = entity.Contact;
                        address.CityName = entity.CityName;
                        address.StateName = entity.StateName;
                        address.Email = entity.Email;
                        address.WebSite = entity.WebSite;
                        address.IsActive = entity.IsActive;
                        address.CreatedBy = entity.CreatedBy;
                        address.CreatedOn = entity.CreatedOn;
                        address.ModifiedBy = entity.ModifiedBy;
                        address.ModifiedOn = entity.ModifiedOn;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            };
        }

        internal Address GetByProperty(Func<Address, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                 return   dbContext.Addresses.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            };
        }

        public void Delete(Address entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Addresses.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            };
        }

        public IEnumerable<Address> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Addresses.ToList();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Addresses.Where(x => x.AddressId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}