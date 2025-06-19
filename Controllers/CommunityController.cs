using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Services;
using ZeroToCoder.Data;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;
using ZeroToCoder.Helpers;



namespace ZeroToCoder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommunityController : ControllerBase
    {
        private readonly CommunityService _communityService;

        public CommunityController(CommunityService communityService)
        {
            _communityService = communityService;
        }

        
        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _communityService.GetPostsAsync();
            return Ok(posts);
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO createPostDto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var post = await _communityService.CreatePostAsync(createPostDto);
            return Ok(post);
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts/{postId}/like")]
        public async Task<IActionResult> ToggleLikePost(int postId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.ToggleLikePostAsync(postId);
            return Ok(result);
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts/{postId}/report")]
        public async Task<IActionResult> ReportPost(int postId, [FromBody] ReportPostDTO reportDto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.ReportPostAsync(postId, reportDto?.Reason);
            return Ok(new { message = result });
        }

       
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts/{postId}/replies")]
        public async Task<IActionResult> CreateReply(int postId, [FromBody] CreateReplyDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var reply = await _communityService.CreateReplyAsync(postId, dto);
            return Ok(reply);
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts/{postId}/replies/{replyId}/like")]
        public async Task<IActionResult> ToggleReplyLike(int replyId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.ToggleReplyLikeAsync(replyId);
            return Ok(result);
        }

        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("posts/{postId}/replies/{replyId}/report")]
        public async Task<IActionResult> ReportReply(int replyId, [FromBody] ReplyReportCreateDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.ReportReplyAsync(replyId, dto.Reason);
            return Ok(new { message = result });
        }

        
        private int GetCurrentUserId()
        {
            
            var userIdClaim = User?.FindFirst("id")?.Value;
            return userIdClaim != null ? int.Parse(userIdClaim) : 0;
        }

       
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("posts/{postId}")]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.DeletePostAsync(postId, userId);

            if (result == null)
                return NotFound("Post not found or you do not have permission to delete it.");

            return Ok(new { message = "Post deleted successfully" });
        }

      
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("posts/{postId}/replies/{replyId}")]
        public async Task<IActionResult> DeleteReply(int postId, int replyId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return Unauthorized("User must be logged in.");

            var result = await _communityService.DeleteReplyAsync(postId, replyId, userId);

            if (result == null)
                return NotFound("Reply not found or you do not have permission to delete it.");

            return Ok(new { message = "Reply deleted successfully" });
        }




    }
}
