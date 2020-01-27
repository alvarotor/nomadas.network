using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Models;

namespace Nomadas.Network
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<WeatherForecast> DBItems { get; set; }
    }
}