using System.ComponentModel.DataAnnotations;

namespace OnlineAuction.Models
{
    public class UserModel
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Имя обязательно.")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна.")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email обязателен.")]
        [EmailAddress(ErrorMessage = "Некорректный формат email.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль обязателен.")]
        [DataType(DataType.Password)]
        public string? PasswordHash { get; set; } 
        public DateTime? CreatedAt { get; set; } 
    }
}
