using AutoMapper;
using Microsoft.AspNetCore.Http;
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
using System.IO;
using System.IO.Pipelines;
using System.Linq;
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

        private static string dataPath;

        public UserService(IMapper mapper, RestaurantDbContext dbContext, IConfiguration config)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _secretKey = config.GetSection("SecretKey");

            dataPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT\\Data\\";
        }

        public async Task<bool> DenyDeliverer(VerificationDTO verification)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == verification.Username);
            if (user == null)
            {
                return false;
            }

            user.Denied = true;
            user.Verified = false;

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<List<UserDTO>> GetAllDeliverers()
        {
            List<User> allDeliverers = await _dbContext.Users.Where(x => x.UserType == "deliverer").ToListAsync();
            if (allDeliverers == null)
            {
                return null;
            }
            List<UserDTO> retList = _mapper.Map<List<UserDTO>>(allDeliverers);
            return retList;
        }

        public async Task<UserDTO> GetUserByEmail(string email)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }


            UserDTO retUser = _mapper.Map<UserDTO>(user);

            string imagesFilePath = dataPath + "UserImages\\";
            string defaultImageFilePath = imagesFilePath + "default-profile-picture.jpg";


            string filePath = user.ImageFilePath;
            string userFolder = Path.GetDirectoryName(filePath);

            if(Directory.Exists(userFolder))
            {
                if(File.Exists(filePath))
                {
                    using (Stream fileStream = new FileStream(filePath, FileMode.Open))
                    {
                        string fileName = Path.GetFileName(filePath);
                        IFormFile file = new FormFile(fileStream, 0, fileStream.Length, fileName, filePath)
                        {
                            Headers = new HeaderDictionary(),
                            ContentType = "application/json"
                        };
                        retUser.Image = file;
                    }
                }
            }

            return retUser;
        }

        public async Task<string> GetUsersEmail(string username)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == username); ;

            return user.Email;
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

            string imagesFilePath = dataPath + "UserImages\\";
            if (userDTO.Image == null)
            {
                imagesFilePath += "default-profile-picture.jpg";
                userToAdd.ImageFilePath = imagesFilePath;
            }
            else
            {
                string newFolderPath = imagesFilePath + userDTO.Email;
                if(!Directory.Exists(newFolderPath))
                {
                    Directory.CreateDirectory(newFolderPath);

                    if (userDTO.Image.Length > 0)
                    {
                        string filePath = Path.Combine(newFolderPath, userDTO.Image.FileName);
                        using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await userDTO.Image.CopyToAsync(fileStream);
                        }
                        userToAdd.ImageFilePath = filePath;
                    }
                }
            }

            userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            _dbContext.Users.Add(userToAdd);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> UnverifyDeliverer(VerificationDTO verification)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == verification.Username);
            if (user == null)
            {
                return false;
            }

            user.Verified = false;

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task UpdateUser(UserDTO userDTO)// ooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooovdeeeeeeeeeeee radimo
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == userDTO.Email);
            if (user == null)
            {
                return;
            }
            User updatedUser = _mapper.Map<User>(userDTO);

            user.UserName = updatedUser.UserName;
            user.Name = updatedUser.Name;
            user.LastName = updatedUser.LastName;
            user.Address = updatedUser.LastName;

            string imagesFilePath = dataPath + "UserImages\\";
            if(userDTO.Image != null)
            {
                if (string.IsNullOrEmpty(user.ImageFilePath))
                {
                    imagesFilePath += "default-profile-picture.jpg";
                    user.ImageFilePath = imagesFilePath;
                }
                else
                {
                    string newFolderPath = imagesFilePath + userDTO.Email;
                    if (!Directory.Exists(newFolderPath))
                    {
                        Directory.CreateDirectory(newFolderPath);

                        if (userDTO.Image.Length > 0)
                        {
                            string filePath = Path.Combine(newFolderPath, userDTO.Image.FileName);
                            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await userDTO.Image.CopyToAsync(fileStream);
                            }
                            user.ImageFilePath = filePath;
                        }
                    }
                    else
                    {
                        if (userDTO.Image.Length > 0)
                        {
                            File.Delete(user.ImageFilePath);
                            string filePath = Path.Combine(newFolderPath, userDTO.Image.FileName);
                            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await userDTO.Image.CopyToAsync(fileStream);
                            }
                            user.ImageFilePath = filePath;
                        }
                    }
                }
            }
            

            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> VerifyDeliverer(VerificationDTO verification)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.UserName == verification.Username);
            if (user == null)
            {
                return false;
            }

            user.Verified = true;
            user.Denied = false;
            
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

        }

        public async Task<byte[]> GetUserImage(string email)
        {
            User user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                return null;
            }

            byte[] retArray = { };

            string imagesFilePath = dataPath + "UserImages\\";
            string defaultImageFilePath = imagesFilePath + "default-profile-picture.jpg";


            string filePath = user.ImageFilePath;
            string userFolder = Path.GetDirectoryName(filePath);

            if (Directory.Exists(userFolder))
            {
                if (File.Exists(filePath))
                {
                    retArray = File.ReadAllBytes(filePath);
                }
            }

            return retArray;
        }
    }
}
