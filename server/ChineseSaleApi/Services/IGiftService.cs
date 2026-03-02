using ChineseSaleApi.Dto;

namespace ChineseSaleApi.Services
{
    public interface IGiftService
    {
        public Task<List<GiftDto>> GetAllGiftsAsync();
        public Task AddGiftAsync(AddGiftDto giftDto);
        public Task UpdateGiftAsync(int id, AddGiftDto giftDto);
        public Task DeleteGiftByIdAsync(int id);
        public Task AddPictureToGiftAsync(int giftId, IFormFile? picture);
        public Task<DonorDto> GetDonorByGiftIdAsync(int giftId);
        public Task<List<GiftDto>> SearchGiftsAsync(SearchGiftDto parameter);

    }
}
