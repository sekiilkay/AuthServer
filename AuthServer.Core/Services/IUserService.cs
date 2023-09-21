using AuthServer.Core.DTOs;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IUserService
    {
        Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<ResponseDto<AppUserDto>> GetUserByNameAsync(string userName);
    }
}
