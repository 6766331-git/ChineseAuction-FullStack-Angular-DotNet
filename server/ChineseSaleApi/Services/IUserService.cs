using ChineseSaleApi.Dto;

namespace ChineseSaleApi.Services
{
    public interface IUserService
    {
        public Task<BuyerDetailDto> CreateUserAsync(UserDto createDto);
        public Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto userDto);
        public Task<List<UserGiftDto>> GetAllGiftsAsync();
        public Task<List<UserGiftDto>> SortingGiftsAsync(string? sortParam);
        public Task AddGiftToCartAsync(int userId, int giftId);
        public Task<List<OrderItemDto>> GetCartItemsAsync(int userId);
        public Task DeleteItemFromCartAsync(int userId, int giftId);
        public Task LessAmountItemFromCartAsync(int userId, int giftId);
        public Task<string> CheckoutAsync(int userId);


    }
}
