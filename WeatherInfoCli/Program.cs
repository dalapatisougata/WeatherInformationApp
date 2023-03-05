using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using WeatherInfoCli.Helper;
using WeatherInfoCli.Model;

namespace WeatherInfoCli
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            JsonDataReaderHelper.ReadCityDataFromJson("Data/in.json");

            var cityOption = new Option<string?>(new string[] { "-c", "--city" },
                                    description: "Enter city name to get weather information",
                                    getDefaultValue: () => "Kolkata");
            RootCommand rootCommand = new RootCommand("Weather information command line tool");
            Command subCommandShowCity = new Command("ShowCities", "Show list of cities");
            Command subCommandShowWeather = new Command("ShowWeather", "Show weather of a city");
            subCommandShowWeather.AddOption(cityOption);
            rootCommand.AddOption(cityOption);
            rootCommand.Add(subCommandShowCity);
            rootCommand.Add(subCommandShowWeather);

            rootCommand.SetHandler((city) =>
            {
                Console.WriteLine($"Welcome from command line, you entered: {city}");
                HandleCommandAsync(city).Wait();
            }, cityOption);
            subCommandShowCity.SetHandler(() =>
            {
                Console.WriteLine("Available Cities:");
                DisplayAllCitiesAsync(JsonDataReaderHelper.GetCityList()).Wait();
            });
            subCommandShowWeather.SetHandler((city) =>
            {
                Console.WriteLine($"Showing weather of city: {city}");
                HandleCommandAsync(city).Wait();
            }, cityOption);

            var commandBuilder = new CommandLineBuilder(rootCommand).UseDefaults();
            var parser = commandBuilder.Build();
            await parser.InvokeAsync(args).ConfigureAwait(false);
        }

        static async Task HandleCommandAsync(string cityName)
        {
            if (cityName == null)
            {
                await Console.Out.WriteLineAsync("Please enter city name").ConfigureAwait(false);
            }
            else
            {
                Location location = await WeatherHelper.GetWeatherAsync(cityName).ConfigureAwait(false);
                await DisplayWeatherAsync(cityName, location).ConfigureAwait(false);
            }

        }

        static async Task DisplayAllCitiesAsync(List<City> cities)
        {
            foreach (var city in cities)
            {
                await Console.Out.WriteLineAsync(city.Name);
            }
        }

        static async Task DisplayWeatherAsync(string city, Location location)
        {
            await Console.Out.WriteAsync($"City Name: {city}{Environment.NewLine}" +
                $"latitude: {location.Latitude}{Environment.NewLine}" +
                $"longitude: {location.Longitude}{Environment.NewLine}" +
                $"elevation: {location.Elevation}{Environment.NewLine}" +
                $"temperature: {location.Current_Weather.Temperature}{Environment.NewLine}" +
                $"windspeed: {location.Current_Weather.Windspeed}{Environment.NewLine}" +
                $"winddirection: {location.Current_Weather.Winddirection}").ConfigureAwait(false);
        }
    }
}