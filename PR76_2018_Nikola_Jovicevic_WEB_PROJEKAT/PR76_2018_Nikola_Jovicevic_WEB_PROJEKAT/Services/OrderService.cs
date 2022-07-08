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

            List<FoodOrder> toAdd = new List<FoodOrder>();
            double price = 0;
            foreach (var food in orderDto.OrderedFood)
            {
                FoodOrder foodOrder = new FoodOrder();
                if (food.Ingredients == null)
                    food.Ingredients = new List<IngredientDTO>();

                var foodQuery = _dbContext.Food.SingleOrDefault(x => x.Name == food.Name && x.Quantity == food.Quantity);
                if(foodQuery == null)
                {
                    return;
                 //   foodQuery.Ingredients = new List<Ingredient>();
                }

                string ingredients = "";
                foreach(var ingredient in food.Ingredients)
                {
                    var ingredientQuery = _dbContext.Ingredients.SingleOrDefault(x => x.Name == ingredient.Name);
                    if(ingredientQuery != null)
                    {
                        if(ingredients == "")
                        {
                            ingredients = ingredientQuery.Name;
                        }
                        else
                        {
                            ingredients += (", " + ingredientQuery.Name);
                        }
                    }
                }
                foodOrder.Order = order;
                foodOrder.Food = foodQuery;
                foodOrder.Ingredients = ingredients;
                foodOrder.FoodAmount = food.Amount;

                toAdd.Add(foodOrder);

                price += (food.Price * food.Amount);
            }

            price += double.Parse(_deliveryFee.Value);  // dodavanje troskova isporuke na cenu

            order.Price = price;
            order.TimePosted = DateTime.Now;
            order.TimeAccepted = null;
            order.TimeDelivered = null;

            try
            {
                _dbContext.Orders.Add(order);
                _dbContext.FoodOrder.AddRange(toAdd);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<List<OrderDTO>> GetAvailableOrders()
        {
            List<Order> orders = await _dbContext.Orders.Where(x => x.Accepted == false).ToListAsync();
            List<OrderDTO> retList = new List<OrderDTO>();
            foreach(var order in orders)
            {
                List<FoodOrder> temp = _dbContext.FoodOrder.Include(x => x.Food).Where(x => x.Order.Id == order.Id).ToList();
                OrderDTO orderDto = _mapper.Map<OrderDTO>(order);
                orderDto.OrderedFood = new List<FoodDTO>();
                foreach (var fo in temp)
                {
                    FoodDTO foodDTO = new FoodDTO();
                    Food food = _dbContext.Food.FirstOrDefault(x => x.Id == fo.Food.Id);
                    if (food == null)
                        break;
                    foodDTO = _mapper.Map<FoodDTO>(food);
                    foodDTO.Ingredients = new List<IngredientDTO>();
                    string[] strArr = fo.Ingredients.Split(", ");
                    foreach (var s in strArr)
                    {
                        foodDTO.Ingredients.Add(new IngredientDTO { Name = s });
                    }
                    foodDTO.Amount = fo.FoodAmount;
                    orderDto.OrderedFood.Add(foodDTO);
                }
                retList.Add(orderDto);
            }
            return retList;
        }

        public async Task<List<OrderDTO>> GetOrdersForUser(string email)
        {
            List<Order> orders = await _dbContext.Orders.Where(x => x.UserEmail == email).ToListAsync();
            List<OrderDTO> retList = new List<OrderDTO>();
            foreach (var order in orders)
            {
                List<FoodOrder> temp = _dbContext.FoodOrder.Include(x=>x.Food).Where(x => x.Order.Id == order.Id).ToList();
                OrderDTO orderDto = _mapper.Map<OrderDTO>(order);
                orderDto.OrderedFood = new List<FoodDTO>();
                foreach (var fo in temp)
                {
                    FoodDTO foodDTO = new FoodDTO();
                    Food food = _dbContext.Food.FirstOrDefault(x => x.Id == fo.Food.Id);
                    if (food == null)
                        break;
                    foodDTO = _mapper.Map<FoodDTO>(food);
                    foodDTO.Ingredients = new List<IngredientDTO>();
                    string[] strArr = fo.Ingredients.Split(", ");
                    foreach(var s in strArr)
                    {
                        foodDTO.Ingredients.Add(new IngredientDTO { Name = s });
                    }
                    foodDTO.Amount = fo.FoodAmount;
                    orderDto.OrderedFood.Add(foodDTO);
                }
                retList.Add(orderDto);
            }

            return retList;
        }

        public async Task<List<OrderDTO>> GetUndeliveredOrders(string email)
        {
            List<Order> orders = await _dbContext.Orders.Where(x => x.UserEmail == email && x.Delivered == false).ToListAsync();
            List<OrderDTO> retList = new List<OrderDTO>();
            foreach (var order in orders)
            {
                List<FoodOrder> temp = _dbContext.FoodOrder.Include(x => x.Food).Where(x => x.Order.Id == order.Id).ToList();
                OrderDTO orderDto = _mapper.Map<OrderDTO>(order);
                orderDto.OrderedFood = new List<FoodDTO>();
                foreach (var fo in temp)
                {
                    FoodDTO foodDTO = new FoodDTO();
                    Food food = _dbContext.Food.FirstOrDefault(x => x.Id == fo.Food.Id);
                    if (food == null)
                        break;
                    foodDTO = _mapper.Map<FoodDTO>(food);
                    foodDTO.Ingredients = new List<IngredientDTO>();
                    string[] strArr = fo.Ingredients.Split(", ");
                    foreach (var s in strArr)
                    {
                        foodDTO.Ingredients.Add(new IngredientDTO { Name = s });
                    }
                    foodDTO.Amount = fo.FoodAmount;
                    orderDto.OrderedFood.Add(foodDTO);
                }
                retList.Add(orderDto);
            }

            return retList;
        }

        public async Task<OrderDTO> TakeOrder(OrderTakeDTO data)
        {
            Order order = await _dbContext.Orders.SingleOrDefaultAsync(x => x.Id == data.OrderId);
            if(order == null)
                return null;
            if (order.Accepted == true)
                return null;
            
            User deliverer = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == data.DelivererEmail);

            order.Deliverer = deliverer;
            order.DelivererEmail = deliverer.Email;
            order.TimeAccepted = DateTime.Now;

            Random r = new Random();
            order.TimeDelivered = order.TimeAccepted?.AddMinutes(r.Next(15, 60));
            order.Accepted = true;
            try
            {
                _dbContext.SaveChanges();
            }
            catch(DbUpdateConcurrencyException ex)
            {
                Console.WriteLine(ex.Message);
                // Order allready being taken
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return _mapper.Map<OrderDTO>(order);
            
        }
    }
}
