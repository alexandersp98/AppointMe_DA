﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Principal;

namespace Persistence
{
    public class ApplicationDbContext : DbContext
    {


        IConfiguration _config;

        public IConfiguration Configuration { get { return _config; } }

        /// <summary>
        /// Parameterloser Konstruktor liest Connection String aus appsettings.json (zur Designzeit)
        /// Für Migration-Erzeugung! Achtung Konstruktor muss an 1. Stelle der Konstruktoren stehen
        /// </summary>
        public ApplicationDbContext()
        {
            var builder = new ConfigurationBuilder()
                        .SetBasePath(Environment.CurrentDirectory).AddJsonFile
                        ("appsettings.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                        optional: true, reloadOnChange: true);

            _config = builder.Build();
        }

        public ApplicationDbContext(IConfiguration configuration) : base()
        {
            _config = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = _config["ConnectionStrings:DefaultConnection"];
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
