using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.BusinessObjects.LeaveMaster
{
    public class HolidayListBO : BaseBO
    {

        HolidayListRepository holidayListRepository = null;
        public HolidayListBO(SessionObj _sessionObj)
        {
            sessionObj = _sessionObj;
            holidayListRepository = new HolidayListRepository();
        }

        public void Add(HolidayList holidayList)
        {
            try
            {
                holidayListRepository.Add(holidayList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete(HolidayList holidayList)
        {
            try
            {
                holidayListRepository.Delete(holidayList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HolidayList GetById(int id)
        {
            try
            {
                return holidayListRepository.GetById(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<HolidayList> GetAll(int id)
        {
            try
            {
                return holidayListRepository.GetAll();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}