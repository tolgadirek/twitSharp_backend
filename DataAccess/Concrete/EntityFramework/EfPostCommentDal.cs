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
    public class EfPostCommentDal : EfEntityRepositoryBase<PostComment, TwitSharpContext>, IPostCommentDal
    {
        private readonly TwitSharpContext _context;
        public EfPostCommentDal(TwitSharpContext context) : base(context)
        {
            _context = context;
        }

        public List<PostComment> GetComments(int postId)
        {
            return _context.PostComments.Where(c => c.PostId == postId).ToList();
        }
    }
}
