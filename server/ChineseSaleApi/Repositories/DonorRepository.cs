using ChineseSaleApi.Data;
using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChineseSaleApi.Repositories
{
    public class DonorRepository : IDonorRepository
    {
        private readonly ChineseSaleContextDb _context;
        public DonorRepository(ChineseSaleContextDb context)
        {
            _context = context;
        }

        // Implement donor-related data access methods here

        //get all donors
        public async Task<IEnumerable<Donor>> GetAllDonors()
        {
            return await _context.Donors.ToListAsync();
        }

        //add donor
        public async Task AddDonor(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
        }


        public Task<bool> ExistsByEmailAsync(string email)
        {
            return _context.Donors.AnyAsync(d => d.Email == email);
        }

        //delete donor
        public async Task<Donor?> DonorFindAsync(int id)
        {
            return await _context.Donors.FindAsync(id);
        }
        public async Task DeleteByIdAsync(Donor donor)
        {
              _context.Donors.Remove(donor);
              await _context.SaveChangesAsync();
        }

        //update donor

        public async Task<bool> DonorExistsAsync(int id)
        {
            return await _context.Donors.AnyAsync(d => d.Id == id);
        }

        public async Task UpdateDonorAsync(Donor donor)
        {
            _context.Donors.Update(donor);
            await _context.SaveChangesAsync();
        }

//get donor gifts by id
        public async Task<List<Gift>> GetDonorGiftsByIdAsync(int donorId)
        {
            return await _context.Gifts
                .Where(g => g.DonorId == donorId)
                .Include(g => g.Category)
                .Include(g => g.Donor) 
                .ToListAsync();
        }
//////////////
        public async Task<bool> ExistsByEmailAsync(string email, int excludeId)
{
    return await _context.Donors
        .AnyAsync(d => d.Email == email && d.Id != excludeId);
}

        //filter donors
     // בתוך DonorRepository.cs

public async Task<IEnumerable<Donor>> FilterDonorsAsync(FilterDonorDto filter)
{
    var query = _context.Donors.AsQueryable();

    // סינון לפי שם (אם נשלח)
    if (!string.IsNullOrWhiteSpace(filter.Name))
    {
        query = query.Where(d => d.Name.Contains(filter.Name));
    }

    // סינון לפי אימייל (אם נשלח)
    if (!string.IsNullOrWhiteSpace(filter.Email))
    {
        query = query.Where(d => d.Email.Contains(filter.Email));
    }

    if (filter.GiftId.HasValue)
    {
        query = query.Where(d => d.Gifts.Any(g => g.Id == filter.GiftId.Value));
    }

    // ביצוע השאילתה והחזרה
    return await query.ToListAsync();
}
    
}
}