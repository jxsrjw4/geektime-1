using GeekTime.Domain.MenuAggregate;
using GeekTime.Domain.UserAggregate;
using GeekTime.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Infrastructure.Repositories
{
    public class UserRepository : Repository<User, string, DomainContext>, IUserRepository
    {
        public UserRepository(DomainContext context) : base(context)
        {
            
        }

        public bool IsAccountExist(string userName, string password)
        {
            return true;
        }

        public List<Menu> GetMenus(string UserName)
        {
            return null;
        }
    }
}
