
using System.ComponentModel.DataAnnotations;

namespace WebApi.Dtos.Authorization
{
    public class RegisterRequestDTO
    {
        /// <summary>
        /// Username of User
        /// </summary>
        /// <example>john</example>
        /// <required>true</required>
        [Required(ErrorMessage = "User Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9]*$")]
        public string? Username { get; set; }

        /// <summary>
        /// Password of User
        /// </summary>
        /// <example>1234</example>
        /// <required>true</required>
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        /// <summary>
        /// Role of User
        /// </summary>
        /// <example>user</example>
        /// <required>true</required>
        public string? Role { get; set; }
    }
}