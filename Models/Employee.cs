using System;
using System.Collections.Generic;

namespace WebApi.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public int? Salary { get; set; }

    public bool? IsDelete { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? DepartmentId { get; set; }

    public virtual Department? Department { get; set; }
}
