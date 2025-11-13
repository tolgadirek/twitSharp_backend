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
    public class PostLikeManager : IPostLikeService
    {
        private readonly IPostLikeDal _likeDal;

        public PostLikeManager(IPostLikeDal likeDal)
        {
            _likeDal = likeDal;
        }

        public IResult ToggleLike(int postId, int userId)
        {
            var existingLike = _likeDal.GetLike(postId, userId);

            if (existingLike != null)
            {
                _likeDal.Delete(existingLike);
                return new SuccessResult("Like removed.");
            }

            _likeDal.Add(new PostLike
            {
                PostId = postId,
                UserId = userId
            });

            return new SuccessResult("Post liked.");
        }

        public IDataResult<int> GetLikeCount(int postId)
        {
            var count = _likeDal.GetAll(x => x.PostId == postId).Count;
            return new SuccessDataResult<int>(count);
        }
    }
}
