## .NET 10 Web API

### ActionResult<T> vs IActionResult

https://learn.microsoft.com/en-us/aspnet/core/web-api/action-return-types?view=aspnetcore-10.0

| Case                | IActionResult            | ActionResult<T>    |
| ------------------- | ------------------------ | ------------------ |
| ระบุ response type  | ต้องใส่ Type = typeof(T) | ไม่ต้องใส่         |
| Swagger infer type  | ไม่ได้                   | ได้จาก <T>         |
| return object ตรง ๆ | ต้อง Ok(obj)             | return obj; ได้เลย |
| implicit cast       | ❌                       | ✅                 |

ActionResult<T>

- ไม่ต้องระบุ Type ใน ProducesResponseType
- มี implicit conversion กรณี return model ตรง ๆ จะถูก convert เป็น return new ObjectResult(product); โดยอัตโนมัติ
- return ActionResult ได้เหมือน IActionResult

ใช้ ActionResult<T> (แนะนำ)

- Web API
- REST API
- ใช้ Swagger
- ต้องการ clean code
- ต้องการ strong typing

ใช้ IActionResult

- Response หลายแบบมาก ๆ ที่ไม่เกี่ยวกับ T
- Controller แบบ MVC (View + PartialView)
- Legacy code

### Jwt Auth in Scalar UI

### Migration

- Initial in Application

```bash
$ dotnet ef migrations add InitialMigration -o Data/Migrations  
```

- Run Migration SQL Server

```bash
$ dotnet ef database update
```
