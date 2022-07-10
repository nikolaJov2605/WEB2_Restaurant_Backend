using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userService;
        private readonly IMail _mailService;

        public UserController(IUser userService, IMail mailService)
        {
            this._userService = userService;
            this._mailService = mailService;
        }

        [HttpGet("all-deliverers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserDTO> allDeliverers = await _userService.GetAllDeliverers();
            if(allDeliverers == null)
            {
                return NotFound();
            }
            return Ok(allDeliverers);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            await _userService.Register(userDTO);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLoginDTO)
        {
            var token = await _userService.Login(userLoginDTO);
            if (token == null)
            {
                return NotFound();
            }
            return Ok(token);
        }

        [HttpGet("{email}")]
        [Authorize(Roles = "admin, deliverer, customer")]
        public async Task<ActionResult> GetUserByEmail(string email)
        {
            UserDTO retUser = await _userService.GetUserByEmail(email);
            if(retUser == null)
            {
                return NotFound();
            }
            return Ok(retUser);
        }

        [HttpPost("update-user")]
        [Authorize(Roles ="admin, deliverer, customer")]
        public async Task<IActionResult> UpdateUser([FromBody] UserDTO userDTO)
        {
            await _userService.UpdateUser(userDTO);
            return Ok();
        }

        [HttpPost("verify-deliverer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> VerifyUser([FromBody] VerificationDTO verification)
        {
            bool retVal = await _userService.VerifyDeliverer(verification);
            if (retVal == false)
            {
                return NotFound();
            }

            string userEmail = await _userService.GetUsersEmail(verification.Username);
            if (string.IsNullOrEmpty(userEmail))
                return NotFound();

            _mailService.SendVerificationMail(userEmail, "approved");


            return Ok(retVal);
        }

        [HttpPost("unverify-deliverer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UnverifyDeliverer([FromBody] VerificationDTO verification)
        {
            bool retVal = await _userService.UnverifyDeliverer(verification);
            if (retVal == false)
            {
                return NotFound();
            }

            string userEmail = await _userService.GetUsersEmail(verification.Username);
            if (string.IsNullOrEmpty(userEmail))
                return NotFound();

            _mailService.SendVerificationMail(userEmail, "unverified");

            return Ok(retVal);
        }

        [HttpPost("deny-deliverer")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DenyDeliverer([FromBody] VerificationDTO verification)
        {
            bool retVal = await _userService.DenyDeliverer(verification);
            if (retVal == false)
            {
                return NotFound();
            }

            string userEmail = await _userService.GetUsersEmail(verification.Username);
            if (string.IsNullOrEmpty(userEmail))
                return NotFound();

            _mailService.SendVerificationMail(userEmail, "denied");

            return Ok(retVal);
        }


    }
}
