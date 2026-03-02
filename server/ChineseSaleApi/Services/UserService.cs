using ChineseSaleApi.Data;
using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using ChineseSaleApi.Repositories;
using Microsoft.Extensions.Configuration;
using System.Linq;  

namespace ChineseSaleApi.Services
{
    public class UserService : IUserService
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _repo;
        private readonly IGiftRepository _repoGift;


        public UserService(IUserRepository repo, ITokenService tokenService, IConfiguration configuration, IGiftRepository repoGift)
        {
            _repo = repo;
            _tokenService = tokenService;
            _configuration = configuration;
            _repoGift = repoGift;
        }

        //register
        public async Task<BuyerDetailDto> CreateUserAsync(UserDto createDto)
        {
            if (await _repo.EmailExistsAsync(createDto.UserName))
            {
                throw new ArgumentException($"Email {createDto.UserName} is already registered.");
            }

            var user = new User
            {
                Name = createDto.Name,
                UserName = createDto.UserName,
                PasswordHash = HashPassword(createDto.Password),
                Phone = createDto.Phone,
                Role = UserStatus.User
            };

            var createdUser = await _repo.CreateAsync(user);
            var userDto = new BuyerDetailDto
            {
                UserName = createdUser.UserName,
                Name = createdUser.Name,
                Phone = createdUser.Phone,

            };
            return userDto;
        }
        private static string HashPassword(string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }


        //login
        public async Task<LoginResponseDto?> AuthenticateAsync(LoginRequestDto userDto)
        {
            var user = await _repo.GetByEmailAsync(userDto.UserName);

            if (user == null)
            {
                throw new KeyNotFoundException($"Login attempt failed: User not found for email {userDto.UserName}");
            }

            var hashedPassword = HashPassword(userDto.Password);
            if (user.PasswordHash != hashedPassword)
            {
                throw new ArgumentException($"Login attempt failed: Invalid password for email {userDto.UserName}");
            }

            var token = _tokenService.GenerateToken(user.Id, user.UserName, user.Name, user.Phone, user.Role.ToString());
            var expiryMinutes = _configuration.GetValue<int>("JwtSettings:ExpiryMinutes", 60);

            return new LoginResponseDto
            {
                Token = token,
                TokenType = "Bearer",
                ExpiresIn = expiryMinutes * 60,
                 Role = user.Role     ,       
                User = new BuyerDetailDto
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    Phone = user.Phone,

                }
            };
        }

        //get all gifts
       // בתוך UserService.cs
public async Task<List<UserGiftDto>> GetAllGiftsAsync()
{
    var gifts = await _repoGift.GetAllGiftsAsync();
    return gifts.Select(d => new UserGiftDto
    {
        Id = d.Id,
        Name = d.Name,
        TicketCost = d.TicketCost,
        Description = d.Description,
        PictureUrl = d.PictureUrl,
        Category = d.Category?.Name ?? string.Empty,
        DonorName = d.Donor?.Name ?? string.Empty,
        WinnerUser = d.WinnerUser != null ? d.WinnerUser.Name : null 
    }).ToList();
}
        //sorting gifts
        public async Task<List<UserGiftDto>> SortingGiftsAsync(string? sortParam)
        {
            var gifts = await _repo.SortingGiftsAsync(sortParam);
            if (gifts == null || !gifts.Any())
                throw new KeyNotFoundException("no gifts found");

            return gifts.Select(d => new UserGiftDto
            {
                Id = d.Id, 
                Name = d.Name,
                TicketCost = d.TicketCost,
                Description = d.Description,
                PictureUrl = d.PictureUrl,
                Category =  d.Category?.Name ?? string.Empty,
                DonorName = d.Donor?.Name ?? string.Empty,
                WinnerUser = d.WinnerUserId != null ? d.WinnerUser?.Name : null

            }).ToList();
        }


        //add gift to bag
        public async Task AddGiftToCartAsync(int userId, int giftId)
        {
            var gift = await _repo.GiftFindAsync(giftId);

            if (gift == null) throw new KeyNotFoundException("gift not found");

            if (gift.WinnerUserId != null) throw new ArgumentException("the gift has already been drawn in the lottery");

            await _repo.AddGiftToCartAsync(userId, gift);
        }


        //get cart items
public async Task<List<OrderItemDto>> GetCartItemsAsync(int userId)
{
    var order = await _repo.GetCartItemsAsync(userId);

    if (order == null || !order.Items.Any())
        return new List<OrderItemDto>(); // מחזיר רשימה ריקה במקום Exception

    return order.Items
        .GroupBy(i => i.GiftId)
        .Select(g => new OrderItemDto
        {
            GiftId = g.Key,
            GiftName = g.First().Gift?.Name,
            Price = g.First().Price,
            Quantity = g.Count(),
            TotalPrice = g.Count() * g.First().Price
        })
        .ToList();
}



        //delete item from cart

        public async Task DeleteItemFromCartAsync(int userId, int giftId)
        {
            var isExist = await _repo.IsGiftInCartAsync(userId, giftId);
            if (!isExist)
                throw new KeyNotFoundException("gift not found in cart");

            var isDelete=await _repo.DeleteItemFromCartAsync(userId, giftId);
            if (!isDelete)
                throw new ArgumentException("your cart is empty...");
            else
                return;
        }
        //update item amount in cart
        public async Task LessAmountItemFromCartAsync(int userId, int giftId)
        {
            var isExist = await _repo.IsGiftInCartAsync(userId, giftId);
            if (!isExist)
                throw new KeyNotFoundException("gift not found in cart");

            var isDelete = await _repo.LessAmountItemFromCartAsync(userId, giftId);
            if (!isDelete)
                throw new ArgumentException("your cart is empty...");
            else
                return;
        }


        //checkout
        public async Task<string> CheckoutAsync(int userId)
        {
            var result= await _repo.CheckoutAsync(userId);
            if (result==null)
                throw new ArgumentException("your cart is empty...");
            else
            if (result.Count > 0)
            {
                string msg = "you can not buy the fallowing items:\n";
                foreach (var name in result)
                    msg += name + "\n";

                msg += "but your another gifts checkout succsefuly";
                return msg;
            }
            else return ("your checkout succseded!");

        }




    }
}
