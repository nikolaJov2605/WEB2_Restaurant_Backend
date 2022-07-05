namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models
{
    public class FoodOrder
    {
        public int Id { get; set; }
        public Food Food { get; set; }
        public Order Order { get; set; }
        public int FoodAmount { get; set; }
        public string Ingredients { get; set; }
    }
}
