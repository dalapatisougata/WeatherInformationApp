using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using WeatherInfoCli.Model;

namespace WeatherInfoCli
{
    internal class Program
    {
        static List<City> cityList = default;
        static async Task Main(string[] args)
        {
            cityList = ReadCityDataFromJson("Data/in.json");
            Console.WriteLine("Get city weather information!");
            await Console.Out.WriteAsync("Enter city name:");
            string? cityName = Console.ReadLine();
            if (cityName == null)
            {
                await Console.Out.WriteLineAsync("Please enter city name");
            }
            else
            {
                Location location = await GetWeatherAsync(cityName).ConfigureAwait(false);
                DisplayWeather(cityName, location);
            }
        }

        static async Task<Location> GetWeatherAsync(string cityName)
        {
            string path = "/v1/forecast?latitude={0}&longitude={1}&current_weather=true";
            Location location = default;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://api.open-meteo.com/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            var cityLocation = GetCityLatitudeLongitude(cityName);
            path = string.Format(path, cityLocation.latitude, cityLocation.longitude);
            location = await client.GetFromJsonAsync<Location>(path).ConfigureAwait(false);
            return location;
        }

        static (string? latitude, string? longitude) GetCityLatitudeLongitude(string cityName)
        {
            var city = cityList.Where(x => IsStringEqual(x.Name, cityName)).FirstOrDefault();
            return (city?.Latitude, city?.Longitude);
        }

        static bool IsStringEqual(string value1, string value2) => String.Compare(value1, value2, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
        static void DisplayWeather(string city, Location location)
        {
            Console.WriteLine($"City Name: {city}{Environment.NewLine}" +
                $"latitude: {location.Latitude}{Environment.NewLine}" +
                $"longitude: {location.Longitude}{Environment.NewLine}" +
                $"elevation: {location.Elevation}{Environment.NewLine}" +
                $"temperature: {location.Current_Weather.Temperature}{Environment.NewLine}" +
                $"windspeed: {location.Current_Weather.Windspeed}{Environment.NewLine}" +
                $"winddirection: {location.Current_Weather.Winddirection}");
        }

        static List<City> ReadCityDataFromJson(string fileName)
        {
            var CityList = File.ReadAllText(fileName);
            var data = JsonSerializer.Deserialize<List<City>>(CityList);
            return data;
        }
    }

    public record Weather
    {
        public decimal Temperature { get; set; }
        public decimal Windspeed { get; set; }
        public decimal Winddirection { get; set; }
    }

    public record Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }

        public Weather Current_Weather { get; set; }
    }
}