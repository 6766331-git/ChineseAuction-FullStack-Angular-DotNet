using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;

namespace ChineseSaleApi.Services
{
    public interface IDonorService
    {
        public Task<IEnumerable<DonorDto>> GetAllDonorsAsync();
        public Task<bool> AddDonorAsync(DonorDto donorDto);
        public Task DeleteDonorByIdAsync(int id);
        public  Task UpdateDonorAsync(int id, DonorDto donorDto);
        public Task<IEnumerable<GiftDto>> GetDonorGiftsByIdAsync(int id);
        public Task<List<DonorDto>> FilterDonorsAsync(FilterDonorDto parameter);

    }
}
