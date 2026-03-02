using ChineseSaleApi.Dto;
using ChineseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChineseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]


    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        // GET: api/User/Gift
        [HttpGet]
        [Route("GetAllGifts")]
        public async Task<ActionResult<IEnumerable<UserGiftDto>>> GetAllGiftAsync()
        {
            try
            {
                var gifts = await _service.GetAllGiftsAsync();
                if (!gifts.Any())
                    return NotFound("we dont have gifts already");
                else
                    return Ok(gifts);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get gifts");
            }
        }

        [AllowAnonymous]
        //GET : api/User/SortingGifts?sortParam=value
        [HttpGet]
        [Route("SortingGifts")]
        public async Task<ActionResult<IEnumerable<UserGiftDto>>> SortingGiftsAsync([FromQuery] string? sortParam)
        {
            try
            {
                var sortedGifts = await _service.SortingGiftsAsync(sortParam);
                if (!sortedGifts.Any())
                    return NotFound("no gifts found");
                else
                    return Ok(sortedGifts);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot sort gifts");
            }
        }

        // POST: api/User/AddGiftToCart?giftId=value
        [HttpPost]
        [Route("AddGiftToCart")]
        public async Task<IActionResult> AddGiftToCartAsync([FromQuery] int giftId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            try
            {
                await _service.AddGiftToCartAsync(userId, giftId);
                return Ok("gift added to cart succesfuly");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (ArgumentException arg)
            {
                return BadRequest(arg.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot add gift to cart");
            }


        }

        // GET: api/User/GetCartItems
[HttpGet]
[Route("GetCartItems")]
public async Task<ActionResult<List<OrderItemDto>>> GetCartItems()
{
    // שולף את userId מתוך ה-Token
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
    if (userIdClaim == null)
        return Unauthorized("User ID not found in token.");

    int userId = int.Parse(userIdClaim.Value);

    try
    {
        var cartItems = await _service.GetCartItemsAsync(userId);
        return Ok(cartItems);
    }
    catch (ArgumentException arg)
    {
        return BadRequest(arg.Message);
    }
    catch (Exception)
    {
        return StatusCode(500, "cannot get cart items");
    }
}


        //DELETE : api/User/RemoveGiftFromCart?giftId=value
        [HttpDelete]
        [Route("RemoveGiftFromCart")]
        public async Task<ActionResult> RemoveGiftFromCartAsync([FromQuery] int giftId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());
            try
            {
                await _service.DeleteItemFromCartAsync(userId, giftId);
                return Ok("the gift deleted succesfuly");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (ArgumentException arg)
            {
                return BadRequest(arg.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot remove gift from cart");
            }
        }

        //PUT :api/User/UpdateAmountItemInCartAsync?giftId=value
        [HttpPut]
        [Route("UpdateAmountItemInCartAsync")]
        public async Task<ActionResult> LessAmountItemFromCartAsync([FromQuery] int giftId)
        {
            var userId = int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());
            try
            {
                await _service.LessAmountItemFromCartAsync(userId, giftId);
                return Ok("the gift amount lesses succesfuly");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (ArgumentException arg)
            {
                return BadRequest(arg.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot less amount gift from cart");
            }
        }

        //PUT :api/User/Checkout
        [HttpPost]
        [Route("Checkout")]
        public async Task<ActionResult<string>> Checkout()
        {
            var userId = int.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!.Value.ToString());

            try
            {
                var res = await _service.CheckoutAsync(userId);
                return Ok(res);
            }
            catch (ArgumentException arg)
            {
                return BadRequest(arg.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "cannot checkout your cart");
            }

        }
    }
}
