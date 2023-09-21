using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Services
{
    public interface ITokenService
    {
        TokenDto CreateToken(AppUser appUser);
        ClientTokenDto CreateTokenByClient(Client client);
    }
}
