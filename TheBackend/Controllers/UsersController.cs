using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TheBackend.Services;
using TheBackend.Models;
using System.Web;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.EntityFrameworkCore;

namespace TheBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly PayArenaMockContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IHashingService _hashingService;
        public UsersController(IUserService userService, PayArenaMockContext context,IEmailSender emailSender,IHashingService hashingService)
        {
            _userService = userService;
            _context = context;
            _emailSender = emailSender;
            _hashingService = hashingService;
        }

        //allowanonoymous route to override the authorize route
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User user)
        {
          
            var users = _userService.AuthenticateAdmin(user.UserName, user.Password);
            if (users == null)
            {
                return BadRequest("User inputted wrong username & password");
            }
            return Ok(users);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(long id)
        {
            var users = await _context.User.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }
        [AllowAnonymous]
        [HttpGet("get/{id}")]
        public async Task<ActionResult<User>> GetByIdd(long id)
        {
            var users = await _context.User.FindAsync(id);

            if (users == null)
            {
                return NotFound();
            }

            return Ok(users);
        }

        //[Authorize(Roles = Role.Admin)]
     
        [HttpGet]
        public IActionResult Get()
        {
            var users = _userService.GetAllUsers().ToList();
            return Ok(users);
        }
        [AllowAnonymous]
        [HttpPost("create")]
        public async Task<ActionResult<User>> Create(User model)
        {
            RandomGenerate random = new RandomGenerate();
            string str = RandomGenerate.CreateRandomPasswordWithRandomLength();
            var firstName = model.FirstName;
            var lastName = model.LastName;
            var userName = model.UserName;
            var role = model.Role;
            var token = model.Token;
            var email = model.Email;
            var salt = SaltGenerator.SaltMethod();
            var password = _hashingService.ComputeSha256Hash(str, salt);
            var user = _context.User.Where(x => x.FirstName == model.FirstName).ToList();
            if (user.Any() || !user.Any())
            {
                var users = new User()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    UserName = userName,
                    Password = password,
                    Role = role,
                    Email = email,
                    Salt = salt

                };

                _context.User.Add(users);

                await _context.SaveChangesAsync();
                //var callback = Url.Page("/localhost:3000"
                //   , pageHandler: null,
                //   values: new { UserId = model.UserName },
                //   protocol: Request.Scheme
                //   );
                var callback = "http://localhost:3000/users/changepassword/" + users.Id ;
                //ussername === email
                await _emailSender.SendEmailAsync(model.Email, "Change Your Password",
                   $"<p>hi {model.UserName}, your username & password is: <br> Username: {model.UserName} Password:{str}</p><br><p>Please change your password by <a href='{HtmlEncoder.Default.Encode(callback)}'>clicking here</a></p>.");
                return Ok();
            }
            return BadRequest();
        }
        //}
      [AllowAnonymous]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<User>> Update(User model, long id)
        {

            var firstName = model.FirstName;
            var lastName = model.LastName;
            var userName = model.UserName;
            var password = model.Password;
            var role = model.Role;
            var token = model.Token;

            var users = _context.User.FirstOrDefault(m =>m.Id == id);
            if (model != null)
            {
                users.FirstName = model.FirstName;
                users.LastName = model.LastName;
                users.Role = model.Role;
                users.UserName = model.UserName;
             
                _context.Update(users);
             //   _context.Entry(User).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
 
        }
        [AllowAnonymous]
        [HttpPut("passwordupdate/{id}")]
        public async Task<ActionResult<User>> PasswordUpdate(User model, long id)
        {

            var firstName = model.FirstName;
            var lastName = model.LastName;
            var userName = model.UserName;
            var password = model.Password;
            

            var users = _context.User.FirstOrDefault(m => m.Id == id);
            var salt = users.Salt;
            if (model != null)
            {
                users.Password = _hashingService.ComputeSha256Hash(password, salt);
                _context.Update(users);
                //   _context.Entry(User).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();

        }

       [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(long id)
        {
            var usr = await _context.User.FindAsync(id);
            if (usr == null)
            {
                return NotFound("Terrrible request. Something obvs broke");
            }

            _context.User.Remove(usr);
            await _context.SaveChangesAsync();
            return Ok();


        }
    }
       
    
   
      
}