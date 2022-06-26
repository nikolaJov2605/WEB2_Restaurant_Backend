using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT
{
    public class UserService : IUser
    {
        private readonly IConfigurationSection _secretKey;
        private readonly IMapper _mapper;
        private readonly RestaurantDbContext _dbContext;

        public UserService(IMapper mapper, RestaurantDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _secretKey = config.GetSection("SecretKey");
        }

        public Task<TokenDTO> Login(UserLoginDTO userLogin)
        {
            throw new System.NotImplementedException();
        }

        public async Task Register(UserDTO userDTO)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == userDTO.Email);
            if (user != null)
            {
                return;
            }

            User userToAdd = _mapper.Map<User>(userDTO);
            userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            _dbContext.Users.Add(userToAdd);
            await _dbContext.SaveChangesAsync();
        }
    }
}
