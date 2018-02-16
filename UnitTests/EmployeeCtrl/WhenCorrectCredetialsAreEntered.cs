using System;
using Xunit;
using Microsoft.Extensions.Logging;
using Ali.Planning.API.Controllers;
using Microsoft.AspNetCore.Identity;
using Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using API.Model;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Ali.Planning.API.Repositories;
using System.Linq;
using Ali.Planning.API.Model;

namespace UnitTests
{

    public class WhenEmployeesAreRequested
    {
        private Mock<IEmployeeRepository> mockEmployeeRepository;
        private Mock<IPlanningRepository> mockPlanningRepository;
        private List<Employee> employees;
        private IEnumerable<PlanModel> employeesPlan;

        public WhenEmployeesAreRequested()
        {
            IEnumerable<Employee> employees = new List<Employee>
            {
                new Employee{FirstName = "Ali", LastName="Ahmadi"},
                new Employee{FirstName = "John", LastName="Jackson"},
                new Employee{FirstName = "Patrick", LastName="Breen"},
                new Employee{FirstName = "Marcelo", LastName = "Paniza"}
            };

            IEnumerable<PlanModel> employeesPlan = new List<PlanModel>
            {
                new PlanModel{EmployeeId = 1, ProjectName="testProj1", Q1 = 10, Q2 = 20, Q3=40, Q4 = 50},
                new PlanModel{EmployeeId = 1, ProjectName="testProj1", Q1 = 10, Q2 = 20, Q3=40, Q4 = 50},
                new PlanModel{EmployeeId = 1, ProjectName="testProj1", Q1 = 10, Q2 = 20, Q3=40, Q4 = 50}
            };

            
            mockEmployeeRepository = new Mock<IEmployeeRepository>();
            mockEmployeeRepository.Setup(x => x.Get()).Returns(employees);

            mockPlanningRepository = new Mock<IPlanningRepository>();
            mockPlanningRepository.Setup(x => x.GetEmployeePlansWithProjectName(It.IsAny<int>())).
                Returns(employeesPlan);

            
        }

        [Fact]
        public void GetEmployeesShouldReturnIQueryableListOfEmployees()
        {
            var employeesController = new EmployeesController(
                new Logger<EmployeesController>(new LoggerFactory()),
                mockEmployeeRepository.Object,
                mockPlanningRepository.Object);

            var result = employeesController.Get();

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.Equal(4, ((IEnumerable<Employee>)((OkObjectResult)result).Value).ToList<Employee>().Count);
        }

        [Fact]
        public void GetEmployeesShouldReturnBadRequestResultOnExceptions()
        {

            mockEmployeeRepository.Setup(x => x.Get()).Throws(new Exception());

            var employeesController = new EmployeesController(
                new Logger<EmployeesController>(new LoggerFactory()),
                mockEmployeeRepository.Object,
                mockPlanningRepository.Object);

            var result = employeesController.Get();

            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, ((BadRequestObjectResult)result).StatusCode);
            Assert.Equal("Internal error!", ((BadRequestObjectResult)result).Value);
        }

        [Fact]
        public void GetEmployeePlansShouldReturnAListOfEmployeePlans()
        {
            var employeesController = new EmployeesController(
                new Logger<EmployeesController>(new LoggerFactory()),
                mockEmployeeRepository.Object,
                mockPlanningRepository.Object);

            var result = employeesController.Get(1);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, ((OkObjectResult)result).StatusCode);
            Assert.Equal(3, ((IEnumerable<PlanModel>)((OkObjectResult)result).Value).ToList<PlanModel>().Count);
        }

        [Fact]
        public void GetEmployeePlansShouldReturn500OnErrors()
        {
            mockPlanningRepository.Setup(x => x.GetEmployeePlansWithProjectName(It.IsAny<int>())).Throws(new Exception());
            var employeesController = new EmployeesController(
                new Logger<EmployeesController>(new LoggerFactory()),
                mockEmployeeRepository.Object,
                mockPlanningRepository.Object);

            var result = employeesController.Get(1);

         
            Assert.Equal(500, ((ObjectResult)result).StatusCode);
            Assert.Equal("InternalError" , ((ObjectResult)result).Value);
        }

    }
}
