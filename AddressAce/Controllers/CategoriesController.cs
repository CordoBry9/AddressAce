using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using AddressAce.Helpers.Extensions;
using AddressAce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoriesController : ControllerBase
{

    private readonly ICategoryDTOService _categoryService;
    private string _userId => User.GetUserId()!; // [authorize] means userId cannot be null

    public CategoriesController(ICategoryDTOService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: "api/categories" -> returns the users categories
    [HttpGet]
    public async Task<ActionResult<CategoryDTO>> GetCategories()
    {

        IEnumerable<CategoryDTO> categories;
        try
        {
           categories = await _categoryService.GetCategoriesAsync(_userId);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
        return Ok(categories);
    }

    // GET: "api/categories/5" -> returns a category or 404
    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDTO>> GetCategoryById([FromRoute] int categoryId)
    {
        try
        {
            CategoryDTO? category = await _categoryService.GetCategoryByIdAsync(categoryId, _userId);
            if (category == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(category);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    //// POST: "api/categories" -> creates a category and returns the created category
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> CreateCategory([FromBody] CategoryDTO category)
    {
        try
        {
            CategoryDTO createdItem = await _categoryService.CreateCategoryAsync(category, _userId);
            return Ok(createdItem);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    //// PUT: "api/categories/5" -> updates the selected category and returns Ok
    [HttpPut("{Id:int}")] //ask why have to do :int
    public async Task<ActionResult> UpdateCategory([FromRoute] int Id, [FromBody] CategoryDTO categoryDTO)
    {


        if (Id != categoryDTO.Id)
            return BadRequest("ID mismatch");
        try
        {
            await _categoryService.UpdateCategoryAsync(Id, categoryDTO, _userId);
            return Ok();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }

    }

    // DELETE: "api/categories/5" -> deletes the selected category and returns NoContent
    [HttpDelete("{categoryId}")] //ask why 
    public async Task<ActionResult<CategoryDTO>> DeleteCategory([FromRoute] int categoryId)
    {
        try
        {
            await _categoryService.DeleteCategoryAsync(categoryId, _userId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }

    // POST: "api/categories/5/email" -> sends an email to category and returns Ok or BadRequest to indicate success or failure
    [HttpPost("{contactId}/email")]
    public async Task<ActionResult> EmailContact([FromRoute] int categoryId, [FromBody] EmailData emailData)
    {
        try
        {
            bool result = await _categoryService.EmailCategoryAsync(categoryId, emailData, _userId);
            if (result == true)
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to send email.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return Problem();
        }
    }
}