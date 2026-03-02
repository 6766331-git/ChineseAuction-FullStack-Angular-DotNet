using ChineseSaleApi.Dto;
using ChineseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class GiftController : ControllerBase
    {
        private readonly IGiftService _service;
        public GiftController(IGiftService service)
        {
            _service = service;
        }

        // GET: api/Gift
        [HttpGet]
        [Route("GetAllGifts")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> GetAllGiftAsync()
        {
            try
            {
                var gifts = await _service.GetAllGiftsAsync();
                if (!gifts.Any())
                    return NotFound("you dont have gifts already");
                else
                    return Ok(gifts);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get gifts");
            }
        }

        
        // POST: api/Gift
        [HttpPost]
        [Route("AddGift")]
        public async Task<ActionResult> AddGift([FromBody] AddGiftDto giftDto)
        {
            try
            {
                await _service.AddGiftAsync(giftDto);
                return Ok("gift create succesfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot create gift");
            }

        }

        
        // PUT: api/Gift/{id}
        [HttpPut]
        [Route("UpdateGift/{id}")]
        public async Task<IActionResult> UpdateGift(int id, [FromBody] AddGiftDto giftDto)
        {
            try
            {
                await _service.UpdateGiftAsync(id, giftDto);
                return Ok("gift updated succesfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot update gift");
            }
        }

        
        // DELETE: api/Gift/{id}
        [HttpDelete]
        [Route("DeleteGift/{id}")]
        public async Task<IActionResult> DeleteGift(int id)
        {
            try
            {
                await _service.DeleteGiftByIdAsync(id);
                return Ok("gift deleted succesfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot delete gift");
            }
        }

        
        // PUT: api/Gift/AddPictureToGift/Picture/{id}
        [HttpPut]
        [Route("AddPictureToGift/Picture/{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddPictureToGift(int id, IFormFile? picture)
        {
            try
            {
                await _service.AddPictureToGiftAsync(id, picture);
                return Ok("gift picture added succesfuly");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot add gift picture");
            }
        }

        
        // GET: api/Gift/GetDonorByGiftId/{id}
        [HttpGet]
        [Route("GetDonorByGiftId/{id}")]
        public async Task<ActionResult<DonorDto>> GetGiftDonor(int id)
        {
            try
            {
                var donor = await _service.GetDonorByGiftIdAsync(id);
                return Ok(donor);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get the donor");
            }

        }
        [AllowAnonymous]
        //GET: api/Gift/SearchGifts
        [HttpGet]
        [Route("SearchGifts")]
        public async Task<ActionResult<IEnumerable<GiftDto>>> SearchGifts([FromQuery] SearchGiftDto parameter)
        {
            try
            {
                var gifts = await _service.SearchGiftsAsync(parameter);
                    return Ok(gifts);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot search gifts");
            }
        }
    }
}