using GeekTime.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekTime.API.Application.Commands
{
    public class UserLoginCommandHandler : IRequestHandler<UserLoginCommand, bool>
    {
        UserRepository _userRepository;

        public UserLoginCommandHandler(UserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        public Task<bool> Handle(UserLoginCommand request, CancellationToken cancellationToken)
        {
            var isExist = _userRepository.IsAccountExist(request.UserName, request.Password);
            return Task.FromResult(isExist);

        }
    }
}
