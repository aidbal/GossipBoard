using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Models;

namespace GossipBoard.Dto
{
    public class PostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public int LikesCount { get; set; }

        public string ApplicationUserId { get; set; }

        public string ApplicationUserEmail { get; set; }

        public string ApplicationUserFirstName { get; set; }

        public string ApplicationUserLastName { get; set; }

    }
}
