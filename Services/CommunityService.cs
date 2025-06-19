using Microsoft.AspNetCore.Mvc;
using ZeroToCoder.Data;
using ZeroToCoder.Helpers;
using Microsoft.EntityFrameworkCore;
using ZeroToCoder.Dto;
using ZeroToCoder.Models;


namespace ZeroToCoder.Services
{

    public class CommunityService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommunityService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<PostDTO>> GetPostsAsync()
        {
            var userId = GetCurrentUserId();

            var posts = await _context.Posts
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostDTO
                {
                    Id = p.PostID,
                    Author = p.User.FullName,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt, 
                    Likes = _context.PostLikes.Count(l => l.PostID == p.PostID),
                    IsLiked = _context.PostLikes.Any(l => l.PostID == p.PostID && l.UserID == userId),
                    Replies = _context.Replies
                        .Where(r => r.PostID == p.PostID)
                        .OrderBy(r => r.CreatedAt)
                        .Select(r => new ReplyDTO
                        {
                            Id = r.ReplyID,
                            Author = r.User.FullName,
                            Content = r.Content,
                            CreatedAt = r.CreatedAt, 
                            Likes = _context.ReplyLikes.Count(rl => rl.ReplyID == r.ReplyID),
                            IsLiked = _context.ReplyLikes.Any(rl => rl.ReplyID == r.ReplyID && rl.UserID == userId)
                        }).ToList()
                })
                .ToListAsync();

            
            foreach (var post in posts)
            {
                post.Timestamp = GetTimeAgo(post.CreatedAt);

                foreach (var reply in post.Replies)
                {
                    reply.Timestamp = GetTimeAgo(reply.CreatedAt);
                }
            }

            return posts;
        }

        public async Task<object?> ToggleLikePostAsync(int postId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return null;

            var existingLike = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostID == postId && l.UserID == userId);

            if (existingLike != null)
            {
                _context.PostLikes.Remove(existingLike);
            }
            else
            {
                var newLike = new PostLike
                {
                    PostID = postId,
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.PostLikes.Add(newLike);
            }

            await _context.SaveChangesAsync();

            var likesCount = await _context.PostLikes.CountAsync(l => l.PostID == postId);
            var isLiked = await _context.PostLikes.AnyAsync(l => l.PostID == postId && l.UserID == userId);

            return new
            {
                postId,
                likes = likesCount,
                isLiked
            };
        }



        public async Task<PostDTO> CreatePostAsync(CreatePostDTO createPostDto)
        {
            var userId = GetCurrentUserId();

            if (userId == 0)
            {
                throw new UnauthorizedAccessException("User must be logged in to create a post.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var post = new Post
            {
                Content = createPostDto.Content,
                UserID = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return new PostDTO
            {
                Id = post.PostID,
                Author = user.FullName,
                Content = post.Content,
                Timestamp = GetTimeAgo(post.CreatedAt),
                Likes = 0,
                IsLiked = false,
                Replies = new List<ReplyDTO>()
            };
        }



        private int GetCurrentUserId()
        {


            var userId = _httpContextAccessor.HttpContext?.User.FindFirst("id")?.Value;
            return userId != null ? int.Parse(userId) : 0;

            // return 2;
        }




        public async Task<string?> DeletePostAsync(int postId, int userId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(p => p.PostID == postId && p.UserID == userId);
            if (post == null)
                return null;

           
            var replies = _context.Replies.Where(r => r.PostID == postId);
            _context.Replies.RemoveRange(replies);

            var likes = _context.PostLikes.Where(l => l.PostID == postId);
            _context.PostLikes.RemoveRange(likes);

            var reports = _context.PostReports.Where(r => r.PostID == postId);
            _context.PostReports.RemoveRange(reports);

         
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return "Post deleted successfully";
        }

        public async Task<string?> DeleteReplyAsync(int postId, int replyId, int userId)
        {
            var reply = await _context.Replies.FirstOrDefaultAsync(r => r.ReplyID == replyId && r.PostID == postId && r.UserID == userId);
            if (reply == null)
                return null;

           
            var likes = _context.ReplyLikes.Where(l => l.ReplyID == replyId);
            _context.ReplyLikes.RemoveRange(likes);

            var reports = _context.ReplyReports.Where(r => r.ReplyID == replyId);
            _context.ReplyReports.RemoveRange(reports);

           
            _context.Replies.Remove(reply);
            await _context.SaveChangesAsync();

            return "Reply deleted successfully";
        }


        public async Task<string?> ReportPostAsync(int postId, string? reason)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return null;

          
            var alreadyReported = await _context.PostReports
                .AnyAsync(r => r.PostID == postId && r.UserID == userId);
            if (alreadyReported)
                return "You have already reported this post.";

            var report = new PostReport
            {
                PostID = postId,
                UserID = userId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow
            };

            _context.PostReports.Add(report);
            await _context.SaveChangesAsync();

            return "Post reported successfully";
        }


        public async Task<ReplyLikeDTO?> ToggleReplyLikeAsync(int replyId)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return null;

            var existingLike = await _context.ReplyLikes
                .FirstOrDefaultAsync(l => l.ReplyID == replyId && l.UserID == userId);

            if (existingLike != null)
            {
                _context.ReplyLikes.Remove(existingLike);
            }
            else
            {
                var newLike = new ReplyLike
                {
                    ReplyID = replyId,
                    UserID = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.ReplyLikes.Add(newLike);
            }

            await _context.SaveChangesAsync();

            var totalLikes = await _context.ReplyLikes.CountAsync(l => l.ReplyID == replyId);
            var isLiked = await _context.ReplyLikes.AnyAsync(l => l.ReplyID == replyId && l.UserID == userId);

            return new ReplyLikeDTO
            {
                ReplyId = replyId,
                Likes = totalLikes,
                IsLiked = isLiked
            };
        }


        public async Task<string?> ReportReplyAsync(int replyId, string? reason)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return null;

            var alreadyReported = await _context.ReplyReports
                .AnyAsync(r => r.ReplyID == replyId && r.UserID == userId);

            if (!alreadyReported)
            {
                var report = new ReplyReport
                {
                    ReplyID = replyId,
                    UserID = userId,
                    Reason = reason,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ReplyReports.Add(report);
                await _context.SaveChangesAsync();
            }

            return "Reply reported successfully";
        }

        public async Task<ReplyDTO?> CreateReplyAsync(int postId, CreateReplyDTO dto)
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
                return null;

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return null;

            var reply = new Reply
            {
                PostID = postId,
                UserID = userId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();

            var replyDto = new ReplyDTO
            {
                Id = reply.ReplyID,
                Author = user.FullName,
                Content = reply.Content,
                CreatedAt = reply.CreatedAt,
                Timestamp = "just now",
                Likes = 0,
                IsLiked = false
            };

            return replyDto;
        }




        private string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "just now";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} minutes ago";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} hours ago";
            if (timeSpan.TotalDays < 30)
                return $"{(int)timeSpan.TotalDays} days ago";

            return dateTime.ToShortDateString();
        }
    }
}
