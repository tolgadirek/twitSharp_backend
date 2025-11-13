using Core.DataAccess;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPostDal : IEntityRepository<Post>
    {
        List<PostResponseDto> GetPostsWithCounts();
        List<PostResponseDto> GetUserPostsWithCounts(int userId);
    }
}
