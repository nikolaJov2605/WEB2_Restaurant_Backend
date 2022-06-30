using Microsoft.EntityFrameworkCore;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration
{
    public class RestaurantDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Food> Food { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public RestaurantDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Kazemo mu da pronadje sve konfiguracije u Assembliju i da ih primeni nad bazom
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RestaurantDbContext).Assembly);

        }
    }


}
