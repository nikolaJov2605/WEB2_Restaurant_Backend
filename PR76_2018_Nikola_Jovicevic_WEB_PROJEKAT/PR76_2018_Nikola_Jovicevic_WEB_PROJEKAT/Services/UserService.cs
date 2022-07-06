using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Services
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

        public async Task<UserDTO> GetUserByUsername(string username)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == username);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<TokenDTO> Login(UserLoginDTO userLogin)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == userLogin.Email);
            if (user == null)
                return null;

            if(BCrypt.Net.BCrypt.Verify(userLogin.Password, user.Password))
            {
                List<Claim> claims = new List<Claim>();
                if (user.UserType == "admin")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));
                }
                if (user.UserType == "deliverer")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "deliverer"));
                }
                if(user.UserType == "customer")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "customer"));
                }

                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "http://localhost:44302", //url servera koji je izdao token
                    claims: claims, //claimovi
                    expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                    signingCredentials: signinCredentials //kredencijali za potpis
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                
                return new TokenDTO { Token = tokenString };
            }

            return null;
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

        public async Task UpdateUser(UserDTO userDTO)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == userDTO.Email);
            if (user == null)
            {
                return;
            }
            User updatedUser = _mapper.Map<User>(userDTO);
            updatedUser.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            user = updatedUser;
            await _dbContext.SaveChangesAsync();
        }
    }
}
