using Business.Abstract;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entity.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly IUserDal _userDal;
        private readonly ITokenHelper _tokenHelper;

        public AuthManager(IUserDal userDal, ITokenHelper tokenHelper)
        {
            _userDal = userDal;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<User> Register(User user, string password, string firstName, string lastName)
        {
            // E-posta kontrolü
            if (!UserExists(user.Email).Success)
            {
                return new ErrorDataResult<User>("Email already exists.");
            }

            // Email formatı kontrolü
            if (!IsValidEmail(user.Email))
            {
                return new ErrorDataResult<User>("Invalid email format.");
            }

            // Şifre uzunluğu kontrolü
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return new ErrorDataResult<User>("Password must be at least 6 characters long.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = user.Email,
                Password = hashedPassword,
                CreatedAt = DateTime.Now
            };

            _userDal.Add(newUser);
            return new SuccessDataResult<User>(newUser, "User registered successfully.");
        }

        public IDataResult<User> Login(string email, string password)
        {
            var existingUser = _userDal.GetByEmail(email);
            if (existingUser == null)
            {
                return new ErrorDataResult<User>("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, existingUser.Password))
            {
                return new ErrorDataResult<User>("Invalid password.");
            }

            return new SuccessDataResult<User>(existingUser, "Login successful.");
        }

        public IResult UserExists(string email)
        {
            var user = _userDal.GetByEmail(email);
            if (user != null)
            {
                return new ErrorResult("User already exists.");
            }
            return new SuccessResult();
        }

        public IDataResult<User> GetProfile(int userId)
        {
            var user = _userDal.GetById(userId);
            if (user == null)
            {
                return new ErrorDataResult<User>("User not found.");
            }

            user.Password = null; // güvenlik için parolayı null gönder
            return new SuccessDataResult<User>(user);
        }

        public IDataResult<User> UpdateProfile(User user, string password = null)
        {
            var existingUser = _userDal.GetById(user.UserId);
            if (existingUser == null)
            {
                return new ErrorDataResult<User>("User not found.");
            }

            if (!IsValidEmail(user.Email))
            {
                return new ErrorDataResult<User>("Invalid email format.");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(password))
            {
                if (password.Length < 6)
                    return new ErrorDataResult<User>("Password must be at least 6 characters long.");

                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
            }

            _userDal.Update(existingUser);
            return new SuccessDataResult<User>(existingUser, "Profile updated successfully.");
        }

        public IDataResult<string> CreateAccessToken(User user)
        {
            var token = _tokenHelper.CreateToken(user);
            return new SuccessDataResult<string>(token, "Token created.");
        }

        public IDataResult<string> CreateRefreshToken(User user)
        {
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var expiration = DateTime.UtcNow.AddDays(7); // 7 gün geçerli

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiration = expiration;

            _userDal.Update(user);

            return new SuccessDataResult<string>(refreshToken, "Refresh token created.");
        }

        public IDataResult<User> GetUserByRefreshToken(string refreshToken)
        {
            var user = _userDal.Get(u => u.RefreshToken == refreshToken);

            if (user == null)
                return new ErrorDataResult<User>("User not found.");

            if (user.RefreshTokenExpiration < DateTime.UtcNow)
                return new ErrorDataResult<User>("Refresh token expired.");

            return new SuccessDataResult<User>(user);
        }

        public IDataResult<object> RefreshAccessToken(string refreshToken)
        {
            var userResult = GetUserByRefreshToken(refreshToken);
            if (!userResult.Success)
                return new ErrorDataResult<object>(userResult.Message);

            var user = userResult.Data;

            // 1) Yeni access token oluştur
            var newAccessToken = _tokenHelper.CreateToken(user);

            // 2) Yeni refresh token oluştur
            var newRefreshTokenResult = CreateRefreshToken(user);

            return new SuccessDataResult<object>(new
            {
                accessToken = newAccessToken,
                refreshToken = newRefreshTokenResult.Data
            }, "Token refreshed.");
        }

        private bool IsValidEmail(string email)
        {
            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
            return emailRegex.IsMatch(email);
        }
    }
}
