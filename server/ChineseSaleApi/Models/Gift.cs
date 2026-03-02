using System.ComponentModel.DataAnnotations.Schema;

namespace ChineseSaleApi.Models
{
    public class Gift
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required int TicketCost { get; set; }
        public string? Description { get; set; }
        public string? PictureUrl { get; set; }
        public required int DonorId { get; set; }
        public Donor? Donor { get; set; }
        public required int CategoryId { get; set; }
        public Category? Category { get; set; }
        [ForeignKey("WinnerUser")]
        public int? WinnerUserId { get; set; }
        public User? WinnerUser { get; set; }
        public ICollection<Ticket> Tickets { get; set; }=new List<Ticket>();


    }
}
