using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBackend.Models;

namespace TheBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly PayArenaMockContext _context;

        public CategoriesController(PayArenaMockContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {
            var cat= await _context.Categories.ToListAsync();
            return Ok(cat);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categories>> GetCategories(long id)
        {
            var categories =  _context.Categories.FirstOrDefault(e => e.Id == id);
            if (categories != null)
            {
                return Ok(categories);
            }
          
            if (categories == null)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        // PUT: api/Categories/5
        [AllowAnonymous]
        [HttpPut("{id}")]

        public async Task<IActionResult> PutCategories(long id, Categories model)
        {
            
            var categoryId = model.CategoryName;
            var categories = _context.Categories.FirstOrDefault(m => m.Id == id);
            if (model != null)
            {
                categories.CategoryName = model.CategoryName;

                _context.Update(categories);
                 await _context.SaveChangesAsync(); 
                
                return Ok();
            }
            return BadRequest();
        }

        // POST: api/Categories
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Categories>> PostCategories(Categories model)
        {
            var categoryName = model.CategoryName;
            var categories = _context.Categories.Where(m => m.CategoryName == model.CategoryName);
            if(categories.Any() || !categories.Any())
            {
                var Categories = new Categories()
                {
                   CategoryName= categoryName
                };
                _context.Categories.Add(Categories);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        // DELETE: api/Categories/5
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categories>> DeleteCategories(long id)
        {
            var categories = _context.Categories.FirstOrDefault(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(categories);
            await _context.SaveChangesAsync();

            return categories;
        }

        private bool CategoriesExists(string id)
        {
            return _context.Categories.Any(e => e.CategoryName == id);
        }
    }
}
