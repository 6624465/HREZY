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
                            .Where(x => x.EmployeeId == entity.EmployeeId &&
                                        x.BranchId == entity.BranchId)
                            .FirstOrDefault();

                if(obj != null)
                {
                    Update(entity, dbCntx);
                }
                else
                {
                    
                }
            }
        }

        public void Update(EmpSalaryStructureHeader entity, HrDataContext dbCntx)
        {

        }

        public void Delete(EmpSalaryStructureHeader entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmpSalaryStructureHeader> GetAll()
        {
            throw new NotImplementedException();
        }

        public EmpSalaryStructureHeader GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}