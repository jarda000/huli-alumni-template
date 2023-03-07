namespace WeatherApp.Interfaces
{
    public interface ICityService
    {
        void AddCityToMyList(int id, string input);
        bool ValidCity(string input);
    }
}