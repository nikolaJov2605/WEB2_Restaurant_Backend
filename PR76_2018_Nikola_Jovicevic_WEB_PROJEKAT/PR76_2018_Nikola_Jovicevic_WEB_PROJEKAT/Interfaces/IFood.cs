using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IFood
    {
        Task<FoodDTO> GetFood(int id);
        Task<List<FoodDTO>> GetAllFood();
        Task<List<IngredientDTO>> GetAllIngredients();
    }
}
