using ChineseSaleApi.Models;
using System.ComponentModel.DataAnnotations;

namespace ChineseSaleApi.Dto
{
    public class DonorDto
    {
    public int Id { get; set; } 

        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public required string Email { get; set; }

    }

    public class FilterDonorDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }

    public int? GiftId { get; set; }   
    }
}
