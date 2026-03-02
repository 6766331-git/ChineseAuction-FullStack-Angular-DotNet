using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChineseSaleApi.Models
{
    public enum UserStatus { User = 0, Admin = 1 }

    public class User
    {
        public int Id { get; set; }

        [EmailAddress]
        public required string UserName { get; set; }
        public required string Name { get; set; }
        public required string PasswordHash { get; set; }
        [Phone]
        public string? Phone { get; set; }
        public UserStatus Role { get; set; } = UserStatus.User;
    }
}
