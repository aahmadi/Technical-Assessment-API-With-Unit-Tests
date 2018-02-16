using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Entities.TestData
{
    public class DatabaseInitializer
    {
        private PlanningDataContext _ctx;
        private RoleManager<IdentityRole> _roleMgr;
        private UserManager<PlanningUser> _userMgr;

        #region Test data

        private List<Project> _projectList = new List<Project>
        {
            new Project
            {
                //ProjectId = 1,
                ProjectName = "HWY 407 Extension",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(5),
                CreatedBy = "Ali"
            },
            new Project
            {
                //ProjectId = 2,
                ProjectName = "Kids Hospital",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(10),
                CreatedBy = "Ali"
            },
            new Project
            {
                //ProjectId = 3,
                ProjectName = "Eglinton LRT",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(3),
                CreatedBy = "Ali"
            },
            new Project
            {
                //ProjectId = 4,
                ProjectName = "Gormley Go Station",
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddYears(1),
                CreatedBy = "Ali"
            }
        };

        private readonly List<Employee> _employeeList = new List<Employee> {
            new Employee
            {
                //EmployeeId = 1,
                FirstName = "John",
                LastName = "Jackson",
                Title = "Project Manager",
                Email ="john@somecompany.ca",
                CreatedBy = "Ali",
                DateCreated = DateTime.Now,
                Plans = new List<ProjectPlanning>
                {
                     new ProjectPlanning
                     {
                         //PlanningId = 1,
                         EmployeeId = 1,
                         ProjectId = 1,
                         year = DateTime.Today.Year,
                         Q1 = 20,
                         Q2 = 20,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 2,
                         EmployeeId = 1,
                         ProjectId = 2,
                         year = DateTime.Today.Year,
                         Q1 = 80,
                         Q2 = 30,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 3,
                         EmployeeId = 1,
                         ProjectId = 3,
                         year = DateTime.Today.Year,
                         Q1 = 0,
                         Q2 = 50,
                         Q3 = 40,
                         Q4 = 0,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 4,
                         EmployeeId = 1,
                         ProjectId = 1,
                         year = DateTime.Today.Year + 1,
                         Q1 = 50,
                         Q2 = 40,
                         Q3 = 30,
                         Q4 = 20,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 5,
                         EmployeeId = 1,
                         ProjectId = 2,
                         year = DateTime.Today.Year + 1,
                         Q1 = 50,
                         Q2 = 60,
                         Q3 = 70,
                         Q4 = 80,
                         CreatedBy = "Ali",
                     }
                }
            },
            new Employee
            {
                //EmployeeId = 2,
                FirstName = "Dan",
                LastName = "Davidson",
                Title = "Engineer",
                Email ="dan@somecompany.ca",
                CreatedBy = "Ali",
                DateCreated = DateTime.Now,
                Plans = new List<ProjectPlanning>
                {
                     new ProjectPlanning
                     {
                         //PlanningId = 6,
                         EmployeeId = 2,
                         ProjectId = 1,
                         year = DateTime.Today.Year,
                         Q1 = 20,
                         Q2 = 20,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 7,
                         EmployeeId = 2,
                         ProjectId = 2,
                         year = DateTime.Today.Year,
                         Q1 = 80,
                         Q2 = 30,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 8,
                         EmployeeId = 2,
                         ProjectId = 3,
                         year = DateTime.Today.Year,
                         Q1 = 0,
                         Q2 = 50,
                         Q3 = 40,
                         Q4 = 0,
                         CreatedBy = "Ali",
                     }
                }
            },
            new Employee
            {
                //EmployeeId = 3,
                FirstName = "Adam",
                LastName = "Carr",
                Title = "Engineer",
                Email ="adam@somecompany.ca",
                CreatedBy = "Ali",
                DateCreated = DateTime.Now,
                Plans = new List<ProjectPlanning>
                {
                     new ProjectPlanning
                     {
                         //PlanningId = 9,
                         EmployeeId = 3,
                         ProjectId = 2,
                         year = DateTime.Today.Year,
                         Q1 = 20,
                         Q2 = 20,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 10,
                         EmployeeId = 3,
                         ProjectId = 3,
                         year = DateTime.Today.Year,
                         Q1 = 80,
                         Q2 = 30,
                         Q3 = 30,
                         Q4 = 50,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 11,
                         EmployeeId = 3,
                         ProjectId = 4,
                         year = DateTime.Today.Year,
                         Q1 = 0,
                         Q2 = 50,
                         Q3 = 40,
                         Q4 = 0,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 12,
                         EmployeeId = 3,
                         ProjectId = 1,
                         year = DateTime.Today.Year + 1,
                         Q1 = 50,
                         Q2 = 40,
                         Q3 = 30,
                         Q4 = 20,
                         CreatedBy = "Ali",
                     },
                     new ProjectPlanning
                     {
                         //PlanningId = 13,
                         EmployeeId = 3,
                         ProjectId = 2,
                         year = DateTime.Today.Year + 1,
                         Q1 = 50,
                         Q2 = 60,
                         Q3 = 70,
                         Q4 = 80,
                         CreatedBy = "Ali",
                     }
                }
            },
        };

        #endregion
        public DatabaseInitializer(PlanningDataContext ctx, 
            UserManager<PlanningUser> userMgr,
            RoleManager<IdentityRole> roleMgr)
        {
            _ctx = ctx;
            _userMgr = userMgr;
            _roleMgr = roleMgr;
        }

        public async Task Seed()
        {
            if (!_ctx.Employees.Any())
            {
                _ctx.Projects.AddRange(_projectList);
                _ctx.Employees.AddRange(_employeeList);
                await _ctx.SaveChangesAsync();
            }

            // Planning users
            var user = await _userMgr.FindByNameAsync("aahmadi");
            if(user == null)
            {
                if (!(await _roleMgr.RoleExistsAsync("Admin")))
                {
                    var role = new IdentityRole("Admin");
                    //role.Claims.Add(new IdentityRoleClaim<string>() { ClaimType = "IsAdmin", ClaimValue = "True" });
                    await _roleMgr.CreateAsync(role);
                }

                user = new PlanningUser
                {
                    UserName = "aahmadi",
                    FirstName = "Ali",
                    LastName = "Ahmadi",
                    Email = "arahmadi06@gmail.com"
                };

                var userResult = await _userMgr.CreateAsync(user, "P@ssw0rd!");
                var roleResult = await _userMgr.AddToRoleAsync(user, "Admin");
                var claimResult = await _userMgr.AddClaimAsync(user, new Claim("SuperUser", "True"));

                if (!userResult.Succeeded || !roleResult.Succeeded || !claimResult.Succeeded)
                {
                    throw new InvalidOperationException("Failed to build user and roles");
                }
            }
        }
       
    }
}
