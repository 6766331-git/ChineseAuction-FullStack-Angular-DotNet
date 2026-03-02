using System.ComponentModel.DataAnnotations;

namespace ChineseSaleApi.Models
{
    public class Donor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
       
        [EmailAddress]
        public required string Email { get; set; }
        public ICollection<Gift> Gifts { get; set; } = new List<Gift>();


    }
}
