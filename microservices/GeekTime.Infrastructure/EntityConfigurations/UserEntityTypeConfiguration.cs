using GeekTime.Domain.OrderAggregate;
using GeekTime.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace GeekTime.Infrastructure.EntityConfigurations
{
    class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id).HasName("UserId");

            
            builder.Property(p => p.Id).HasMaxLength(12);
            builder.Property(p => p.UserName).HasMaxLength(50);

        }
    }
}
