using System.Collections.Generic;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs
{
    public class FoodDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public List<IngredientDTO> Ingredients { get; set; }
    }
}
