using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GossipBoard.Dto;
using GossipBoard.Models;
using GossipBoard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GossipBoard.Controllers
{
    //[Authorize]
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private readonly IPostService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public PostsController(IPostService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet]
        [Produces(typeof(PostDto[]))]
        public async Task<IActionResult> Get([FromQuery]int offset = 0, [FromQuery]int limit = 50)
        {
            var posts = await _service.GetAllPosts(offset, limit);
            return Ok(posts);
        }
        [HttpGet("{id}")]
        [Produces(typeof(Post))]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            var result = await _service.GetPostById(id);

            if (result == null)
            {
                return NotFound("Can't Find Post by this Id");
            }
            return Ok(result);
        }

        [HttpPut("{id}")]
        //[Produces(typeof(Post))]
        //public async Task<IActionResult> Post([FromBody]Post value)
        public async Task<IActionResult> Put([FromBody]Post value, [FromRoute]int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            value.Id = id;
            value.ApplicationUser = user;
            var updatedId = await _service.Update(value);
            if (updatedId == -1) return NotFound();
            return Ok(id);
        }


        [HttpPost]
        [Produces(typeof(Post))]
        //public async Task<IActionResult> Post([FromBody]Post value)
        public async Task<IActionResult> Post([FromBody]Post value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _userManager.GetUserAsync(User);
            value.ApplicationUser = user;
            var id = await _service.Create(value);
            //return Ok(id);
            return CreatedAtAction("Post", new { id = id }, value);
        }

        [HttpGet("upvote/{id}")]
        [Produces(typeof(int))]
        public async Task<IActionResult> UpVotePost([FromRoute]int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var postLikeCount = await _service.UpVotePost(user , id);
            return Ok(postLikeCount);
        }

        //[HttpDelete("{id}")]
        //[Produces(typeof(Post))]
        //public async Task<IActionResult> Delete([FromRoute]int id)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (id == 0) return NotFound("Can't Find Post by this Id");
        //    IActionResult result = await _service.DeleteById(user,id);
        //    return result;
        //}

        [HttpDelete("{id}")]
        [Produces(typeof(Post))]
        public async Task<IActionResult> DeleteWithoutUser([FromRoute]int id)
        {
            try
            {
                await _service.DeleteByIdWithoutUser(id);
            }
            catch (InvalidOperationException e)
            {
                return BadRequest("No post found with id " + id);
            }
            return new OkResult();
        }

        [HttpGet("search")]
        [Produces(typeof(Post[]))]
        public async Task<ActionResult> Search([FromQuery] string search = "", [FromQuery]int offset = 0, [FromQuery]int limit = 10)
        {
            if (search == null || search.Equals("")) {
                //GetAll controller implementation
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Empty search, just getting posts!");
                Console.ResetColor();
                var p = await _service.GetAllPosts(offset, limit);
                return Ok(p);
            }
            
            var posts = _service.SearchPosts(search, offset, limit);
            return Ok(posts);
        }
    }
}