using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPostLikeDal : EfEntityRepositoryBase<PostLike, TwitSharpContext>, IPostLikeDal
    {
        private readonly TwitSharpContext _context;
        public EfPostLikeDal(TwitSharpContext context) : base(context)
        {
            _context = context;
        }
        public PostLike GetLike(int postId, int userId)
        {
            return _context.PostLikes.FirstOrDefault(x => x.PostId == postId && x.UserId == userId);
        }
    }
}
