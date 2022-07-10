using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Controllers
{
    [Route("api/food")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFood _foodService;

        public FoodController(IFood foodService)
        {
            _foodService = foodService;
        }

        [HttpPost("add-food")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddFood([FromBody] FoodUploadDTO food)
        {
            bool retVal = await _foodService.AddFood(food);

            return Ok(retVal);
        }

        [HttpPost("add-ingredient")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AddIngredient([FromBody] IngredientDTO ingredient)
        {
            bool retVal = await _foodService.AddIngredient(ingredient);

            return Ok(retVal);
        }

        [HttpGet("all")]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult> GetAllFood()
        {
            List<FoodDTO> allFood = await _foodService.GetAllFood();
            return Ok(allFood);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> GetFood(int id)
        {
            FoodDTO retFood = await _foodService.GetFood(id);
            return Ok(retFood);
        }

        [HttpGet("ingredients")]
        [Authorize(Roles = "admin, customer")]
        public async Task<ActionResult> GetAllIngredients()
        {
            List<IngredientDTO> allIngredients = await _foodService.GetAllIngredients();
            return Ok(allIngredients);
        }
    }
}
