
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models;


namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private EmployeeContext _db = new EmployeeContext();

        private IHostEnvironment _hostEnvironment;

        private readonly ILogger _logger;

        public FileController(ILogger<FileController> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _hostEnvironment = environment;
        }

        [HttpPost(Name = "UploadFile")]
        public ActionResult UploadFile(IFormFile formFile)
        {
            if (formFile != null)
            {
                //ถ้ามี ข้อมูลไฟล์เข้ามา ก็สร้าง Object จากโมเดล File
                Models.File file = new Models.File
                {
                    FileName = formFile.FileName,
                    FilePath = "Uploaded/ProfileImg/"
                };

                // เพิ่มเข้าฐานข้อมูลเพื่อเอา "ID"
                file = Models.File.Create(_db, file);

                // เอา "ID" มากำหนดเป็นชื่อ "โฟลเดอร์"
                string uploads = Path.Combine(_hostEnvironment.ContentRootPath, "Uploaded/ProfileImg/" + file.Id);

                // สร้างโฟลเดอร์เก็บไฟล์
                Directory.CreateDirectory(uploads);

                string filePath = Path.Combine(uploads, formFile.FileName);

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    formFile.CopyTo(stream);
                }

                return Ok(new Response
                {
                    Status = 200,
                    Message = "File uploaded successfully",
                    Data = file
                });
            }
            else
            {
                return BadRequest(new Response
                {
                    Status = 400,
                    Message = "File is required",
                });
            }
        }

        [HttpPost("V2", Name = "UploadFiles")]
        public ActionResult UploadFiles(List<IFormFile> formFiles)
        {
            if (!formFiles.IsNullOrEmpty())
            {
                foreach (IFormFile f in formFiles)
                {
                    Models.File file = new Models.File
                    {
                        FileName = f.FileName,
                        FilePath = "Uploaded/ProfileImg/"
                    };

                    // Add to the database to get the "ID"
                    file = Models.File.Create(_db, file);

                    if (f != null && f.Length > 0)
                    {
                        // Use the "ID" as the folder name
                        string uploads = Path.Combine(_hostEnvironment.ContentRootPath, "Uploaded/ProfileImg/" + file.Id);
                        Directory.CreateDirectory(uploads); // Create directory to store the file

                        string filePath = Path.Combine(uploads, f.FileName);
                        using (Stream stream = new FileStream(filePath, FileMode.Create))
                        {
                            f.CopyTo(stream);
                        }
                    }
                }

                return Ok(new Response
                {
                    Status = 200,
                    Message = "Files uploaded successfully",
                    Data = formFiles
                });
            }
            else
            {
                return BadRequest(new Response
                {
                    Status = 400,
                    Message = "At least one file is required",
                });
            }
        }
    }
}
