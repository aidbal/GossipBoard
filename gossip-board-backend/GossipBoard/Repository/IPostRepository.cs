using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Dto;
using GossipBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace GossipBoard.Repository
{
    public interface IPostRepository
    {
        Task<ICollection<PostDto>> GetAll(int offset, int limit);
        Task<int> Create(Post post);
        Task<int> Update(Post post);
        Task<Post> GetById(int id);
        Task <int> UpVotePost(ApplicationUser user,int id);
        Task<IActionResult> DeleteById(ApplicationUser user, int id);
        Task DeleteByIdWithoutUser(int id);
        ICollection<Post> Search(string str, int offset, int limit);
    }
}
