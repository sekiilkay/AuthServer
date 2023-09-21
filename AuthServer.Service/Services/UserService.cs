using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Service.Mapping;
using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Identity;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ResponseDto<AppUserDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new AppUser
            {
                Email = createUserDto.Email,
                UserName = createUserDto.UserName,
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return ResponseDto<AppUserDto>.Fail(400, new ErrorDto(errors, true));
            }
            var resultDto = ObjectMapper.Mapper.Map<AppUserDto>(user);

            return ResponseDto<AppUserDto>.Success(200, resultDto);
        }

        public async Task<ResponseDto<AppUserDto>> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return ResponseDto<AppUserDto>.Fail(404, "UserName not found");

            var userDto = ObjectMapper.Mapper.Map<AppUserDto>(user);

            return ResponseDto<AppUserDto>.Success(200, userDto);
        }
    }
}
