using ChineseSaleApi.Models;

namespace ChineseSaleApi.Dto
{
    public class PurchaseDto
    {
                public int Id { get; set; }

        public string GiftName { get; set; } = null!;
        public int Quantity { get; set; }
        public string WinnerName { get; set; } = null!;
    }
    public class PurchaseDetailDto
    {
        public BuyerDetailDto? User { get; set; }
        public DateTime? PurchaseDate { get; set; }

    }

    public class WinnersGiftsReportDto
    {
        public BuyerDetailDto? User { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string GiftName { get; set; } = null!;
        public string GiftDescription { get; set; } = null!;
    }

}
