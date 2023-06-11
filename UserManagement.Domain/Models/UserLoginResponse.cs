using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
    public class UserLoginResponse
    {
        public Guid Id { get; set; }
        public string type { get; set; }
        public string Username { get; set; }
        public string accessToken { get; set; }

        public string roles { get; set; }







    }
}
