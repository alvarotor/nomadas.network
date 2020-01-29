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

        private readonly string[] Summaries = new[]
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
                    Summary = Summaries[rng.Next(Summaries.Length)],
                    RandomString = RandomString(10)
                })
                .ToArray()
            );
        }

        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}