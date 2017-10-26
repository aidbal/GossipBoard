using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipBoard.Dto
{
    public class UserDto
    {

        public string Username { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }
    }
}
