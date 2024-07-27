using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Core.models;

namespace UserService.Core.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByUsernameAsync(string userId);

        Task<bool> UpdateUserEmailAsync(string userId, string newEmail);
    }
}