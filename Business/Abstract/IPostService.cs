using Core.Utilities.Results;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPostService
    {
        IResult Add(Post post);
        IResult Delete(int postId, int userId);
        IDataResult<List<PostResponseDto>> GetAll();
        IDataResult<List<PostResponseDto>> GetByUserId(int userId);
    }
}
