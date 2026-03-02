using ChineseSaleApi.Data;
using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace ChineseSaleApi.Repositories
{
    public class GiftRepository : IGiftRepository
    {
        private readonly ChineseSaleContextDb _context;
        public GiftRepository(ChineseSaleContextDb context)
        {
            _context = context;
        }

//get all gifts
        public async Task<List<Gift>> GetAllGiftsAsync()
        {
            return await _context.Gifts
            .Include(g => g.Category)
            .Include(g=>g.Donor)
            .Include(g=>g.WinnerUser)
            .ToListAsync();
        }


        //add gift

        public async Task AddGiftAsync(Gift gift)
        {
            _context.Gifts.Add(gift);
            await _context.SaveChangesAsync();
        }

        public Task<bool> DonorExistsAsync(int donorId) =>
             _context.Donors.AnyAsync(d => d.Id == donorId);

        public Task<bool> CategoryExistsAsync(int categoryId) =>
            _context.Categories.AnyAsync(c => c.Id == categoryId);

        //update gift

        public async Task<bool> GiftExistsAsync(int id)
        {
            return await _context.Gifts.AnyAsync(d => d.Id == id);
        }
           
//update gift
        public async Task<Gift?> GetGiftByIdAsync(int id)
        {
            return await _context.Gifts.FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        //delete gift
        public async Task<Gift?> GiftFindAsync(int id)
        {
            return await _context.Gifts.FindAsync(id);
        }
        public async Task DeleteGiftAsync(Gift gift)
        {
            _context.Gifts.Remove(gift);
            await _context.SaveChangesAsync();

        }

        //add picture to gift
        public async Task AddPictureToGiftAsync(Gift gift, string pictureurl)
        {
                gift.PictureUrl = pictureurl;
                _context.Gifts.Update(gift);
                await _context.SaveChangesAsync(); 
        }

        //get donor by gift id
        public async Task<Donor?> GetDonorByGiftIdAsync(int giftId)
        {
            var gift = await _context.Gifts
                .Include(g => g.Donor)
                .FirstOrDefaultAsync(g => g.Id == giftId);
            return gift?.Donor;
        }
        //gift search
        public async Task<List<Gift>> SearchGiftsAsync(SearchGiftDto parameter)
        {
            var query = _context.Gifts.Include(g => g.Donor).Include(g => g.Donor)  
        .Include(g => g.WinnerUser).AsQueryable();
            if (!string.IsNullOrEmpty(parameter.Name))
            {
                query = query.Where(g => g.Name.Contains(parameter.Name));
            }
            if (!string.IsNullOrEmpty(parameter.DonorName))
            {
                query = query.Where(g => g.Donor.Name.Contains(parameter.DonorName));
            }
            if (parameter.NumOfTickets.HasValue)
            {
                query = query.Where(g => g.Tickets.Count() == parameter.NumOfTickets);
            }

            return await query.ToListAsync();
        }

    }
}


    






