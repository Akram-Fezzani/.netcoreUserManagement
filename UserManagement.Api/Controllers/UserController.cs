using AutoMapper;
using UserManagement.Domain.Models;
using UserManagement.Domain.Commands;
using UserManagement.Domain.Handlers;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using UserManagement.Api.helper;
using System.Linq;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IGenericRepository<User> Repository;

        public IGenericRepository<Role> RoleRepository;


        private readonly AppSettings _appSettings;

        CancellationToken cancellation;
        private readonly IMapper _mapper;



        public UserController(IGenericRepository<User> _Repository,IMapper mapper, IGenericRepository<Role> _RoleRepository)

        {
            Repository = _Repository;
            RoleRepository = _RoleRepository;

            cancellation = new CancellationToken();

            _mapper = mapper;
        }





        [HttpGet("GetUsers")]
        public IEnumerable<User> getAllUsers()
        {
            return (new GetListGenericHandler<User>(Repository).Handle(new GetListGenericQuery<User>(null, null), cancellation).Result);
        }

        [HttpGet("getUserById")]
        public User getUserById(Guid Id)
        {
            return (new GetGenericHandler<User>(Repository).Handle(new GetGenericQuery<User>(condition: x => x.UserId == Id, null), cancellation).Result);
        }





        [HttpPost("EncryptPassword")]

        public HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; // Generate a 128-bit salt using a secure PRNG
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return new HashSalt { Hash = encryptedPassw, Salt = salt };
        }


        [HttpPost("AjoutUser")]
        public async Task<User> PostUser([FromBody] User user)
        {
            var hashsalt = EncryptPassword(user.Password);
            user.Password = hashsalt.Hash;
            user.StoredSalt = hashsalt.Salt;

            var x = new AddGenericCommand<User>(user);
            var GenericHandler = new AddGenericHandler<User>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }
        [HttpPut("UpdateUser")]
        public async Task<User> PutUser([FromBody] User User)
        {
            var x = new PutGenericCommand<User>(User);
            var GenericHandler = new PutGenericHandler<User>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }

        [HttpDelete("DeleteUser")]
        public async Task<User> DeleteUser(Guid Id)
        {
            var x = new RemoveGenericCommand<User>(Id);
            var GenericHandler = new RemoveGenericHandler<User>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }


        [HttpPost("VerifyPassword")]
        public bool VerifyPassword(string enteredPassword, byte[] salt, string storedPassword)
        {
            string encryptedPassw = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8
            ));
            return encryptedPassw == storedPassword;
        }


        [HttpPost("generateJwtToken")]
        private string generateJwtToken(User user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is the secret key to encrypt the auth jwt token");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



        [HttpPost("Login")]
        public IActionResult Login(string Username, string Password)
        {
            UserLoginResponse loggeduser = new UserLoginResponse() { };

            var user = (new GetGenericHandler<User>(Repository).Handle(new GetGenericQuery<User>(condition: x => x.Username == Username, null), cancellation).Result);
            if (user != null)
            {
                var isPasswordMatched = VerifyPassword(Password, user.StoredSalt, user.Password);
                if (isPasswordMatched)
                {
                    generateJwtToken(user);
                    loggeduser.Username = user.Username;
                    loggeduser.Id = user.UserId;
                    loggeduser.roles = getRoleById(user.RoleId);
                    //loggeduser.AuthAuthorities = "Admin";
                    loggeduser.accessToken = generateJwtToken(user);




 
                    return Ok(loggeduser);
                }
                else
                {
                    return BadRequest(new { message = " password is incorrect" });
                }
            }
            else
                return BadRequest(new { message = "User not found" });
        }

        [HttpGet("GetRoleById")]
        public String getRoleById(Guid Id)
        {
            var role = (new GetGenericHandler<Role>(RoleRepository).Handle(new GetGenericQuery<Role>(condition: x => x.RoleId == Id, null), cancellation).Result);
            return (role.role);
        }

        [HttpGet("GetNumberOfUsers")]
        public int GetNumberOfUsers()
        {
            IEnumerable<User> users = (new GetListGenericHandler<User>(Repository).Handle(new GetListGenericQuery<User>(null, null), cancellation).Result);
            return users.Count();
        }


        [HttpGet("GetNumberOfActiveUsers")]
        public int GetNumberOfActiveUsers()
        {
      var  s=0;
            IEnumerable<User> users = (new GetListGenericHandler<User>(Repository).Handle(new GetListGenericQuery<User>(null, null), cancellation).Result);
            foreach (var user in users)
            {
                if (user.state == true)
                {
                    s = s + 1;
                }
            }
            return (s); 
        }







        [HttpGet("GetNumberOfAdmins")]
        public int GetNumberOfAdmins()
        {
            var x = 0;
            IEnumerable<User> users = (new GetListGenericHandler<User>(Repository).Handle(new GetListGenericQuery<User>(null, null), cancellation).Result);
            foreach (var user in users)
            {
                if (getRoleById(user.RoleId) == "ADMIN")
                {
                    x = x + 1;
                }
            }
            return (x);
        }

    }
}
