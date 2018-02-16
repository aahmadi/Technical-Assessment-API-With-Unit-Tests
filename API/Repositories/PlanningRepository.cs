using Ali.Planning.API.Model;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Planning.API.Repositories
{
    public class PlanningRepository:
        BaseRepository<ProjectPlanning>,
        IPlanningRepository
    {
        public PlanningRepository(PlanningDataContext ctx)
            :base(ctx)
        { }

        public IEnumerable<PlanModel> GetEmployeePlansWithProjectName(int employeeId)
        {
           return _context.Plannings.Where(e => e.EmployeeId == employeeId && !e.Deleted).Join(_context.Projects, p => p.ProjectId, j => j.Id, (p, j) => new { plan = p, proj = j})
                .Select(r => new PlanModel {
                    EmployeeId = r.plan.EmployeeId,
                    ProjectName = r.proj.ProjectName,
                    year = r.plan.year,
                    Q1 = r.plan.Q1,
                    Q2 = r.plan.Q2,
                    Q3 = r.plan.Q3,
                    Q4 = r.plan.Q4,
                });
            
        }
    }
}
