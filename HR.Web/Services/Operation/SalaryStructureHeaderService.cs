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
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);

                if(obj != null)
                {
                    Update(entity);
                }
                else
                {
                    dbCntx.EmpSalaryStructureHeaders.Add(entity);
                    dbCntx.SaveChanges();
                }
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