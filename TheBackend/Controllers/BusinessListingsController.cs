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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessListingsController : ControllerBase
    {
        private readonly PayArenaMockContext _context;

        public BusinessListingsController(PayArenaMockContext context)
        {
            _context = context;
        }

        // GET: api/BusinessListings
        [HttpGet]
        //[AllowAnonymous]
       
       
        public async Task<ActionResult<IEnumerable<BusinessListing>>> GetBusinessListing()
        {

            //var businesslisting = _context.BusinessListing.Include(b => b.CategoryNameNav);
            var businesslisting = await _context.BusinessListing.ToListAsync()
           ;
            return Ok( businesslisting);
        }

        // GET: api/BusinessListings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BusinessListing>> GetBusinessListing(long id)
        {
            var businessListing = await _context.BusinessListing.FindAsync(id);

            if (businessListing == null)
            {
                return NotFound();
            }

            return Ok(businessListing);
        }

        // PUT: api/BusinessListings/5 Create
        // PUT: api/Todo/5
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<ActionResult<BusinessListing>> PutTodoItem(long id, BusinessListing model)
        {
         //   var id = model.Id;

            var customerName = model.CustomerName;
            var emailAddress = model.Email;
            var businessName = model.BusinessName;
            var businessAddress = model.Address;
            var city = model.City;
            var lga = model.Lga;
            var url = model.Url;
            var businessDescription = model.Description;
            var categoryId = model.CategoryId;

           var categories = _context.Categories.Where(m => m.Id == categoryId);
            var businesslisting = _context.BusinessListing.FirstOrDefault(m => m.Id == id);
                if (model != null)
                {
                businesslisting.CustomerName = model.CustomerName;
                businesslisting.Email = model.Email;
                businesslisting.BusinessName = model.BusinessName;
                businesslisting.Address = model.Address;
                businesslisting.City = model.City;
                businesslisting.Lga = model.Lga;
                businesslisting.Url = model.Url;
                businesslisting.Description = model.Description;
                    _context.Update(businesslisting);
                    await _context.SaveChangesAsync();
                }
            
           
           
           /* if (id != model.Id)
            {
                return BadRequest();
            }*/

            // _context.Entry(model).State = EntityState.Modified;
          

            return Ok();
        }

        // POST: api/BusinessListings
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<BusinessListing>> PostBusinessListing([FromBody]BusinessListing model)
        {
            var customerName = model.CustomerName;
            var emailAddress = model.Email;
            var businessName = model.BusinessName;
            var businessAddress = model.Address;
            var city = model.City;
            var lga = model.Lga;
            var url = model.Url;
            var businessDescription = model.Description;
            var categoryId = model.CategoryId;
            var categories = _context.Categories.Where(m => m.Id == categoryId);
            var businesslisting = _context.BusinessListing.Where(m => m.BusinessName == model.BusinessName).ToList();
            if (businesslisting.Any()|| !businesslisting.Any())
           // if(!businesslisting.Any())
            {
                var Businesslisting = new BusinessListing()
                {
                    CustomerName = customerName,
                    Email = emailAddress,
                    BusinessName = businessName,
                    Address = businessAddress,
                    City = city,
                    Lga = lga,
                    Url = url,
                    Description = businessDescription,
                   CategoryId=categoryId
                };
                _context.BusinessListing.Add(Businesslisting);
                await _context.SaveChangesAsync();
                return Ok();
            }


            return BadRequest();
        }

        // DELETE: api/BusinessListings/5
        
        [HttpDelete("{id}")]
        public async Task<ActionResult<BusinessListing>> DeleteBusinessListing(long id)
        {
            var businessListing = await _context.BusinessListing.FindAsync(id);
            if (businessListing == null)
            {
                return NotFound();
            }

            _context.BusinessListing.Remove(businessListing);
            await _context.SaveChangesAsync();

            return Ok(businessListing);
        }

        private bool BusinessListingExists(long id)
        {
            return _context.BusinessListing.Any(e => e.Id == id);
        }
    }
}
