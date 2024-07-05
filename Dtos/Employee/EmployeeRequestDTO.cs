using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos.Employee
{
    public class EmployeeRequestDTO
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
}