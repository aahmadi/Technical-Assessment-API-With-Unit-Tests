
using Ali.Planning.API.Model;
using Entities;
using System.Collections.Generic;
using System.Linq;

namespace Ali.Planning.API.Repositories
{
    public interface IPlanningRepository:
        IRepository<ProjectPlanning>
    {
        IEnumerable<PlanModel> GetEmployeePlansWithProjectName(int employeeId);
    }
}
