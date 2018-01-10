using HR.Web.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmpSalaryStructureDetailService : IRepository<EmpSalaryStructureDetail>
    {
        public void Add(EmpSalaryStructureDetail entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                List<EmpSalaryStructureDetail> detailList = dbCntx.EmpSalaryStructureDetails
                    .Where(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId && x.Code == entity.Code)
                    .ToList();
                
                if (detailList.Count > 0)
                    dbCntx.EmpSalaryStructureDetails.RemoveRange(detailList);

                //EmpSalaryStructureDetail obj = dbCntx.EmpSalaryStructureDetails
                //    .Where(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId && x.Code == entity.Code)
                //    .FirstOrDefault();

                dbCntx.EmpSalaryStructureDetails.Add(entity);
                dbCntx.SaveChanges();
                

            }
        }

        private void Update(EmpSalaryStructureDetail entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                EmpSalaryStructureDetail obj = dbCntx.EmpSalaryStructureDetails
                    .Where(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId && x.Code == entity.Code)
                    .FirstOrDefault();
                obj.Amount = entity.Amount;
                obj.Code = entity.Code;
                obj.BranchId = entity.BranchId;
                obj.Computation = entity.Computation;
                obj.ContributionRegister = entity.ContributionRegister;
                obj.EmployeeId = entity.EmployeeId;
                obj.IsActive = entity.IsActive;
                obj.Total = entity.Total;
                obj.ModifiedBy = entity.ModifiedBy;
                obj.ModifiedOn = entity.ModifiedOn;

                dbCntx.SaveChanges();
            }
        }

        public EmpSalaryStructureDetail GetByProperty(Func<EmpSalaryStructureDetail, bool> predicate)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmpSalaryStructureDetails
                            .Where(predicate)
                            .FirstOrDefault();
            }
        }

        public void Delete(EmpSalaryStructureDetail entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId && x.Code == entity.Code);

                if (obj != null)
                {
                    obj.IsActive = false;
                    dbCntx.SaveChanges();
                }
            }
        }

        public IEnumerable<EmpSalaryStructureDetail> GetAll()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmpSalaryStructureDetails.ToList();
            }
        }

        public EmpSalaryStructureDetail GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}