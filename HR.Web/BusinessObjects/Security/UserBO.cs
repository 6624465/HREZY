using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.Security;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.Security
{
    public class UserBO : BaseBO
    {
        UserRepository userRepository = null;

        public int BRANCHID { get; private set; }

        public UserBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            userRepository = new UserRepository();
        }

        public void Add(User user)
        {
            try
            {
                userRepository.Add(user);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(User user)
        {
            try
            {
                userRepository.Delete(user);
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
                return userRepository.GetById(id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<User> GetAll(int id)
        {
            try
            {
                return userRepository.GetAll();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        internal void AddUserVm(EmployeeVm empVm)
        {
            User user = new User()
            {
                BranchId = sessionObj.BRANCHID,
                CreatedBy = sessionObj.USERID,
                CreatedOn = UTILITY.SINGAPORETIME,
                ModifiedBy = sessionObj.USERID,
                ModifiedOn = UTILITY.SINGAPORETIME,
                Email = empVm.empHeader.UserEmailId,
                EmployeeId = empVm.empHeader.EmployeeId,
                IsActive = true,
                MobileNumber = empVm.address.MobileNo,
                UserName = empVm.empHeader.UserEmailId,
                Password = empVm.empHeader.Password,
                RoleCode = "Employee"
            };
            Add(user);
        }
    }
}