using System;
using System.Collections.Generic;
using System.Text;

namespace UserManagement.Domain.Models
{
   public class HashSalt
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}
