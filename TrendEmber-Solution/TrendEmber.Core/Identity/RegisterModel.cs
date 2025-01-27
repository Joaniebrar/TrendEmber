
namespace TrendEmber.Core.Identity
{
    public class RegisterModel
    {
        public RegisterModel(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
    }
}
