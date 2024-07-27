using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.Core.interfaces;
using UserService.Core.models;
using UserService.Infrastructure.data;

namespace UserService.Infrastructure.repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<UserModel> _userManager;

        public UserRepository(ApplicationDBContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<UserModel> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<bool> UpdateUserEmailAsync(string userId, string newEmail)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Email = newEmail;
            user.NormalizedEmail = newEmail.ToUpper();
            
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}