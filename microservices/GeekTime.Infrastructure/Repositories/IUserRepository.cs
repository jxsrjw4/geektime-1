using GeekTime.Domain.MenuAggregate;
using GeekTime.Domain.UserAggregate;
using GeekTime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Infrastructure.Repositories
{
    public interface IUserRepository:IRepository<User, string>
    {
        bool IsAccountExist(string userName, string password);

        List<Menu> GetMenus(string Token);
    }
}
