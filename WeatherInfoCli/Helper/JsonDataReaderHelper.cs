using System.Globalization;
using System.Text.Json;
using WeatherInfoCli.Model;

namespace WeatherInfoCli.Helper
{
    internal class JsonDataReaderHelper
    {
        static List<City> cityList = default;
        internal static void ReadCityDataFromJson(string fileName)
        {
            var cityJsonList = File.ReadAllText(fileName);
            cityList = JsonSerializer.Deserialize<List<City>>(cityJsonList);
        }
        internal static (string latitude, string longitude) GetCityLatitudeLongitude(string cityName)
        {
            var city = cityList.Where(x => IsStringEqual(x.Name, cityName)).FirstOrDefault();
            return (city?.Latitude, city?.Longitude);
        }
        static bool IsStringEqual(string value1, string value2) => String.Compare(value1, value2, CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0;
        internal static List<City> GetCityList() => cityList;
    }
}
