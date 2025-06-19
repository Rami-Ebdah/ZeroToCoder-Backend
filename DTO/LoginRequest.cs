using System.ComponentModel.DataAnnotations;
namespace ZeroToCoder.Dto


{
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
