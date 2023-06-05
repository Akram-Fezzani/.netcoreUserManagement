using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
   public class Role
    {
        public Guid RoleId { get; set; }
        public String role { get; set; }
        public IList<User> users { get; set; }


    }
}
