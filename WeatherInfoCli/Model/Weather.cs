namespace WeatherInfoCli.Model
{
    public record Weather
    {
        public decimal Temperature { get; set; }
        public decimal Windspeed { get; set; }
        public decimal Winddirection { get; set; }
    }
}