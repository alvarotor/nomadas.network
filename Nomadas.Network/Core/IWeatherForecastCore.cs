
using System.Collections.Generic;
using System.Threading.Tasks;
using Nomadas.Network.Models;

namespace Nomadas.Network.Core
{
    public interface IWeatherForecastCore : IRepositoryBase<WeatherForecast>
    {
        Task<IEnumerable<WeatherForecast>> GetAll();
    }
}