using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nomadas.Network.Models;

namespace Nomadas.Network
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<WeatherForecast> DBItems { get; set; }

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var rng = new Random();

            modelBuilder.Entity<WeatherForecast>().HasData(
                Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Id = index,
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]
                })
                .ToArray()
            );
        }
    }
}