using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;



namespace HR.Web.Services.Payroll
{
    public class PayslipBatchHeaderRepository : IRepository<PayslipBatchHeader>
    {
        public void Add(PayslipBatchHeader entity)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    PayslipBatchHeader payslipBatchHeader = dbContext.PayslipBatchHeaders
                        .Where(x => x.BatchHeaderId == entity.BatchHeaderId).FirstOrDefault();
                    if (payslipBatchHeader == null)
                    {
                        dbContext.PayslipBatchHeaders.Add(entity);
                        dbContext.SaveChanges();
                    }
                    else
                    {
                        payslipBatchHeader.BatchHeaderId = entity.BatchHeaderId;
                        payslipBatchHeader.BatchNo = entity.BatchNo;
                        payslipBatchHeader.Month = entity.Month;
                        payslipBatchHeader.Year = entity.Year;
                        payslipBatchHeader.TotalSalary = entity.TotalSalary;
                        payslipBatchHeader.ProcessDate = entity.ProcessDate;
                        dbContext.SaveChanges();

                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public void Delete(PayslipBatchHeader entity)
        {
            try
            {
                using (var dbcntx = new HrDataContext())
                {
                    dbcntx.PayslipBatchHeaders.Attach(entity);
                    dbcntx.PayslipBatchHeaders.Remove(entity);
                    dbcntx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<PayslipBatchHeader> GetAll()
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchHeader GetById(int id)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public IEnumerable<PayslipBatchHeader> GetListByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(predicate).ToList();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchHeader GetByProperty(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(predicate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public System.Data.DataTable GeneratePayslip(Int16 BranchId, int CurrentMonth, int CurrentYear)
        {
            using (var dbCntx = new HrDataContext())
            using (SqlConnection Con = new
                SqlConnection(dbCntx.Database.Connection.ConnectionString))
            {
                Con.Open();
                SqlCommand Cmd = new SqlCommand();

                Cmd.Connection = Con;
                Cmd.CommandType = System.Data.CommandType.StoredProcedure;
                Cmd.CommandText = "[Payroll].[GeneratePayslip]";

                Cmd.Parameters.Add("@BranchID", System.Data.SqlDbType.SmallInt);
                Cmd.Parameters.Add("@CurrentMonth", System.Data.SqlDbType.Int);
                Cmd.Parameters.Add("@CurrentYear", System.Data.SqlDbType.Int);

                Cmd.Parameters["@BranchID"].Value = BranchId;
                Cmd.Parameters["@CurrentMonth"].Value = CurrentMonth;
                Cmd.Parameters["@CurrentYear"].Value = CurrentYear;
                

                System.Data.DataTable dt = new System.Data.DataTable();
                var da = new SqlDataAdapter(Cmd);
                
                da.Fill(dt);

                Con.Close();

                return dt;
            }

        }
        internal int GetCount(int branchId)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(x=>x.BranchId==branchId).Count();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public PayslipBatchHeader GetLatestRecord(Func<PayslipBatchHeader, bool> predicate)
        {
            try
            {
                using (HrDataContext dbContext = new HrDataContext())
                {
                    return dbContext.PayslipBatchHeaders.Where(predicate).OrderByDescending(x=>x.BatchHeaderId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}