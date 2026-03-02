using ChineseSaleApi.Dto;
using ChineseSaleApi.Models;
using ChineseSaleApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChineseSaleApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        //GET : api/Category
        [HttpGet]
        [Route("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _service.GetAllCategoriesAsync();
                    return Ok(categories);
            }
            catch(KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot get categories");
            }
        }
        //POST : api/Category/AddCategory
        [HttpPost]
        [Route("AddCategory")]
        public async Task<ActionResult> AddCategoryAsync([FromBody] CategoryDto categoryName)
        {
            try
            {
                await _service.AddCategoryAsync(categoryName);
                return Ok("category added successfully");
            }
            catch (ArgumentException argEx)
            {
                return BadRequest(argEx.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "cannot add category");
            }
        }

                //DELETE : api/Category/DeleteCategory/5
[HttpDelete]
[Route("DeleteCategory/{id}")]
public async Task<ActionResult> DeleteCategoryAsync(int id)
{
    try
    {
        await _service.DeleteCategoryAsync(id);
        return Ok("category deleted successfully");
    }
    catch(KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch(Exception)
    {
        return StatusCode(500, "cannot delete category");
    }
}

//PUT : api/Category/UpdateCategory
[HttpPut]
[Route("UpdateCategory")]
public async Task<ActionResult> UpdateCategoryAsync([FromBody] CategoryDto categoryDto)
{
    try
    {
        await _service.UpdateCategoryAsync(categoryDto);
        return Ok("category updated successfully");
    }
    catch(KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch(Exception)
    {
        return StatusCode(500, "cannot update category");
    }
}


    }
}
