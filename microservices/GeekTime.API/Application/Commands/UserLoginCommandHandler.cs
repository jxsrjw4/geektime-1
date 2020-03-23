using GeekTime.API.Infrastructure.Auth;
using GeekTime.Infrastructure.Repositories;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GeekTime.API.Application.Commands
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, string>
    {
        IUserRepository _userRepository;
        private readonly TokenManagement _tokenManagement;
        public UserLoginCommandHandler(IUserRepository userRepository, IOptions<TokenManagement> tokenManagement)
        {
            this._userRepository = userRepository;
            _tokenManagement = tokenManagement.Value;
        }

        public Task<string> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var isExist = _userRepository.IsAccountExist(request.UserName, request.Password);

            if (isExist)
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name,request.UserName)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims, expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration), signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

                return Task.FromResult(token);
            }

            return Task.FromResult("");

        }
    }
}
