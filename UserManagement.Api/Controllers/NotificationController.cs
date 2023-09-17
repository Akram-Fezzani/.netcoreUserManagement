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
    public class NotificationController : ControllerBase
    {
        public IGenericRepository<Notification> Repository;



        CancellationToken cancellation;
        private readonly IMapper _mapper;



        public NotificationController(IGenericRepository<Notification> _Repository, IMapper mapper)

        {
            Repository = _Repository;
            cancellation = new CancellationToken();
            _mapper = mapper;
        }





        [HttpGet("GetNotifications")]
        public IEnumerable<Notification> getAllNotifications()
        {
            return (new GetListGenericHandler<Notification>(Repository).Handle(new GetListGenericQuery<Notification>(null, null), cancellation).Result);
        }

        [HttpGet("GetNotificationById")]
        public Notification getNotificationById(Guid Id)
        {
            return (new GetGenericHandler<Notification>(Repository).Handle(new GetGenericQuery<Notification>(condition: x => x.NotificationId == Id, null), cancellation).Result);
        }

        

        [HttpPost("AjoutNotification")]
        public async Task<Notification> PostNotification([FromBody] Notification Notification)
        {
            var x = new AddGenericCommand<Notification>(Notification);
            var GenericHandler = new AddGenericHandler<Notification>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }
        [HttpPut("UpdateNotification")]
        public async Task<Notification> PutNotification([FromBody] Notification Notification)
        {
            var x = new PutGenericCommand<Notification>(Notification);
            var GenericHandler = new PutGenericHandler<Notification>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }

        [HttpDelete("DeleteNotification")]
        public async Task<Notification> DeleteNotification(Guid Id)
        {
            var x = new RemoveGenericCommand<Notification>(Id);
            var GenericHandler = new RemoveGenericHandler<Notification>(Repository);
            return await GenericHandler.Handle(x, cancellation);
        }


        [HttpGet("GetNotificationByUser")]
        public List<Notification> GetNotificationByUser(Guid Id)
        {

            List<Notification> notifbyuser = new List<Notification>();
            IEnumerable<Notification> Notifications = (new GetListGenericHandler<Notification>(Repository).Handle(new GetListGenericQuery<Notification>(null, null), cancellation).Result);
            
                foreach (var n in Notifications)
                {
                    if (n.UseriD == Id)
                    {
                    notifbyuser.Add(n);

                    }
                }
            
            return notifbyuser;
        }

    }
}
