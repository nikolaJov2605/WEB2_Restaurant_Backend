using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Services
{
    public class FoodService : IFood
    {
        private readonly IConfigurationSection _secretKey;
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public FoodService(IMapper mapper, RestaurantDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _secretKey = config.GetSection("SecretKey");

        }

        public async Task<bool> AddFood(FoodUploadDTO food)
        {
            Food foodCheck = await _dbContext.Food.SingleOrDefaultAsync(x => x.Name == food.Name && x.Quantity == food.Quantity && x.UnitOfMeasure == food.UnitOfMeasure);
            if(foodCheck != null)
            {
                return false;
            }

            Food toUpload = _mapper.Map<Food>(food);

            try
            {
                await _dbContext.Food.AddAsync(toUpload);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> AddIngredient(IngredientDTO ingredient)
        {
            Ingredient ingredientCheck = await _dbContext.Ingredients.SingleOrDefaultAsync(x => x.Name == ingredient.Name);
            if(ingredientCheck != null)
            {
                return false;
            }

            Ingredient toAdd = _mapper.Map<Ingredient>(ingredient);

            try
            {
                await _dbContext.Ingredients.AddAsync(toAdd);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<FoodDTO>> GetAllFood()
        {
            try
            {
                List<Food> retList = await _dbContext.Food.ToListAsync();
                return _mapper.Map<List<FoodDTO>>(retList);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<IngredientDTO>> GetAllIngredients()
        {
            List<Ingredient> retList = await _dbContext.Ingredients.ToListAsync();
            return _mapper.Map<List<IngredientDTO>>(retList);
        }

        public async Task<FoodDTO> GetFood(int id)
        {
            try
            {
                Food food = await _dbContext.Food.SingleOrDefaultAsync(x => x.Id == id);
                return _mapper.Map<FoodDTO>(food);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
