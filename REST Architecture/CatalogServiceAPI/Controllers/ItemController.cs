using CatalogServiceAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogServiceAPI.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private static readonly List<Item> items = new List<Item> {
            new Item { Id = 1, Name = "item1", CategoryId = 1 },
            new Item { Id = 2, Name = "item2", CategoryId = 2 },
            new Item { Id = 3, Name = "item3", CategoryId = 3 },
        };

        [HttpGet()]
        public IEnumerable<Item> GetItems()
        {
            return items;
        }

        [HttpGet("{id}")]
        public ActionResult<Item> GetItem(long id)
        {
            var item = items.Find(el => el.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost()]
        public ActionResult<Item> Post(Item item)
        {
            items.Add(item);

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, Item updatedItem)
        {
            if (id != updatedItem.Id)
            {
                return BadRequest();
            }

            var item = items.Find(el => el.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            item.Name = updatedItem.Name;
            item.CategoryId = updatedItem.CategoryId;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var item = items.Find(el => el.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            items.Remove(item);

            return NoContent();
        }
    }
}
