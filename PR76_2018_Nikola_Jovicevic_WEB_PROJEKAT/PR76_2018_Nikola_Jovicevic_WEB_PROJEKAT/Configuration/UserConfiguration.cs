using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(30);
            builder.Property(x => x.LastName).HasMaxLength(40);
            builder.Property(x => x.Address).HasMaxLength(50);
            builder.Property(x => x.UserName).HasMaxLength(30);
            builder.Property(x => x.ImageFilePath).HasMaxLength(250);
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
