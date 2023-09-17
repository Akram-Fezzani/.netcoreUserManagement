using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
  public  class Notification
    {
        public Guid NotificationId { get; set; }
        public String content { get; set; }

        public Guid UseriD { get; set; }
        public Guid senderId { get; set; }
        public Boolean MyProperty { get; set; }
        public Boolean hovored { get; set; }
        public DateTime notifDate { get; set; }





    }
}
