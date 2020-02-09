using JWTbasedauthentication.Helper;
using JWTbasedauthentication.Models;

namespace JWTbasedauthentication.Managers
{
    public interface IUserManager
    {
        bool CompareCredentials(AppSettings appSettings, string requestUserName, string requestPassword);

        int RegisterUser(AppSettings appSettings, UserModel userModel);

        string GetAthorizeToken(string userName);
    }
}
