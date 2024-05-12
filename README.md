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

namespace WebApi.Models
{
  public class EmployeeMetadata { }

  [MetadataType(typeof(EmployeeMetadata))]
  public partial class Employee
  {

  }
}
```
