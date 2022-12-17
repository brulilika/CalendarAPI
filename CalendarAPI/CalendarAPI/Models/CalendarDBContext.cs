using System;
using CalendarAPI.Models;
using CalendarAPI.Authentication.Model;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Database
{
	public class CalendarDBContext : DbContext
	{
		public CalendarDBContext() : base ()
		{
		}

        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("CalendarConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

