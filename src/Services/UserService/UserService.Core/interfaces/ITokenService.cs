using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Core.models;

namespace UserService.Core.interfaces
{
    public interface ITokenService
    {
        string CreateToken(UserModel user);
    }
}