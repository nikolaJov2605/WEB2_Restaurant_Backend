using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using System.Threading.Tasks;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces
{
    public interface IUser
    {
        Task<TokenDTO> Login(UserLoginDTO userLogin);
        Task Register(UserDTO userDTO);
    }
}
