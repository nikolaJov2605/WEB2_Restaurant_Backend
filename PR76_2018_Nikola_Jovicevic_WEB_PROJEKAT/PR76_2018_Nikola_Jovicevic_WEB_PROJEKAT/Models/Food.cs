using System.Collections.Generic;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public double Price { get; set; }
        //public List<Ingredient> Ingredients { get; set; }
    }
}
