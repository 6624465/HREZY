using HR.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HR.Web.Services.Operation
{
    public class EmpSalaryStructureHeaderService : IRepository<EmpSalaryStructureHeader>
    {
        public void Add(EmpSalaryStructureHeader entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                var obj = dbCntx.EmpSalaryStructureHeaders
                    .Where(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId)
                    .FirstOrDefault();

                //GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);

                if (obj == null)
                {
                    dbCntx.EmpSalaryStructureHeaders.Add(entity);

                    //Update(entity);
                }
                else
                {
                    obj.IsActive = entity.IsActive;
                    obj.ModifiedBy = entity.ModifiedBy;
                    obj.ModifiedOn = entity.ModifiedOn;
                    obj.Remarks = entity.Remarks;
                    obj.Salary = entity.Salary;
                    obj.TotalDeductions = entity.TotalDeductions;
                    obj.TotalGross = entity.TotalGross;

                }
                dbCntx.SaveChanges();
            }
        }

        private void Update(EmpSalaryStructureHeader entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);
                obj.ModifiedBy = entity.ModifiedBy;
                obj.ModifiedOn = entity.ModifiedOn;

                dbCntx.SaveChanges();
            }
        }

        public void Delete(EmpSalaryStructureHeader entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);
                obj.IsActive = false;

                dbCntx.SaveChanges();
            }
        }

        public IEnumerable<EmpSalaryStructureHeader> GetAll()
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmpSalaryStructureHeaders.ToList();
            }
        }

        public EmpSalaryStructureHeader GetById(int id)
        {
            throw new NotImplementedException();
        }

        public EmpSalaryStructureHeader GetByProperty(Func<EmpSalaryStructureHeader, bool> predicate)
        {
            using (var dbCntx = new HrDataContext())
            {
                return dbCntx.EmpSalaryStructureHeaders
                            .Where(predicate)
                            .FirstOrDefault();
            }
        }
    }
}