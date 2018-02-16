using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Ali.Planning.API.Repositories;
using Ali.Planning.API.Model;
using System.Linq;

namespace UnitTests.Repositories
{
    public class PlanningRepo
    {
        private Mock<IConfiguration> mockConfig;
        public PlanningRepo()
        {
            mockConfig = new Mock<IConfiguration>();
            //mockConfig.Setup(x => x.GetSection("ConnectionStrings:DefaultConnection").Value).Returns("Server=(localdb)\\MSSQLLocalDB;Database=PlanningDb;Trusted_Connection=True;");
        }

        [Fact]
        public void ShouldReturnListOfPlansWithProjectNames()
        {
            var options = new DbContextOptionsBuilder<PlanningDataContext>()
                .UseInMemoryDatabase(databaseName: "Test_Planning_Database")
                .Options;


            using (var ctx = new PlanningDataContext(options, mockConfig.Object, true))
            {
                ctx.Plannings.AddRange(
                    new ProjectPlanning
                    {
                        Id = 1,
                        EmployeeId = 1,
                        ProjectId = 1,
                        Q1 = 1,
                        Q2 = 1,
                        Q3 = 1,
                        Q4 = 1,
                        year = 2018,
                        Deleted = false

                    },
                    new ProjectPlanning
                    {
                        Id = 2,
                        EmployeeId = 1,
                        ProjectId = 2,
                        Q1 = 1,
                        Q2 = 1,
                        Q3 = 1,
                        Q4 = 1,
                        year = 2018,
                        Deleted = true

                    },
                    new ProjectPlanning
                    {
                        Id = 3,
                        EmployeeId = 2,
                        ProjectId = 2,
                        Q1 = 1,
                        Q2 = 1,
                        Q3 = 1,
                        Q4 = 1,
                        year = 2018,
                        Deleted = true

                    }

                    );

                ctx.Projects.AddRange(
                    new Project
                    {
                        Id = 1,
                        ProjectName = "test1"
                    },
                    new Project
                    {
                        Id = 2,
                        ProjectName = "test2"
                    }
                    );

                ctx.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var ctx = new PlanningDataContext(options, mockConfig.Object, true))
            {
                var planningRepo = new PlanningRepository(ctx);
                var result = planningRepo.GetEmployeePlansWithProjectName(1).ToList();

                Assert.Single(result);
            }
        }
    }
}
