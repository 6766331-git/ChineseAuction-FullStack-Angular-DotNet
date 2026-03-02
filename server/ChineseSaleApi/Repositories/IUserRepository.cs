using ChineseSaleApi.Models;
using static ChineseSaleApi.Repositories.UserRepository;

namespace ChineseSaleApi.Repositories
{
    public interface IUserRepository
    {
        public Task<User> CreateAsync(User user);
        public Task<bool> EmailExistsAsync(string email);
        public Task<User?> GetByEmailAsync(string email);
        public Task<List<Gift>> SortingGiftsAsync(string? sortParam);
        public Task AddGiftToCartAsync(int userId, Gift gift);
        public Task<Gift?> GiftFindAsync(int giftId);
        public Task<Order?> GetCartItemsAsync(int userId);
        public Task<bool> IsGiftInCartAsync(int userId, int giftId);
        public Task<bool> DeleteItemFromCartAsync(int userId, int giftId);
        public Task<bool> LessAmountItemFromCartAsync(int userId, int giftId);
        public Task<List<string?>?> CheckoutAsync(int userId);

    }
}
