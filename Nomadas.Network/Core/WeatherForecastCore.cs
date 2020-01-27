using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Models;

namespace Nomadas.Network.Core
{
    public class WeatherForecastCore : RepositoryBase<WeatherForecast>, IWeatherForecastCore
    {
        public WeatherForecastCore(DBContext repositoryContext) : base(repositoryContext) { }

        public async Task<IEnumerable<WeatherForecast>> GetAll()
        {
            return await FindAll()
                .OrderBy(o => o.Summary)
                .ToListAsync();
        }
    }
}