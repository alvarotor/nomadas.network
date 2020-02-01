using System;
using System.Linq;
using System.Threading.Tasks;
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

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).Created = DateTime.UtcNow;
                }
                ((BaseEntity)entry.Entity).Modified = DateTime.UtcNow;
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var rng = new Random();

            modelBuilder.Entity<WeatherForecast>().HasData(
                Enumerable.Range(1, 5).Select(index => new WeatherForecast
                {
                    Id = index,
                    Created = DateTime.Now,
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = "Summary " + Summaries[rng.Next(Summaries.Length)],
                    RandomString = RandomString(10)
                })
                .ToArray()
            );
        }

        private string RandomString(int length)
        {
            var rng = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[rng.Next(s.Length)]).ToArray());
        }
    }
}