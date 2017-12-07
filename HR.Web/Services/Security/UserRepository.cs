using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Security
{
    public class UserRepository : IRepository<User>
    {
        public UserRepository()
        {

        }
        public void Add(User entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    User user = dbContext.Users.Where(x => x.UserId == entity.UserId).FirstOrDefault();
                    if (user == null)
                    {
                        dbContext.Users.Add(user);
                    }
                    else
                    {
                        user.BranchId = entity.BranchId;
                        user.CreatedBy = UTILITY.SSN_USERID;
                        user.CreatedOn = UTILITY.SINGAPORETIME;
                        user.Email = entity.Email;
                        user.EmployeeId = entity.EmployeeId;
                        user.IsActive = entity.IsActive;
                        user.MobileNumber = entity.MobileNumber;
                        user.ModifiedBy = UTILITY.SSN_USERID;
                        user.ModifiedOn = UTILITY.SINGAPORETIME;
                        user.Password = entity.Password;
                        user.RoleCode = entity.RoleCode;
                        user.UserId = entity.UserId;
                        user.UserName = entity.UserName;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(User entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.Users.Remove(entity);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public IEnumerable<User> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Users.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public User GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.Users.Where(x => x.UserId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}