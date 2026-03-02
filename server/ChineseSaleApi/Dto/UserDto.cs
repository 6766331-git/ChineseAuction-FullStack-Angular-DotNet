using System.ComponentModel.DataAnnotations;
using ChineseSaleApi.Models;
namespace ChineseSaleApi.Dto
{
    public class UserDto
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string UserName { get; set; }

        [Required]
        public required string Name { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [MaxLength(15, ErrorMessage = "Password cant more than 15 characters")]
        [Required]
        public required string Password { get; set; }

        
       
        [Phone]
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone must be at least 10 digits")]
        public string? Phone { get; set; }
    }

    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public required string UserName { get; set; }

        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; }
        public BuyerDetailDto User { get; set; } = null!;
         public UserStatus Role { get; set; }

    }
    public class BuyerDetailDto
    {
        public required string UserName { get; set; }
        public required string Name { get; set; }
        public string? Phone { get; set; }

    }


}
