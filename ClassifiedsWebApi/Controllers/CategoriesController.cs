using Microsoft.AspNetCore.Mvc;
using ClassifiedsWebApi.Models;

namespace ClassifiedsWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    // In-memory storage for demonstration
    private static List<Category> categories = new()
    {
        new Category { Id = 1, Name = "Electronics", Description = "Electronic devices and gadgets" },
        new Category { Id = 2, Name = "Furniture", Description = "Home and office furniture" },
        new Category { Id = 3, Name = "Clothing", Description = "Apparel and accessories" }
    };

    /// <summary>
    /// Get all categories
    /// </summary>
    /// <returns>List of all categories</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Category>> GetAllCategories()
    {
        return Ok(categories);
    }

    /// <summary>
    /// Get a specific category by ID
    /// </summary>
    /// <param name="id">Category ID</param>
    /// <returns>The requested category</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Category> GetCategoryById(int id)
    {
        var category = categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }
        return Ok(category);
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="category">Category object to create</param>
    /// <returns>The created category</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Category> CreateCategory([FromBody] Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return BadRequest(new { message = "Category name is required" });
        }

        // Generate new ID
        category.Id = categories.Max(c => c.Id) + 1;
        category.CreatedAt = DateTime.UtcNow;
        category.UpdatedAt = DateTime.UtcNow;

        categories.Add(category);

        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    /// <summary>
    /// Update an existing category
    /// </summary>
    /// <param name="id">Category ID to update</param>
    /// <param name="category">Updated category object</param>
    /// <returns>The updated category</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Category> UpdateCategory(int id, [FromBody] Category category)
    {
        if (string.IsNullOrWhiteSpace(category.Name))
        {
            return BadRequest(new { message = "Category name is required" });
        }

        var existingCategory = categories.FirstOrDefault(c => c.Id == id);
        if (existingCategory == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        return Ok(existingCategory);
    }

    /// <summary>
    /// Delete a category
    /// </summary>
    /// <param name="id">Category ID to delete</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult DeleteCategory(int id)
    {
        var category = categories.FirstOrDefault(c => c.Id == id);
        if (category == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        categories.Remove(category);
        return NoContent();
    }
}
