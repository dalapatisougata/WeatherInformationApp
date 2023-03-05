using System.Net.Http.Json;
using WeatherInfoCli.Model;

namespace WeatherInfoCli.Helper
{
    internal static class WeatherHelper
    {
        internal static async Task<Location> GetWeatherAsync(string cityName)
        {
            string path = "/v1/forecast?latitude={0}&longitude={1}&current_weather=true";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.open-meteo.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var cityLocation = JsonDataReaderHelper.GetCityLatitudeLongitude(cityName);
            path = string.Format(path, cityLocation.latitude, cityLocation.longitude);
            return await client.GetFromJsonAsync<Location>(path).ConfigureAwait(false);
        }
    }
}