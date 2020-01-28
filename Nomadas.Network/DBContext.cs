using System;
using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Models;

namespace Nomadas.Network
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<WeatherForecast> DBItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var rng = new Random();
            modelBuilder.Entity<WeatherForecast>().HasData(
                new { Id = 1, Date = DateTime.Now.AddDays(1), Summary = "First post", TemperatureC = rng.Next(-20, 55) },
                new { Id = 2, Date = DateTime.Now.AddDays(2), Summary = "Second post", TemperatureC = rng.Next(-20, 55) });
        }
    }
}