using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace GossipBoard.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public List<Post> Posts { get; set; }
        public ApplicationUser()
        {
            Posts = new List<Post>();
        }
    }

    public class GossipBoardContext : IdentityDbContext<ApplicationUser>
    {
        public GossipBoardContext(DbContextOptions<GossipBoardContext> options)
            : base(options)
        { }

        public DbSet<Post> Posts { get; set; }
        public DbSet<VoteHistory> VoteHistories { get; set; }
    }
}
