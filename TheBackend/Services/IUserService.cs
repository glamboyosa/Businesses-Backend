using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheBackend.Models;
using TheBackend.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TheBackend.Services
   
{
   public interface IUserService
    {
       User AuthenticateAdmin(string username, string password);
        IEnumerable<User> GetAllUsers();
      User GetUsersById(long id);
    }
    
    public class UserService : IUserService
    {
        private readonly PayArenaMockContext _context;
        private readonly AppSettings _appSettings;
        private readonly IHashingService _hashingService;
        public UserService(IOptions<AppSettings> appSettings, PayArenaMockContext context, IHashingService hashingService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _hashingService = hashingService;
        }
        // private readonly IUserService _service;

        //public UserService()
        //{

        // //   _service = service;
        //}
        //seeding or hardcoding the admin user. Works
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var scope= serviceProvider.CreateScope())
            {
                using (var context = scope.ServiceProvider.GetRequiredService<PayArenaMockContext>())
                {
                    if(!context.User.Any())
                    {
                        string salt = SaltGenerator.SaltMethod();
                        HashingService hash = new HashingService();
                        context.User.Add(new User() { FirstName = "Uzoezi", LastName = "Ubiri", UserName = "Admin", Email="timothy.ogbemudia@up-ng.com", Role = Role.Admin, Token = "Buzz", Salt=salt, Password = hash.ComputeSha256Hash("Admin", salt) });
                        context.SaveChanges();
                    }
                    
                    //add a user
                    /*  if (!context.User.Any())
                      {
                          context.User.Add(new User() { FirstName = "Uzoezi", LastName = "Ubiri", UserName = "Admin", Password = "Admin", Role = Role.Admin });
                          context.SaveChanges();
                      }*/
                }
                
            }
        }
      /*  private List<User> _users = new List<User>
        {
        new User{Id=1, FirstName="Uzoezi", LastName="Ubiri",UserName="Admin", Password="Admin",Role=Role.Admin}
        };*/
    
        public   User AuthenticateAdmin(string username, string password)
        {
            var users = _context.User.FirstOrDefault(x => x.UserName == username);
            //return null if user is not found 
            if (users == null) return null;
            var salt = users.Salt;

            var verifyPassword = _hashingService.ComputeSha256Hash(password, salt);

             var user =  _context.User.FirstOrDefault((x => x.UserName == username && x.Password == verifyPassword));


            if (user == null)
            {
                return null;
            }
            //authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject= new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role,user.Role)
                }),
                Expires=DateTime.UtcNow.AddDays(7),
                SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            user.Password = null;
            return user;
        }
        public IEnumerable <User> GetAllUsers()
        {
           
          return _context.User.ToList();
        }
      public  User GetUsersById(long id)
        {
            var user = _context.User.FirstOrDefault(x => x.Id == id);

            //return user without password
            //if (user != null) user.Password = null;
            return  user;
        }
    }
}
