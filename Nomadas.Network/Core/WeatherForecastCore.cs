using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nomadas.Network.Models;

namespace Nomadas.Network.Core
{
    public class WeatherForecastCore : RepositoryBase<WeatherForecast>, IWeatherForecastCore
    {
        public WeatherForecastCore(DBContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<WeatherForecast>> GetAllOrderBySummary()
        {
            var all = await FindAll(); 
            return all
                .OrderBy(o => o.Summary)
                .ToList();
        }
    }
}