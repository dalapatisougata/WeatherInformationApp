namespace WeatherInfoCli.Model
{
    public record Location
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Elevation { get; set; }

        public Weather Current_Weather { get; set; }
    }
}