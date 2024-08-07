using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Dtos.Employee;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private EmployeeContext _db = new EmployeeContext();

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Creates a Employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns>A newly created Employee</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /api/Employee
        ///     {
        ///         "FirstName": "John",
        ///         "LastName": "Don",
        ///         "Salary": 25000,
        ///         "DepartmentId": 1
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly created employee</response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost(Name = "CreateEmployee")]
        public ActionResult Create(EmployeeRequestDTO employeeRequest)
        {
            Employee employee = new Employee
            {
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
                Salary = employeeRequest.Salary,
                DepartmentId = employeeRequest.DepartmentId
            };

            employee = Employee.Create(_db, employee);

            return Ok(new Response
            {
                Status = 200,
                Message = "Employee saved",
                Data = employee
            }
            );
        }

        [HttpGet(Name = "GetAllEmployee")]
        public ActionResult GetAll()
        {
            // List<Employee> employees = Employee.GetAll(_db).OrderBy(q => q.Salary).ToList(); // น้อย -> มาก
            List<Employee> employees = Employee.GetAll(_db).OrderByDescending(q => q.Salary).ToList(); // มาก -> น้อย
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employees
            });
        }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public ActionResult GetById(int id)
        {
            Employee employee = Employee.GetById(_db, id);
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employee
            });
        }

        [HttpPut(Name = "UpdateEmployee")]
        public ActionResult Update(Employee employee)
        {
            bool employeeExists = _db.Employees.Any(e => e.Id == employee.Id && e.IsDelete != true);
            if (!employeeExists)
            {
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "Employee not found",
                    Data = null
                });
            }
            try
            {
                employee = Employee.Update(_db, employee);
            }
            catch (Exception e)
            {
                return StatusCode(500, new Response
                {
                    Status = 500,
                    Message = e.Message,
                    Data = null
                });
            }
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employee
            });
        }

        /// <summary>
        /// Deletes a specific Employee ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}", Name = "DeleteEmployeeById")]
        public ActionResult DeleteById(int id)
        {
            Employee employee = Employee.Delete(_db, id);
            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employee
            });
        }

        [HttpGet("search/{name}", Name = "SearchEmployeeByName")]
        public ActionResult Search(string name)
        {
            List<Employee> employees = Employee.Search(_db, name);
            if (employees.Count == 0)
            {
                return NotFound(new Response
                {
                    Status = 404,
                    Message = "Employees not found",
                    Data = null,
                });
            }

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employees
            });
        }

        [HttpGet("page/{page}", Name = "GetAllEmployeeByPage")]
        public ActionResult GetAllByPage(int page)
        {
            int pageSize = 3;
            // ดึงข้อมูลพนักงานแบบหน้าเพจโดยใช้ Skip และ Take เพื่อจำกัดจำนวนข้อมูลที่ถูกดึง
            List<Employee> employees = Employee.GetAll(_db).OrderByDescending(q => q.Salary).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return Ok(new Response
            {
                Status = 200,
                Message = "Success",
                Data = employees
            });
        }
    }
}