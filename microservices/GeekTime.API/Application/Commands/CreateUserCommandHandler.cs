using DotNetCore.CAP;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GeekTime.Infrastructure.Repositories;

namespace GeekTime.API.Application.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        ICapPublisher _capPublisher;
        UserRepository orderRepository;
        public CreateUserCommandHandler(UserRepository userRepository)
        { 
            
        }

        public Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
