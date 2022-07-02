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
    public class OrderService : IOrder
    {
        private readonly IConfigurationSection _secretKey;
        private readonly IConfigurationSection _deliveryFee;
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public OrderService(IMapper mapper, RestaurantDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _secretKey = config.GetSection("SecretKey");
            _deliveryFee = config.GetSection("DeliveryFee");
        }
        public async Task AnounceOrder(OrderDTO orderDto)
        {
            Order order = _mapper.Map<Order>(orderDto);
            order.OrderedFood.Clear();
            List<Food> orderedFood = _mapper.Map<List<Food>>(order.OrderedFood);

            double price = 0;
            foreach (var food in orderDto.OrderedFood)
            {
                if (food.Ingredients == null)
                    food.Ingredients = new List<IngredientDTO>();

                var foodQuery = _dbContext.Food.SingleOrDefault(x => x.Name == food.Name && x.Quantity == food.Quantity);
                if(foodQuery != null)
                {
                    foodQuery.Ingredients = new List<Ingredient>();
                }
                
                foreach(var ingredient in food.Ingredients)
                {
                    var ingredientQuery = _dbContext.Ingredients.SingleOrDefault(x => x.Name == ingredient.Name);
                    if(ingredientQuery != null)
                    {
                        foodQuery.Ingredients.Add(ingredientQuery);
                    }
                }

                order.OrderedFood.Add(foodQuery);

                price += (food.Price * food.Amount);
            }

            price += double.Parse(_deliveryFee.Value);  // dodavanje troskova isporuke na cenu

            order.Price = price;

            try
            {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            /*foreach (var food in orderedFood)
            {
                List<Ingredient> ingredients = _mapper.Map<List<Ingredient>>(food.Ingredients);
            }*/
            /*Order order = _mapper.Map<Order>(orderDto);
            double price = 0;
            foreach(var food in orderDto.OrderedFood)
            {
                _dbContext.Ingredients.SingleOrDefault(x=>x.Name == food.in)
                price += (food.Price * food.Amount);
            }
            price += double.Parse(_deliveryFee.Value);
            try
            {
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }*/
        }
    }
}
