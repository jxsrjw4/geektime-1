using GeekTime.Domain.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain.UserAggregate
{
    public class User : Entity<string>, IAggregateRoot
    {
        public string UserId { get; private set; }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public bool IsActivity { get; private set; }

        public User(string userid, string userName, string password, bool isActivity)
        {
            this.UserId = userid;
            this.UserName = userName;
            this.Password = password;
            this.IsActivity = isActivity;

            this.AddDomainEvent(new UserCreatedDomainEvent(this));
        }

    }
}
