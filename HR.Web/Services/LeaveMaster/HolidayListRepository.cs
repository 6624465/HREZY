using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.LeaveMaster
{
    public class HolidayListRepository : IRepository<HolidayList>
    {
        public HolidayListRepository()
        {
        }

        public void Add(HolidayList entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    HolidayList holidayList = dbContext.HolidayLists
                        .Where(x => x.HolidayId == entity.HolidayId).FirstOrDefault();
                    if (holidayList == null)
                    {
                        
                        dbContext.HolidayLists.Add(entity);
                    }
                    else
                    {
                        holidayList.Date = entity.Date;
                        holidayList.Description = entity.Description;
                        holidayList.CountryId = entity.CountryId;
                        holidayList.ModifiedBy = entity.ModifiedBy;
                        holidayList.ModifiedOn = entity.ModifiedOn;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void Delete(HolidayList entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    dbContext.HolidayLists.Remove(entity);
                    dbContext.SaveChanges();
                }
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
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.HolidayLists.ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public HolidayList GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.HolidayLists.Where(x => x.HolidayId == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}