using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using ChineseSaleApi.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChineseSaleApi.Services
{
    public class DonorService : IDonorService
    {
        private readonly IDonorRepository _repo;

        public DonorService(IDonorRepository repo)
        {
            _repo = repo;
        }

        // get all donors
        public async Task<IEnumerable<DonorDto>> GetAllDonorsAsync()
        {
            var donors = await _repo.GetAllDonors();
            return donors.Select(d => new DonorDto
            {
                Name = d.Name,
                Email = d.Email,
                Id=d.Id
            });
        }

        //add donor
       public async Task<bool> AddDonorAsync(DonorDto donorDto)
{
    if (await _repo.ExistsByEmailAsync(donorDto.Email))
        return false;

    var donor = new Donor
    {
        Name = donorDto.Name,
        Email = donorDto.Email
    };

    await _repo.AddDonor(donor);
    return true;
}

        //delete donor
        public async Task DeleteDonorByIdAsync(int id)
        {

           var exists= await _repo.DonorFindAsync(id);
              if (exists==null)
                 throw new KeyNotFoundException($"Donor with ID {id} not found.");

            await _repo.DeleteByIdAsync(exists);

        }

       public async Task UpdateDonorAsync(int id, DonorDto donorDto)
{
    var exists = await _repo.DonorExistsAsync(id);
    if (!exists)
        throw new KeyNotFoundException($"Donor with ID {id} not found.");

    // ✅ בדיקת כפילות אימייל (למעט התורם עצמו)
    if (await _repo.ExistsByEmailAsync(donorDto.Email, id))
        throw new InvalidOperationException("Email already exists");

    var donor = new Donor
    {
        Id = id,
        Name = donorDto.Name,
        Email = donorDto.Email
    };

    await _repo.UpdateDonorAsync(donor);
}



//get donor gifts by id
        public async Task<IEnumerable<GiftDto>> GetDonorGiftsByIdAsync(int id)
        {
            if (!await _repo.DonorExistsAsync(id))
                throw new KeyNotFoundException($"Donor with ID {id} not found.");

            var gifts = await _repo.GetDonorGiftsByIdAsync(id);

            return gifts.Select(g => new GiftDto
            {

                Id = g.Id,
                Donor = new DonorDto
                {
                    Id = g.Donor.Id,
                    Name = g.Donor.Name,
                    Email = g.Donor.Email
                },

                Name = g.Name,
                Description = g.Description,
                PictureUrl = g.PictureUrl,
                TicketCost = g.TicketCost,
                Category = new CategoryDto
                {
                    Name = g.Category?.Name ?? string.Empty
                }
            });
        
        }

        //filter donors
        public async Task<List<DonorDto>> FilterDonorsAsync(FilterDonorDto parameter)
        {
            var donors = await _repo.FilterDonorsAsync(parameter);
            if (donors == null || !donors.Any())
                throw new KeyNotFoundException("no donors found");
            return donors.Select(d => new DonorDto
            {
                  Id = d.Id,
                Name = d.Name,
                Email = d.Email,
            }).ToList();
        }

    }
}

