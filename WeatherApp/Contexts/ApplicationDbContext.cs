﻿using Microsoft.EntityFrameworkCore;
using WeatherApp.Models.Entities;

namespace WeatherApp.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<EmailMessage> EmailMessages { get; set; }
        public DbSet<EmailVerification> EmailVerifications { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
