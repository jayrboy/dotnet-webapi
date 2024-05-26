using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
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

        public struct EmployeeCreate
        {
            /// <summary>
            /// Employee First Name
            /// </summary>
            /// <example>John</example>
            /// <required>true</required>
            [Required]
            [RegularExpression(@"^[a-zA-Z0-9]*$")]
            public string? FirstName { get; set; }

            /// <summary>
            /// Employee Last Name
            /// </summary>
            /// <example>Don</example>
            /// <required>true</required>
            [Required]
            [StringLength(50)]
            public string? LastName { get; set; }

            /// <summary>
            /// Employee Salary
            /// </summary>
            /// <example>25000</example>
            /// <required>true</required>
            [Required]
            [Range(15000, 80000)]
            public int? Salary { get; set; }

            /// <summary>
            /// Employee Department ID
            /// </summary>
            /// <example>1</example>
            /// <required>true</required>
            [Required]
            [Range(1, 5)]
            public int? DepartmentId { get; set; }
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
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Create(EmployeeCreate employeeCreate)
        {
            Employee employee = new Employee
            {
                FirstName = employeeCreate.FirstName,
                LastName = employeeCreate.LastName,
                Salary = employeeCreate.Salary,
                DepartmentId = employeeCreate.DepartmentId
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
                return BadRequest(new Response
                {
                    Status = 400,
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