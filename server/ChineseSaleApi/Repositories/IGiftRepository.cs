using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;

namespace ChineseSaleApi.Repositories
{
    public interface IGiftRepository
    {
        public Task<List<Gift>> GetAllGiftsAsync();
        public Task AddGiftAsync(Gift gift);
        public Task<bool> DonorExistsAsync(int donorId);
        public Task<bool> CategoryExistsAsync(int categoryId);
        public  Task<bool> GiftExistsAsync(int id);
        // public Task UpdateGiftAsync(Gift gift);
        public Task DeleteGiftAsync(Gift gift);
        public Task<Gift?> GiftFindAsync(int id);
        public Task AddPictureToGiftAsync(Gift gift, string pictureurl);
        public Task<Donor?> GetDonorByGiftIdAsync(int giftId);
        public Task<List<Gift>> SearchGiftsAsync(SearchGiftDto parameter);
                public  Task<Gift?> GetGiftByIdAsync(int id);
                        public  Task SaveChangesAsync();


    }
}
