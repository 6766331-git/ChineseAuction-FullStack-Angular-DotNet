using ChineseSaleApi.Dto;

namespace ChineseSaleApi.Services
{
    public interface IPurchaseService
    {
        public Task<List<PurchaseDto>> GetAllPurchasesAsync();
        public Task<List<PurchaseDetailDto>> GetBuyersDetailsByGiftIdAsync(int giftId);
        public Task<string> MakeLottery(int giftId);
        public Task<List<WinnersGiftsReportDto>> WinnersGiftsReportsAsync();
        public Task<int> TotalSalesRevenueReportAsync();
        public Task<List<PurchaseDto>> SortingGiftsAsync(string? sortParam);

    }
}
