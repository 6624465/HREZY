﻿using HR.Web.Controllers;
using HR.Web.Models;
using HR.Web.Services.LeaveMaster;
using HR.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        internal List<calendarVM> GetHolidayList()
        {
            var obj = new List<HolidayList>();
            List<calendarVM> holidayList = new List<calendarVM>();
            if (sessionObj.ROLECODE == UTILITY.ROLE_SUPERADMIN)
            {
                obj = GetAll().ToList();

            }
            else
            {
                obj = GetAll().Where(x => x.BranchID == sessionObj.BRANCHID).ToList();
            }
            foreach (HolidayList item in obj)
            {
                calendarVM list = new calendarVM();
                list.title = item.Description;
                list.date = item.Date;

                var strHref = "";
                if (sessionObj.ROLECODE == UTILITY.ROLE_EMPLOYEE)
                {
                    strHref = "#";
                }
                else
                    strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

                var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                string hrefUrl = UrlHelper.GenerateContentUrl(strHref, context);
                list.url = hrefUrl;
                holidayList.Add(list);
            }

            return holidayList;
        }

        internal List<calendarVM> GetHolidayListByBranch(int branchId)
        {
            var obj = new List<HolidayList>();
            List<calendarVM> holidayList = new List<calendarVM>();

            obj = GetAll().Where(x => x.BranchID == branchId).ToList();

            foreach (HolidayList item in obj)
            {

                calendarVM list = new calendarVM();
                list.title = item.Description.Replace("'","");
                list.date = item.Date;

                var strHref = "";
                if (sessionObj.ROLECODE == UTILITY.ROLE_EMPLOYEE)
                {
                    strHref = "#";
                }
                else
                    strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

                var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                string hrefUrl = UrlHelper.GenerateContentUrl(strHref, context);
                list.url = hrefUrl;
                holidayList.Add(list);
            }

            return holidayList;
        }


        internal holidayVm SaveHolidayList(int HolidayId)
        {
            var obj = GetAll();

            if (sessionObj.ROLECODE == UTILITY.ROLE_SUPERADMIN)
            {
                obj = GetAll().ToList();

            }
            else
            {
                obj = GetAll().Where(x => x.BranchID == sessionObj.BRANCHID).ToList();
            }
            holidayVm holidayVm = new holidayVm();
            List<calendarVM> holidayList = new List<calendarVM>();
            foreach (HolidayList item in obj)
            {
                calendarVM list = new calendarVM();
                list.title = item.Description;
                list.date = item.Date;


                var strHref = "~/Leave/AddHoliday" + "?HolidayId=" + item.HolidayId;

                var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                string hrefUrl = UrlHelper.GenerateContentUrl(strHref, context);
                list.url = hrefUrl;
                holidayList.Add(list);



                holidayVm.calendarVM = holidayList;
                holidayVm.HolidayList = GetById(HolidayId);
            }

            return holidayVm;
        }

        public void Add(HolidayList holidayList)
        {
            try
            {
                holidayList.CreatedBy = sessionObj.USERID;
                holidayList.CreatedOn = UTILITY.SINGAPORETIME;
                if (sessionObj.BRANCHID != -1)
                    holidayList.BranchID = sessionObj.BRANCHID;

                holidayListRepository.Add(holidayList);

                //if (holidayList.HolidayId != -1)
                //{

                //}
                //else
                //{
                //    holidayList.ModifiedBy = sessionObj.USERID;
                //    holidayList.ModifiedOn = UTILITY.SINGAPORETIME;
                //    holidayListRepository.Add(holidayList);
                //}


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
                holidayListRepository.Delete(id);
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

        public IEnumerable<HolidayList> GetAll()
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

        public HolidayList GetByProperty(Func<HolidayList, bool> predicate)
        {

            try
            {
                return holidayListRepository.GetByProperty(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<HolidayList> GetListByProperty(Func<HolidayList, bool> predicate)
        {

            try
            {
                return holidayListRepository.GetListByProperty(predicate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}