using GeekTime.Domain.MenuAggregate;
using GeekTime.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GeekTime.API.Application.Queries
{
    public class MyMenuQueryHandler : IRequestHandler<MyMenuQuery, List<Menu>>
    {
        IUserRepository _userRepository;
        public MyMenuQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<List<Menu>> Handle(MyMenuQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return Task.FromResult(new List<Menu>());
            }

            return Task.FromResult(new List<Menu>());
        }
    }
}
