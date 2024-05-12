# Create a web project

```bash
dotnet new webapi --use-controllers -o WebApi
cd WebApi
dotnet add package Microsoft.EntityFrameworkCore.InMemory
code -r ../WebApi
```

```bash
touch .gitignore README.md
```

- .gitignore for untracked directory & files ( bin, obj )
- README.md for written documentation

# Initial Github

- Create a repository : dotnet-webapi to https://github.com/new

```bash
git init
git add .
git commit -m "first commit"
git branch -M main
git remote add origin https://github.com/jayrboy/dotnet-webapi.git
git push -u origin main
```

# SQL Server Management Studio

1. Create the database

- Database name: Employee
- Options -> Collation: Thai_CI_AS

2. New Table: Employee (Name; Type; Allow Nulls)

ID; int; false
Firstname; nvarchar(50); true
Lastname; nvarchar(50); true
Salary; int; true
IsDelete; bit; true
CreateDate; datetime; true
UpdateDate; datetime; true

- Set Primary Key: ID
- Identity (Name): Employee
- Identity Column: ID

```bash
dotnet tool install --global dotnet-ef --version 8.*
```

- เพิ่ม InvariantGlobalization เป็น false ที่ไฟล์ WebApi/WebApi.csproj

```cs
<PropertyGroup>
    ...
    ...
    <InvariantGlobalization>false</InvariantGlobalization>
</PropertyGroup>
```

- Create a folder: Models
- Add "ConnectionStrings": { "DefaultConnection": "..." } ที่ไฟล์ WebApi/appsettings.json

```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost; Database=Employee; Trusted_Connection=False; TrustServerCertificate=True; User ID=sa; Password=Password"
  }
}
```

## Scaffold Database มาใส่ Models

```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
```

```bash
dotnet ef dbcontext scaffold "Data Source=BUMBIM\SQLEXPRESS;Initial Catalog=Employee;Integrated Security=True;Encrypt=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer --context-dir Data --output-dir Models --force
```

- หากสำเร็จจะมีข้อมูล Table EmployeeContext จาก Database อยู่ใน Folder Data
- และมี Model ทำหน้าที่จัดเตรยมข้อมูล เพื่อนำไปใช้
  - Data Logic
  - เป็นอิสระ ไม่ถูกกระทบจาก Controller
  - ตัวอย่าง: การเช็ค User ซ้ำใน Model User ควรอยู่ใน Model เมื่อนำไปใช้กับ Project อื่นก็ยังมีการเช็ค user ซ้ำได้เหมือนกัน ไม่ขึ้นกับ Controller

## Add Database Context Services by ConnectionStrings

```cs
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Connect SQL Server Management Studio
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EmployeeContext>(option => option.UseSqlServer(connectionString));
```

## Metadata file

- New C# -> Class -> EmployeeMetadata

```cs
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
  public class EmployeeMetadata { }

  [MetadataType(typeof(EmployeeMetadata))]
  public partial class Employee
  {
    public static Employee Create(EmployeeContext db, Employee employee) { }

    public static List<Employee> GetAll(EmployeeContext db) { }

    public static Employee GetById(EmployeeContext db, int id) { }

    public static Employee Update(EmployeeContext db, Employee employee) { }

    public static Employee Delete(EmployeeContext db, int id) { }

    public static List<Employee> Search(EmployeeContext db, string keyword) { }
  }
}
```

## Controller

- New C# -> Class -> EmployeeController

```cs
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
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
            public string? FirstName { get; set; }

            /// <summary>
            /// Employee Last Name
            /// </summary>
            /// <example>Don</example>
            /// <required>true</required>
            [Required]
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

        [HttpPost(Name = "CreateEmployee")]
        public ActionResult Create(EmployeeCreate employeeCreate) { }

        [HttpGet(Name = "GetAllEmployee")]
        public ActionResult GetAll() { }

        [HttpGet("{id}", Name = "GetEmployeeById")]
        public ActionResult GetById(int id) { }

        [HttpPut(Name = "UpdateEmployee")]
        public ActionResult Update(Employee employee) { }

        [HttpDelete("{id}", Name = "DeleteEmployeeById")]
        public ActionResult DeleteById(int id) { }

        [HttpGet("search/{name}", Name = "SearchEmployeeByName")]
        public ActionResult Search(string name) { }

        [HttpGet("page/{page}", Name = "GetAllEmployeeByPage")]
        public ActionResult GetAllByPage(int page) { }
    }
}
```

# Generate API Spec with Swagger

- API Spec เครื่องมือสำคัญที่ช่วยให้ Dev สามารถทำความเข้าใจกับการทำงานของ API ได้อย่างชัดเจน และช่วยให้สามารถสร้างและใช้งาน API ได้ถูกต้องปลอดภัย

- เพิ่ม Generate ในไฟล์ WebApi/WebApi.csproj

```cs
  <PropertyGroup>
    ...
    ...
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn>
  </PropertyGroup>
```

- เพิ่ม version, title, description ของ API บนหน้า Swagger

```cs
using System.Reflection;

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "My WebApi Project API",
        Description = "A simple example ASP.NET Core Web API",
    });

    // สำหรับใช้งาน XML Comment
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
```

## XML Comment - Action & Response

```
        /// <summary>
        /// Create Employee
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
        /// <response code="201">
        /// Success
        /// <br/>
        /// <br/>
        /// Example response:
        ///
        ///     {
        ///         "Code": 201,
        ///         "Message": "Success",
        ///         "Data": {
        ///             "Id": 1,
        ///             "FirstName": "John",
        ///             "LastName": "Doe",
        ///             "Salary": 25000,
        ///             "DepartmentId": 1
        ///         }
        ///     }
        ///
        /// </response>
        /// <response code="400">Bad Request</response>
        /// <response code="500">Internal Server Error</response>
```

### XML Comment - Attribute

```cs
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
```
