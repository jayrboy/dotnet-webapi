using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Models
{
    public class UploadFileMetadata { }

    [MetadataType(typeof(UploadFileMetadata))]
    public partial class UploadFile
    {
        // [NotMapped]
        // public File? File { get; set; }

        public static UploadFile Create(EmployeeContext db, UploadFile file)
        {
            file.CreateDate = DateTime.Now;
            file.UpdateDate = DateTime.Now;
            file.IsDelete = false;
            db.UploadFiles.Add(file);
            db.SaveChanges();

            return file;
        }
    }
}