using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekTime.API.Application.Commands
{
    public class UserLoginCommand:IRequest<string>
    {
        public string UserName { get; private set; }

        public string Password { get; private set; }

        public UserLoginCommand(string userName, string password)
        {
            this.UserName = userName;
            this.Password = password;
        }

    }
}
