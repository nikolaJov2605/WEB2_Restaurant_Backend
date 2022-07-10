using System;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.DTOs
{
    public class UserDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public bool Verified { get; set; }
        public bool Denied { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
