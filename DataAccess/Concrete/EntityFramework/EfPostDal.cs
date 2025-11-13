using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPostDal : EfEntityRepositoryBase<Post, TwitSharpContext>, IPostDal
    {
        private readonly TwitSharpContext _context;
        public EfPostDal(TwitSharpContext context) : base(context) 
        {
            _context = context;
        }

        public List<PostResponseDto> GetPostsWithCounts()
        {
            var result = from p in _context.Posts select new PostResponseDto
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt,
                LikeCount = _context.PostLikes.Count(l => l.PostId == p.PostId),
                CommentCount = _context.PostComments.Count(c => c.PostId == p.PostId)
            };

            return result.OrderByDescending(x => x.CreatedAt).ToList();
        }

        public List<PostResponseDto> GetUserPostsWithCounts(int userId)
        {
            var result =
            from p in _context.Posts
            where p.UserId == userId
            select new PostResponseDto
            {
                PostId = p.PostId,
                UserId = p.UserId,
                Content = p.Content,
                ImageUrl = p.ImageUrl,
                CreatedAt = p.CreatedAt,
                LikeCount = _context.PostLikes.Count(l => l.PostId == p.PostId),
                CommentCount = _context.PostComments.Count(c => c.PostId == p.PostId)
            };

            return result.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}
