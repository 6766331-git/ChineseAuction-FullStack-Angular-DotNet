using ChineseSaleApi.Models;

namespace ChineseSaleApi.Dto
{
    public class OrderItemDto
    {
    public int GiftId { get; set; }    

        public string? GiftName { get; set; }
        public int TotalPrice { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
 
    }
}
