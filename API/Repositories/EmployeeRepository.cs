using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ali.Planning.API.Repositories
{
   
    public class EmployeeRepository:
        BaseRepository<Employee>,
        IEmployeeRepository
    {
        public EmployeeRepository(PlanningDataContext ctx)
            :base(ctx)
        {}
    }
}
