using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;

namespace WebApi.Models
{
    public class UserMetadata { }

    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        // Create Action
        public static User Create(EmployeeContext db, User user)
        {
            user.Role = user.Role;
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
            user.IsDelete = false;
            db.Users.Add(user);
            db.SaveChanges();

            return user;
        }

        //Get All Action
        public static List<User> GetAll(EmployeeContext db)
        {
            List<User> users = db.Users.Where(q => q.IsDelete != true).ToList();
            return users;
        }

        //Get ID Action
        public static User GetById(EmployeeContext db, int id)
        {
            User? result = db.Users.Where(q => q.Id == id && q.IsDelete != true).FirstOrDefault();
            return result ?? new User();
        }

        //Update Action
        public static User Update(EmployeeContext db, User user)
        {
            user.UpdateDate = DateTime.Now;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return user;
        }

        //Delete Action
        public static User Delete(EmployeeContext db, int id)
        {
            User user = GetById(db, id);
            user.IsDelete = true;
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();

            return user;
        }


    }


}