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
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _service;
        public PurchaseController(IPurchaseService service)
        {
            _service = service;
        }

        //GET : api/Purchase
        [HttpGet]
        [Route("GetAllPurchases")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> GetAllPurchaseAsync()
        {
                try
                {
                    var purchases = await _service.GetAllPurchasesAsync();
                if (!purchases.Any())
                    return NotFound("you dont have purchases already");
                else
                    return Ok(purchases);
                }
                catch (Exception)
                {
                    return StatusCode(500, "cannot get purchases");
                }
        }

        //GET : api/Purchase/BuyersDetails/{giftId}
        [HttpGet]
        [Route("BuyersDetails/{giftId}")]
        public async Task<ActionResult<IEnumerable<PurchaseDetailDto>>> GetBuyersDetailsByGiftIdAsync(int giftId)
        {
            try
            {
                var buyersDetails = await _service.GetBuyersDetailsByGiftIdAsync(giftId);
                return Ok(buyersDetails);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get buyers details");
            }
        }
        //POST : api/Purchase/MakeLottery/{giftId}
        [HttpPost]
        [Route("MakeLottery/{giftId}")]
        public async Task<ActionResult<string>> MakeLottery(int giftId)
        {
            try
            {
                var winnerName = await _service.MakeLottery(giftId);
                return Ok(winnerName);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot make lottery");
            }
        }

        //GET : api/Purchase/WinnersGiftsReports
        [HttpGet]
        [Route("WinnersGiftsReports")]
        public async Task<ActionResult<IEnumerable<WinnersGiftsReportDto>>> WinnersGiftsReportsAsync()
        {
            try
            {
                var report = await _service.WinnersGiftsReportsAsync();
                    return Ok(report);
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get winners gifts report");
            }
        }

        //GET : api/Purchase/TotalSalesRevenueReport
        [HttpGet]
        [Route("TotalSalesRevenueReport")]
        public async Task<ActionResult<int>> TotalSalesRevenueReportAsync()
        {
            try
            {
                var totalRevenue = await _service.TotalSalesRevenueReportAsync();
                return Ok($"the total sales revenue is{totalRevenue}");
            }
            catch (KeyNotFoundException knf)
            {
                return NotFound(knf.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get total sales revenue report");
            }
        }

        //GET : api/Purchase/SortingGifts?sortParam=value
        [HttpGet]
        [Route("SortingGifts")]
        public async Task<ActionResult<IEnumerable<PurchaseDto>>> SortingGiftsAsync([FromQuery] string? sortParam)
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
    }
}


