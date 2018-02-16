using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ali.Planning.API.Repositories;
using Ali.Planning.API.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Ali.Planning.API.Filters;

namespace Ali.Planning.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [EnableCors("Any")]
    [ValidateModel]
    public class EmployeesController : Controller
    {
        private ILogger<EmployeesController> _logger;
        private IEmployeeRepository _employeeRepo;
        private IPlanningRepository _planningRepo;

        public EmployeesController(
            ILogger<EmployeesController> logger,
            IEmployeeRepository employeeRepo,
            IPlanningRepository planningRepo)
        {
            _logger = logger;
            _employeeRepo = employeeRepo;
            _planningRepo = planningRepo;
        }

        // GET: api/employees
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var employees = _employeeRepo.Get().ToList();
                return Ok(employees);
            }
            catch (Exception e)
            {
                ///TODO: Handle exception 
                _logger.LogError(e.ToString());
                return BadRequest("Internal error!");
            }
        }

        // GET api/values/5
        [HttpGet("{employeeId}/plans")]
        public IActionResult Get(int employeeId)
        {
            try
            {
                //var plans = _planningRepo.Get().Where(p => p.EmployeeId == employeeId);
                var plans = _planningRepo.GetEmployeePlansWithProjectName(employeeId);
                return Ok(plans);
            }
            catch (Exception e)
            {
                ///TODO: Handle exception 
                _logger.LogError(e.ToString());
                //return BadRequest("Internal error!");
                return StatusCode(500, "InternalError");
            }
        }  
    }
}
