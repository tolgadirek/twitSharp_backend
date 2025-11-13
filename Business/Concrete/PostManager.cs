using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class PostManager : IPostService
    {
        private readonly IPostDal _postDal;
        public PostManager(IPostDal postDal)
        {
            _postDal = postDal;
        }
        public IResult Add(Post post)
        {
            if (string.IsNullOrWhiteSpace(post.Content))
                return new ErrorResult("Gönderi içeriği boş olamaz.");

            if (post.Content.Length > 280)
                return new ErrorResult("Gönderi 280 karakterden uzun olamaz.");

            post.CreatedAt = DateTime.Now;
            _postDal.Add(post);

            return new SuccessResult("Gönderi başarıyla eklendi.");
        }

        public IResult Delete(int postId, int userId)
        {
            var post = _postDal.Get(p => p.PostId == postId && p.UserId == userId);

            if (post == null)
                return new ErrorResult("Post Bulunamadı.");

            _postDal.Delete(post);
            return new SuccessResult("Post Silindi.");
        }

        public IDataResult<List<PostResponseDto>> GetAll()
        {
            var posts = _postDal.GetPostsWithCounts();
            return new SuccessDataResult<List<PostResponseDto>>(posts);
        }

        public IDataResult<List<PostResponseDto>> GetByUserId(int userId)
        {
            var posts = _postDal.GetUserPostsWithCounts(userId);

            if (posts.Count == 0)
                return new ErrorDataResult<List<PostResponseDto>>("No posts found.");

            return new SuccessDataResult<List<PostResponseDto>>(posts);
        }
    }
}
