using System.Threading.Tasks;
using DatingApp.API.Models;
using DatingApp.API.Data;
using Documents.GitHub.DatingAPP.DatingApp.API.Data;
using System;
using System.Security.Cryptography;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _dbcontext;

        public AuthRepository(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<User> Login(string username, string password)
        {
            var user = await _dbcontext.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;

            if (!VerifyPassword(password, user.PassHash, user.PassSalt))
                return null;

            return user;
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            var hmac = new HMACSHA512(passwordSalt); 
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                        return false;
                }
            }
            return true;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHassh, passwordSalt;
            CreatePasswordHash(password, out passwordHassh, out passwordSalt);

            user.PassHash = passwordHassh;
            user.PassSalt = passwordSalt;

            await _dbcontext.AddAsync(user);
            await _dbcontext.SaveChangesAsync();

            return (user);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            var hmac = new HMACSHA512();
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }

        public async Task<bool> UserExists(string username)
        {
            if (await _dbcontext.Users.AnyAsync(x => x.UserName == username))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}