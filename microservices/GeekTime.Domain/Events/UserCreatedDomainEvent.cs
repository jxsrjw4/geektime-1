using GeekTime.Domain.UserAggregate;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeekTime.Domain.Events
{
    public class UserCreatedDomainEvent:IDomainEvent
    {
        public User User { get; private set; }

        public UserCreatedDomainEvent(User user)
        {
            this.User = user;
        }
    }
}
