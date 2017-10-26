using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Models;
using GossipBoard.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GossipBoard.Services
{
    public interface IPostService
    {

        Task<ICollection<PostDto>> GetAllPosts(int offset, int limit);
        Task<int> Create(Post post);
        Task<int> Update(Post post);
        Task<int> UpVotePost(ApplicationUser user, int id);
        Task<Post> GetPostById(int id);
        Task<IActionResult> DeleteById(ApplicationUser user, int id);
        Task DeleteByIdWithoutUser(int id);
        ICollection<Post> SearchPosts(string str, int offset, int limit);
    }
}
