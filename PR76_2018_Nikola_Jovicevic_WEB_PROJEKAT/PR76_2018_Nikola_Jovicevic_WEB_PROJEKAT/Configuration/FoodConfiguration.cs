using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration
{
    public class FoodConfiguration : IEntityTypeConfiguration<Food>
    {
        public void Configure(EntityTypeBuilder<Food> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.UnitOfMeasure).HasMaxLength(10);
        }
    }
}
