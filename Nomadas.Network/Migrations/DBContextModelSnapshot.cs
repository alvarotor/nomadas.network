﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nomadas.Network;

namespace Nomadas.Network.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nomadas.Network.Models.WeatherForecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("RandomString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TemperatureC")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("WeatherForecast");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Date = new DateTime(2020, 1, 30, 16, 16, 49, 241, DateTimeKind.Local).AddTicks(5335),
                            RandomString = "TMAZ0JB72H",
                            Summary = "Freezing",
                            TemperatureC = 43
                        },
                        new
                        {
                            Id = 2,
                            Date = new DateTime(2020, 1, 31, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7543),
                            RandomString = "AUIFTJQS6J",
                            Summary = "Scorching",
                            TemperatureC = -16
                        },
                        new
                        {
                            Id = 3,
                            Date = new DateTime(2020, 2, 1, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7860),
                            RandomString = "CLDKZ33UE8",
                            Summary = "Mild",
                            TemperatureC = 45
                        },
                        new
                        {
                            Id = 4,
                            Date = new DateTime(2020, 2, 2, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7897),
                            RandomString = "D8AKIQ7MYX",
                            Summary = "Cool",
                            TemperatureC = 13
                        },
                        new
                        {
                            Id = 5,
                            Date = new DateTime(2020, 2, 3, 16, 16, 49, 247, DateTimeKind.Local).AddTicks(7922),
                            RandomString = "IQIF8OB9VG",
                            Summary = "Scorching",
                            TemperatureC = 51
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
