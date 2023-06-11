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
    public class ChefCenterController : ControllerBase
    {
        public IGenericRepository<ChefCentre> ChefCentreRepository;



        private readonly AppSettings _appSettings;

        CancellationToken cancellation;
        private readonly IMapper _mapper;



        public ChefCenterController( IGenericRepository<ChefCentre> _ChefRepository, IMapper mapper)

        {
            ChefCentreRepository = _ChefRepository;
            cancellation = new CancellationToken();

            _mapper = mapper;
        }





        [HttpGet("GetChefs")]
        public IEnumerable<ChefCentre> getAllChefs()
        {
            return (new GetListGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetListGenericQuery<ChefCentre>(null, null), cancellation).Result);
        }

      


        [HttpGet("GetChef")]
        public ChefCentre getCheById(Guid Id)
        {
            return (new GetGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetGenericQuery<ChefCentre>(condition: x => x.UserId == Id, null), cancellation).Result);
        }

        [HttpGet("GetChefId")]
        public Guid getCheId(String Username)
        {

            var chefcenter = new GetGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetGenericQuery<ChefCentre>(condition: x => x.Username == Username, null), cancellation).Result;
            return (chefcenter.UserId);
        }

        [HttpPost("AjoutChef")]
        public async Task<ChefCentre> PostUser([FromBody] ChefCentre ChefCentre)
        {
            var hashsalt = EncryptPassword(ChefCentre.Password);
            ChefCentre.Password = hashsalt.Hash;
            ChefCentre.StoredSalt = hashsalt.Salt;

            var x = new AddGenericCommand<ChefCentre>(ChefCentre);
            var GenericHandler = new AddGenericHandler<ChefCentre>(ChefCentreRepository);
            return await GenericHandler.Handle(x, cancellation);
        }
        [HttpPut("UpdateChef")]
        public async Task<ChefCentre> PutUser([FromBody] ChefCentre ChefCentre)
        {
            var x = new PutGenericCommand<ChefCentre>(ChefCentre);
            var GenericHandler = new PutGenericHandler<ChefCentre>(ChefCentreRepository);
            return await GenericHandler.Handle(x, cancellation);
        }

        [HttpDelete("DeleteChef")]
        public async Task<ChefCentre> DeleteUser(Guid Id)
        {
            var x = new RemoveGenericCommand<ChefCentre>(Id);
            var GenericHandler = new RemoveGenericHandler<ChefCentre>(ChefCentreRepository);
            return await GenericHandler.Handle(x, cancellation);
        }



        [HttpPost("AffectChefToCenter")]
        public IActionResult AffectChefToCenter(Guid ChefCenterId, Guid CenterId)
        {             
            var chefcenter = (new GetGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetGenericQuery<ChefCentre>(condition: x => x.UserId == ChefCenterId, null), cancellation).Result);
            if (chefcenter != null)
            {
                chefcenter.CentreId = CenterId;
                PutUser(chefcenter);
                return Ok(chefcenter);
           }
            return BadRequest(new { message = "Chef Center Not Found" });

        }


        [HttpGet("GetCenterFomChef")]
        public Guid getRoleById(Guid Id)
        {
            var chef = (new GetGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetGenericQuery<ChefCentre>(condition: x => x.UserId == Id, null), cancellation).Result);
            return (chef.CentreId);
        }

        [HttpGet("GetNumberOfChefCenters")]
        public int GetNumberOfChefCenters()
        {
            IEnumerable<ChefCentre> users = (new GetListGenericHandler<ChefCentre>(ChefCentreRepository).Handle(new GetListGenericQuery<ChefCentre>(null, null), cancellation).Result);
            return users.Count();
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




    }
}
