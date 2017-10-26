using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GossipBoard.Models
{
    public class VoteHistory
    {
        public int Id { get; set; }

        public int PostId { get; set; }

        public List<ApplicationUser> ApplicationUsers { get; set; }

        public VoteHistory()
        {
            ApplicationUsers = new List<ApplicationUser>();
        }
    }
}
