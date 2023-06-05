using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
    public class UserLoginResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string AuthToken { get; set; }

    }
}
