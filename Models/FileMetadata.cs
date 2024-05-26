using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Models
{
    public class FileMetadata { }

    [MetadataType(typeof(FileMetadata))]
    public partial class File
    {
        // [NotMapped]
        // public File? File { get; set; }

        public static File Create(EmployeeContext db, File file)
        {
            file.CreateDate = DateTime.Now;
            file.UpdateDate = DateTime.Now;
            file.IsDelete = false;
            db.Files.Add(file);
            db.SaveChanges();

            return file;
        }
    }
}