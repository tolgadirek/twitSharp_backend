using Core.DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfUserDal : EfEntityRepositoryBase<User, TwitSharpContext>, IUserDal
    {
        private readonly TwitSharpContext _context;

        public EfUserDal(TwitSharpContext context) : base(context)
        {
            _context = context;
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }
    }
}
