using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Concrete
{
    public class PostCommentManager : IPostCommentService
    {
        private readonly IPostCommentDal _commentDal;
        private readonly IUserDal _userDal;
        private readonly IPostDal _postDal;

        public PostCommentManager(IPostCommentDal commentDal, IUserDal userDal, IPostDal postDal)
        {
            _commentDal = commentDal;
            _userDal = userDal;
            _postDal = postDal;
        }

        public IDataResult<PostComment> AddComment(int userId, CreateCommentDto dto)
        {
            var post = _postDal.Get(p => p.PostId == dto.PostId);
            if (post == null)
                return new ErrorDataResult<PostComment>("Post not found.");

            var user = _userDal.Get(u => u.UserId == userId);
            if (user == null)
                return new ErrorDataResult<PostComment>("User not found.");

            var comment = new PostComment
            {
                PostId = dto.PostId,
                UserId = userId,
                Content = dto.Content,
                CreatedAt = DateTime.Now
            };

            _commentDal.Add(comment);

            // User bilgisi de dönsün diye ekliyoruz
            comment.User = user;
            comment.User.Password = null; // güvenlik

            return new SuccessDataResult<PostComment>(comment, "Comment added.");
        }

        public IDataResult<List<PostComment>> GetComments(int postId)
        {
            var comments = _commentDal.GetAll(c => c.PostId == postId);

            foreach (var c in comments)
            {
                c.User = _userDal.Get(u => u.UserId == c.UserId);
                if (c.User != null)
                    c.User.Password = null;
            }

            return new SuccessDataResult<List<PostComment>>(comments);
        }
    }
}
