using AuthServer.Core.DTOs;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface IAuthenticationService
    {
        Task<ResponseDto<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        ResponseDto<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
        Task<ResponseDto<TokenDto>> CreateTokenByRefreshTokenAsync(string refreshToken);
        Task<ResponseDto<NoContentDto>> RevokeRefreshTokenAsync(string refreshToken);
    }
}
