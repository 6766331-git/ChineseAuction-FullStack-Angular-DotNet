using ChineseSaleApi.Data;
using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace ChineseSaleApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ChineseSaleContextDb _context;
        public UserRepository(ChineseSaleContextDb context)
        {
            _context = context;
        }

        //register
        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.UserName == email);
        }

        //login
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == email);
        }

        //get all gifts from gift repository

        public async Task<List<Gift>> SortingGiftsAsync(string? sortParam)
        {
 IQueryable<Gift> query = _context.Gifts
                .Include(g => g.Category)
                .Include(g => g.Donor)
                .Include(g => g.WinnerUser);

            switch (sortParam)
            {
                case "price":
                    query = query.OrderByDescending(g => g.TicketCost);
                    break;

                case "category":
                    query = query.OrderByDescending(g => g.Category.Name); 
                    break;

                default:
                    break;
            }

            return await query.ToListAsync();
        }
        

        //add gift to cart

        public async Task<Gift?> GiftFindAsync(int giftId)
        {
            return await _context.Gifts.FindAsync(giftId);
        }
        public async Task AddGiftToCartAsync(int userId, Gift gift)
        {

            var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId);

            if (order == null)
            {
                order = new Order
                {
                    UserId = userId
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

            }
            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                GiftId = gift.Id,
                Price = gift.TicketCost,
            };
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
        }


        //get cart items
        public async Task<Order?> GetCartItemsAsync(int userId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                    .ThenInclude(i => i.Gift)
                .FirstOrDefaultAsync(o => o.UserId == userId);
            return order;

        }



        //delete item from cart

        public async Task<bool> IsGiftInCartAsync(int userId, int giftId)
        {
            var orderId = await _context.Orders
                .Where(o => o.UserId == userId)
                .Select(o => o.Id)
                .FirstOrDefaultAsync();

            return await _context.OrderItems.AnyAsync(i =>
                i.OrderId == orderId && i.GiftId == giftId
            );
        }


        public async Task<bool> DeleteItemFromCartAsync(int userId, int giftId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (order == null || order.Items.Count == 0)
                return false;


            var itemsToDelete = order.Items.Where(i => i.GiftId == giftId).ToList();
            _context.OrderItems.RemoveRange(itemsToDelete);

            await _context.SaveChangesAsync();
            return true;

        }

        //update item amount in cart
        public async Task<bool> LessAmountItemFromCartAsync(int userId, int giftId)
        {
            var order = await _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId);

            if (order == null || order.Items.Count == 0)
                return false;


            var itemToDelete = order.Items.Where(i => i.GiftId == giftId).FirstOrDefault();
            _context.OrderItems.Remove(itemToDelete);

            await _context.SaveChangesAsync();
            return true;
        }

        //checkout
        public async Task<List<string?>?> CheckoutAsync(int userId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Gift)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            if (order == null || order.Items.Count == 0)
                return null;

            var giftIdsInCart = order.Items.Select(i => i.GiftId).ToList();

            var unavailableGiftIds = order.Items
                .Where(i => i.Gift != null && i.Gift.WinnerUserId != null)
                .Select(i =>  i.Gift?.Name)
                .ToList();


            var purchasableGiftIds = order.Items
                .Where(i => i.Gift != null && i.Gift.WinnerUserId == null)
                .Select(i => i.GiftId)
                .ToList();


            using var tx = await _context.Database.BeginTransactionAsync();

            foreach (var giftId in purchasableGiftIds)
            {
                _context.Tickets.Add(new Ticket
                {
                    GiftId = giftId,
                    UserId = userId,
                    PurchaseDate = DateTime.UtcNow
                });

            }

            _context.OrderItems.RemoveRange(order.Items);
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
            await tx.CommitAsync();

            return unavailableGiftIds;
        }
    }
    }
