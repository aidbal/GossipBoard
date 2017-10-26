using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GossipBoard.Models;
using GossipBoard.Dto;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace GossipBoard.Repository
{
    public class PostRepository : IPostRepository
    {
        public IActionResult BadRequestResult { get; set; }
        public IActionResult OkResult { get; private set; }

        private readonly DbSet<Post> _posts;
        private readonly DbSet<VoteHistory> _voteHistories;
        private readonly DbContext _context;
        private readonly IMapper _mapper;

        public PostRepository(GossipBoardContext context, IMapper mapper)
        {
            _posts = context.Posts;
            _voteHistories = context.VoteHistories;
            _context = context;
            _mapper = mapper;
        }

        public async Task<Post> GetById(int id)
        {
            var post = await _posts.FindAsync(id);
            return post;
        }

        public async Task<ICollection<PostDto>> GetAll(int offset, int limit)
        {
            var posts = await _posts
                .Include(m => m.ApplicationUser)
                .OrderByDescending(m => m.Id)
                .Skip(offset)
                .Take(limit)
                .ToArrayAsync();
            return _mapper.Map<ICollection<Post>, ICollection<PostDto>>(posts);
        }

        public async Task<int> Create(Post newPost)
        {
            await _posts.AddAsync(newPost);
            await _context.SaveChangesAsync();
            return newPost.Id;
        }

        public async Task<int> Update(Post newPost)
        {
            var post = await _posts.FindAsync(newPost.Id);
            if (post == null) return -1;
            post.Text = newPost.Text;
            post.Title = newPost.Title;
            await _context.SaveChangesAsync();
            return newPost.Id;
        }

        public async Task<int> UpVotePost(ApplicationUser user, int id)
        {
            var post = await _posts.FindAsync(id);
            if (post == null) return -1;
            var voteHistory = _voteHistories.SingleOrDefault(c => c.PostId == post.Id);

            if (voteHistory == null)
            {
                VoteHistory voteHistoryNew = new VoteHistory();
                voteHistoryNew.PostId = post.Id;
                voteHistoryNew.ApplicationUsers.Add(user);
                await _voteHistories.AddAsync(voteHistoryNew);
                post.LikesCount++;
            }
            else if (voteHistory.ApplicationUsers.Count == 0)
            {
                voteHistory.ApplicationUsers.Add(user);
                post.LikesCount++;
            }
            else
            {
                for (var i = 0; i < voteHistory.ApplicationUsers.Count; i++)
                {
                    if (voteHistory.ApplicationUsers[i].Id == user.Id)
                    {
                        voteHistory.ApplicationUsers.Remove(user);
                        post.LikesCount--;
                        await _context.SaveChangesAsync();
                        return post.LikesCount;
                    }
                    voteHistory.ApplicationUsers.Add(user);
                    post.LikesCount++;
                }
            }
            await _context.SaveChangesAsync();

            return post.LikesCount;
        }

        public async Task<IActionResult> DeleteById(ApplicationUser user, int id)
        {

            //var post = _posts.SingleOrDefault(c => c.Id == id);
            //if (post.ApplicationUser.Id == user.Id)
            //{
            //    _context.Remove(post);
            //    await _context.SaveChangesAsync();
            //    return OkResult;
            //}
            return BadRequestResult;

        }

        public async Task DeleteByIdWithoutUser(int id)
        {
            var foundPost = await _posts.Where(post => post.Id == id).FirstOrDefaultAsync();
            if (foundPost == null)
                throw new InvalidOperationException("No post found matching id " + id);
            _context.Remove(foundPost);
            await _context.SaveChangesAsync();
        }

        

        public ICollection<Post> Search(string search, int offset, int limit)
        {
            search = search.ToLower();
            string[] searches = search.Split(null);
            ICollection<Post> foundPosts = new List<Post>();
            foreach (string searchStub in searches)
            {
                var str = Regex.Replace(searchStub, @"\s", ""); //remove whitespace
                if (str.Length == 0) continue;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Searching for:'" + str + "'");
                Console.ResetColor();
                List<Post> posts = _posts.Where(s => (s.Title.ToLower().Contains(str) ||
                                              s.Text.ToLower().Contains(str) ||
                                              //s.ApplicationUser.Email.ToLower().Contains(str) ||
                                              s.ApplicationUser.Firstname.ToLower().Contains(str) ||
                                              s.ApplicationUser.Lastname.ToLower().Contains(str)
                                              )).OrderByDescending(m => m.Id).ToList();
                foreach (Post p in posts)
                {
                    if (!foundPosts.Contains(p))
                    {
                        foundPosts.Add(p);
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Found: " + foundPosts.Count);
            Console.ResetColor();

            return foundPosts.AsQueryable()
                .Skip(offset)
                .Take(limit)
                .ToArray();
        }

    }
}
