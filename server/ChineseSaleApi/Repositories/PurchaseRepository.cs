using ChineseSaleApi.Data;
using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ChineseSaleApi.Repositories
{
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly ChineseSaleContextDb _context;
        public PurchaseRepository(ChineseSaleContextDb context)
        {
            _context = context;
        }
        // Implement purchase-related data access methods here

       //get all purchases with tickets and users
public async Task<List<Gift>> GetAllPurchasesAsync()
{
    return await _context.Gifts
        .Include(g => g.Tickets)         // כולל את כל הכרטיסים של המתנה
            .ThenInclude(t => t.User)    // כולל את המשתמשים של כל כרטיס
        .ToListAsync();
}

        //get buyers details by gift id
        public async Task<List<Ticket>> GetBuyersDetailsByGiftIdAsync(int giftId)
        {
            return await _context.Tickets
                .Where(t => t.GiftId == giftId)
                .Include(t => t.User)

                .ToListAsync();
        }
        public async Task<bool> GiftExistsAsync(int giftId)
        {
            return await _context.Gifts.AnyAsync(g => g.Id == giftId);
        }

        //update gift with winner user id
        public async Task UpdateGiftAsync(Ticket winner)
        {
            var gift = await _context.Gifts.FindAsync(winner.GiftId);
            if (gift == null)
            {
                throw new Exception("Gift not found");
            }
            gift.WinnerUserId = winner.UserId;
            await _context.SaveChangesAsync();
        }

        //Create a gifts and winners report
        public async Task<List<Gift>> MakeGiftsAndWinnersReportAsync()
        {
            return await _context.Gifts
                .Include(g => g.WinnerUser)
                .Include(g => g.Tickets) 
                .ToListAsync();
        }

        //Creating a Total Sales Revenue Report
        public async Task<int> MakeTotalSalesRevenueReportAsync()
        {
            return await _context.Tickets
                .SumAsync(t => t.Gift.TicketCost);
        }

        //sorting gifts
        public async Task<List<Gift>> SortingGiftsAsync(string? sortParam)
        {

            switch (sortParam)
            {
                case "price":
                    return await _context.Gifts
                        .OrderByDescending(g => g.TicketCost)
                        .ToListAsync();

                case "purchases":
                    return await _context.Gifts
                        .OrderByDescending(g => g.Tickets.Count())
                        .ToListAsync();

                default:
                    return await _context.Gifts.ToListAsync();
            }

        }

    }
}


