using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Concrete;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(User user, string password, string firstName, string lastName);
        IDataResult<User> Login(string email, string password);
        IDataResult<User> GetProfile(int userId);
        IDataResult<User> UpdateProfile(User user, string password = null);
        IDataResult<string> CreateAccessToken(User user);
        IDataResult<string> CreateRefreshToken(User user);
        IDataResult<User> GetUserByRefreshToken(string refreshToken);
        IDataResult<Object> RefreshAccessToken(string refreshToken);
    }
}
