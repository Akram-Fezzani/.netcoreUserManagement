using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
    public class UserStats
    {
        public IList<String> roles { get; set; }
        public IList<int> users { get; set; }

    }
}
