using CatalogServiceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private static readonly List<Category> categories = new List<Category> { 
            new Category { Id = 1, Name = "category1" },
            new Category { Id = 2, Name = "category2" },
            new Category { Id = 3, Name = "category3" },
        };

        [HttpGet()]
        public IEnumerable<Category> GetCategories() {
            return categories;
        }

        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(long id) {
            var category = categories.Find(el => el.Id == id);

            if (category == null) {
                return NotFound();
            }

            return category;
        }

        [HttpPost()]
        public ActionResult<Item> Post(Category category)
        {
            categories.Add(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id}, category);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, Category updatedCategory)
        {
            if (id != updatedCategory.Id)
            {
                return BadRequest();
            }

            var category = categories.Find(el => el.Id == id);

            if (category == null) {
                return NotFound(); 
            }

            category.Name = updatedCategory.Name;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var category = categories.Find(el => el.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            categories.Remove(category);

            return NoContent();
        }
    }
}
