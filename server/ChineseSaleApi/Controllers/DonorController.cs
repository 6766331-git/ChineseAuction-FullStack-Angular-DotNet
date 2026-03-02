using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using ChineseSaleApi.Repositories;
using ChineseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class DonorController : ControllerBase
    {
        private readonly IDonorService _service;

        public DonorController(IDonorService service)
        {
            _service = service;
        }

        // GET: api/Donor
        [HttpGet]
        [Route("GetAllDonors")]
        public async Task<ActionResult<IEnumerable<DonorDto>>> GetAllDonors()
        {
            try
            {
                var donors = await _service.GetAllDonorsAsync();
                return Ok(donors);
            }
            catch (Exception)
            {
                return StatusCode(500,"cannot get donors");
            }

        }

        [HttpPost]
[Route("AddDonor")]
public async Task<IActionResult> AddDonor([FromBody] DonorDto donorDto)
{
    if (!ModelState.IsValid)
        return BadRequest("Invalid data");

    try
    {
        var success = await _service.AddDonorAsync(donorDto);
        if (!success)
            return Conflict("Email already exists"); // מחזיר 409 במקום 400

        return Ok("Donor created successfully");
    }
    catch (Exception)
    {
        return StatusCode(500, "Cannot create donor");
    }
}


        // DELETE: api/Donor/{id}
        [HttpDelete]
        [Route("DeleteDonor/{id}")]
        public async Task<IActionResult> DeleteDonor(int id)
        {
            try
            {
                await _service.DeleteDonorByIdAsync(id);
                return Ok("donor deleted succesfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500,"cannot delete donor");
            }
        }

        // PUT: api/Donor/{id}
        [HttpPut]
[Route("UpdateDonor/{id}")]
public async Task<IActionResult> UpdateDonor(int id, [FromBody] DonorDto donorDto)
{
    try
    {
        await _service.UpdateDonorAsync(id, donorDto);
        return Ok("donor updated succesfuly");
    }
    catch (InvalidOperationException)
    {
        return Conflict("Email already exists"); // ✅ 409
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (Exception)
    {
        return StatusCode(500,"cannot update donor");
    }
}


        // GET: api/Donor/{id}
        [HttpGet]
        [Route("GetDonorGifts/{id}")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> GetDonorGifts(int id) {
            try
            {
                var gifts=await _service.GetDonorGiftsByIdAsync(id);
                return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500,"cannot get donor gifts");
            }
            

        }
        // GET: api/Donor/Filter
        [HttpGet]
        [Route("FilterDonors")]
        public async Task<ActionResult<IEnumerable<DonorDto>>> FilterDonors([FromQuery] FilterDonorDto parameter)
        {
            try
            {
                var donors = await _service.FilterDonorsAsync(parameter);
                return Ok(donors);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot filter donors");
            }
        }
    }
}
