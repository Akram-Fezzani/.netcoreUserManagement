using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
    public class DirecteurAntenne : User
    {
        public Guid AntennaId { get; set; }

    }
}
