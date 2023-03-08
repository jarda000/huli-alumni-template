using WeatherApp.Models.Entities;

namespace WeatherApp.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
        int GetUserFromJwtClaim(string jwt);
    }
}