using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nomadas.Network.Models
{
    [Table("WeatherForecast")]
    public class WeatherForecast : BaseEntity
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [Required(ErrorMessage = "Summary is required")]
        public string Summary { get; set; }

        public string RandomString { get; set; }
    }
}
