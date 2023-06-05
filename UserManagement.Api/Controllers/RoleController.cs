using AutoMapper;
using UserManagement.Domain.Models;
using UserManagement.Domain.Commands;
using UserManagement.Domain.Handlers;
using UserManagement.Domain.Interfaces;
using UserManagement.Domain.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        public IGenericRepository<Role> Repository;



        CancellationToken cancellation;
        private readonly IMapper _mapper;



        public RoleController(IGenericRepository<Role> _Repository, IMapper mapper)

        {
            Repository = _Repository;
            cancellation = new CancellationToken();
            _mapper = mapper;
        }





        [HttpGet("GetRoles")]
        public IEnumerable<Role> getAllRoles()
        {
            return (new GetListGenericHandler<Role>(Repository).Handle(new GetListGenericQuery<Role>(null, null), cancellation).Result);
        }

        [HttpGet("GetRoleById")]
        public Role getRoleById(Guid Id)
        {
            return (new GetGenericHandler<Role>(Repository).Handle(new GetGenericQuery<Role>(condition: x => x.RoleId == Id, null), cancellation).Result);
        }

        [HttpGet("GetRoleByName")]
        public Role getRoleByName(String role)
        {
            return (new GetGenericHandler<Role>(Repository).Handle(new GetGenericQuery<Role>(condition: x => x.role == role, null), cancellation).Result);
        }

        [HttpPost("AjoutRole")]
        public async Task<Role> PostRole([FromBody] Role role)
        {
            var x = new AddGenericCommand<Role>(role);
            var GenericHandler = new AddGenericHandler<Role>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }
        [HttpPut("UpdateRole")]
        public async Task<Role> PutRole([FromBody] Role role)
        {
            var x = new PutGenericCommand<Role>(role);
            var GenericHandler = new PutGenericHandler<Role>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }

        [HttpDelete("DeleteRole")]
        public async Task<Role> DeleteRole(Guid Id)
        {
            var x = new RemoveGenericCommand<Role>(Id);
            var GenericHandler = new RemoveGenericHandler<Role>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }




    }
}
