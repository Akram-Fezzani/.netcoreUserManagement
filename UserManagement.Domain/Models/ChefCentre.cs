using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
   public class ChefCentre : User
    {
        public Guid CentreId { get; set; }
    }
}
