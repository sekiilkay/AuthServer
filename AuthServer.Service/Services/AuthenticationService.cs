using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly List<Client> _clients;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<UserRefreshToken> _userRefreshToken;
        public AuthenticationService(IOptions<List<Client>> option,ITokenService tokenService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IGenericRepository<UserRefreshToken> userRefreshToken)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _userRefreshToken = userRefreshToken;
            _clients = option.Value;
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto)
        {
            if (loginDto == null)
                throw new ArgumentNullException(nameof(loginDto));

            var hasUser = await _userManager.FindByEmailAsync(loginDto.Email);

            if (hasUser == null)
                return ResponseDto<TokenDto>.Fail(400, "Email or Password is wrong");

            var checkPassword = await _userManager.CheckPasswordAsync(hasUser, loginDto.Password);  
    
            if (!checkPassword)
                return ResponseDto<TokenDto>.Fail(400, "Email or Password is wrong");

            var token = _tokenService.CreateToken(hasUser);

            var userRefreshToken = await _userRefreshToken.Where(x => x.UserId == hasUser.Id).SingleOrDefaultAsync();
        
            if (userRefreshToken == null)
            {
                await _userRefreshToken.AddAsync(new UserRefreshToken { UserId = hasUser.Id, Code = token.RefreshToken, Expiration = token.RefreshTokenExpiration });
            }
            else
            {
                userRefreshToken.Code = token.RefreshToken;
                userRefreshToken.Expiration = token.RefreshTokenExpiration;
            }

            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Success(200, token);

        }

        public ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
        {
            var client = _clients.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

            if (client == null)
                return ResponseDto<ClientTokenDto>.Fail(404, "ClientId or ClientSecret not found");

            var token = _tokenService.CreateTokenByClient(client);

            return ResponseDto<ClientTokenDto>.Success(200, token);
        }

        public async Task<ResponseDto<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _userRefreshToken.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
                return ResponseDto<TokenDto>.Fail(404, "RefreshToken not found");

            var hasUser = await _userManager.FindByIdAsync(existRefreshToken.UserId);

            if (hasUser == null)
                return ResponseDto<TokenDto>.Fail(404, "UserId not found");

            var tokenDto = _tokenService.CreateToken(hasUser);

            existRefreshToken.Code = tokenDto.RefreshToken;
            existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

            await _unitOfWork.CommitAsync();

            return ResponseDto<TokenDto>.Success(200, tokenDto);
        }

        public async Task<ResponseDto<NoContentDto>> RevokeRefreshTokenAsync(string refreshToken)
        {
            var existRefreshToken = await _userRefreshToken.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

            if (existRefreshToken == null)
                return ResponseDto<NoContentDto>.Fail(404, "RefreshToken not found");

            _userRefreshToken.Remove(existRefreshToken);
            await _unitOfWork.CommitAsync();

            return ResponseDto<NoContentDto>.Success(200);
        }
    }
}
