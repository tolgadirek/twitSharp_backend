using Core.Utilities.Results;
using Entity.Concrete;
using Entity.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPostCommentService
    {
        IDataResult<PostComment> AddComment(int userId, CreateCommentDto dto);
        IDataResult<List<PostComment>> GetComments(int postId);
    }
}
