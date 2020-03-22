using GeekTime.Domain.MenuAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeekTime.API.Application.Queries
{
    public class MyMenuQuery : IRequest<List<Menu>>
    {
        public MyMenuQuery(string token)
        {
            Token = token;
        }

        /// <summary>
        /// 登录后获取的Token
        /// </summary>
        public string Token { get; private set; }
    }
}
