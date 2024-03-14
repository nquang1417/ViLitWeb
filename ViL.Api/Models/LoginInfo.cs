
namespace ViL.Api.Models
{
    public class LoginInfo
    {
        public string username { get; set; }
        public string password { get; set; }
        public LoginInfo(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
