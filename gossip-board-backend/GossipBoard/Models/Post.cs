using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Dto;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace GossipBoard.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Title { get; set; }

        [Required]
        [MinLength(6)]
        public string Text { get; set; }

        public int LikesCount { get; set; }

       public ApplicationUser ApplicationUser { get; set; }

    }
}
