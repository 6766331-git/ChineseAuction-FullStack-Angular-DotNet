using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;

namespace ChineseSaleApi.Repositories
{
    public interface IDonorRepository
    {
        Task<IEnumerable<Donor>> GetAllDonors();
        public Task AddDonor(Donor donor);
        public Task<bool> ExistsByEmailAsync(string email);
        public Task DeleteByIdAsync(Donor donor);
        public Task UpdateDonorAsync(Donor donor);
        public Task<List<Gift>> GetDonorGiftsByIdAsync(int donorId);
        public Task<bool> DonorExistsAsync(int id);
        public Task<Donor?> DonorFindAsync(int id);
        public  Task<IEnumerable<Donor>> FilterDonorsAsync(FilterDonorDto filter);
        Task<bool> ExistsByEmailAsync(string email, int excludeId);


    }
}
