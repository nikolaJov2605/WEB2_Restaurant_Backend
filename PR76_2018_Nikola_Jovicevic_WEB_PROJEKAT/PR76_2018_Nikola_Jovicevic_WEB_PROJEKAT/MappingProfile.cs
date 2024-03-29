﻿using AutoMapper;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, User>().ReverseMap();
            CreateMap<Food, FoodDTO>().ReverseMap();
            CreateMap<Food, FoodUploadDTO>().ReverseMap();
            CreateMap<Ingredient, IngredientDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
        }
    }
}
