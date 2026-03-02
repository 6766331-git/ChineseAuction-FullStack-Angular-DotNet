namespace ChineseSaleApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public required int GiftId { get; set; }
        public Gift? Gift { get; set; }
        public required int UserId { get; set; }
        public User? User { get; set; }
        public DateTime? PurchaseDate { get; set; }

    }
}
