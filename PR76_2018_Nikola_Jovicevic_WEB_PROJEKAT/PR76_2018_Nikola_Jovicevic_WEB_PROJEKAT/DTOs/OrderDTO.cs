using System.Collections.Generic;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public string UserEmail { get; set; }
        public string Comment { get; set; }
        public string Address { get; set; }
        public bool Accepted { get; set; }
        public List<FoodDTO> OrderedFood { get; set; }
    }
}
