using System.Text.Json.Serialization;

namespace WeatherInfoCli.Model
{
    internal record City
    {
        [JsonPropertyName("lat")]
        public string Latitude { get; set; }
        [JsonPropertyName("lng")]
        public string Longitude { get; set; }
        [JsonPropertyName("city")]
        public string Name { get; set; }
    }
}
