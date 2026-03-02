namespace ChineseSaleApi.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int GiftId { get; set; }
        public Gift? Gift { get; set; }
        public int Price { get; set; }


    }
}
