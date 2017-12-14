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
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);
                if(obj != null)
                {
                    Update(entity);
                }
                else
                {
                    dbCntx.EmpSalaryStructureDetails.Add(entity);
                    dbCntx.SaveChanges();
                }
            }
        }

        private void Update(EmpSalaryStructureDetail entity)
        {
            using (var dbCntx = new HrDataContext())
            {
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);
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
                var obj = GetByProperty(x => x.EmployeeId == entity.EmployeeId && x.BranchId == entity.BranchId);

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