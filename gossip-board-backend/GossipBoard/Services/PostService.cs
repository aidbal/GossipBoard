using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Models;
using GossipBoard.Repository;
using GossipBoard.Dto;
using Microsoft.AspNetCore.Mvc;

namespace GossipBoard.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _repository;

        public PostService(IPostRepository repository)
        {
            _repository = repository;
        }


        public async Task<Post> GetPostById(int id)
        {
            var post = await _repository.GetById(id);
            return post;
        }



        public async Task<ICollection<PostDto>> GetAllPosts(int offset, int limit)
        {
            var posts = await _repository.GetAll(offset, limit);
            return posts;
        }

        public async Task<int> Update(Post newPost)
        {
            var post = await _repository.Update(newPost);
            return post;
        }

        public async Task<int> Create(Post newPost)
        {
            var post = await _repository.Create(newPost);
            return post;
        }
        public async Task<IActionResult> DeleteById(ApplicationUser user, int id)
        {
            return await _repository.DeleteById(user,id);
        }
        public async Task DeleteByIdWithoutUser(int id)
        {
            await _repository.DeleteByIdWithoutUser(id);
        }

        public async Task<int> UpVotePost(ApplicationUser user, int id)
        {
            var postLikeCount = await _repository.UpVotePost(user, id);
            return postLikeCount;
        }
        public ICollection<Post> SearchPosts(string str, int offset, int limit)
        {
            var result = _repository.Search(str, offset, limit);
            return result;
        }
    }
}
