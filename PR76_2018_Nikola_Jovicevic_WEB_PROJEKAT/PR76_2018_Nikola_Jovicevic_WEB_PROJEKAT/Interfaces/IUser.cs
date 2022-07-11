using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IUser
    {
        Task<TokenDTO> Login(UserLoginDTO userLogin);
        Task Register(UserDTO userDTO);
        Task<UserDTO> GetUserByEmail(string email);
        Task UpdateUser(UserDTO userDTO);
        Task<List<UserDTO>> GetAllDeliverers();
        Task<bool> VerifyDeliverer(VerificationDTO verification);
        Task<bool> UnverifyDeliverer(VerificationDTO verification);
        Task<bool> DenyDeliverer(VerificationDTO verification);
        Task<string> GetUsersEmail(string username);
        Task<byte[]> GetUserImage(string email);
    }
}
