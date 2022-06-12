using Microsoft.EntityFrameworkCore;
using MVC_ViewModels_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_ViewModels_Data.Data
{
    public class ExDbContext : DbContext
    {
        public ExDbContext(DbContextOptions<ExDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(p => p.ContactNumber).HasMaxLength(50).HasDefaultValue(string.Empty);

            //countries
            Country England = new Country() { CountryId = 1, Name = "England" };
            Country Sweden = new Country() { CountryId = 2, Name = "Sweden" };
            modelBuilder.Entity<Country>().HasData(England, Sweden);

            //cities
            City London = new City() {  CityId = 1, Name = "London", CountryId = England.CountryId };
            City Birmingham = new City() { CityId = 2, Name = "Birmingham", CountryId = England.CountryId };
            City Stockholm = new City() { CityId = 3, Name = "Stockholm", CountryId = Sweden.CountryId };
            City Gothenburg = new City() { CityId = 4, Name = "Gothenburg", CountryId = Sweden.CountryId };
            modelBuilder.Entity<City>().HasData(London, Birmingham, Stockholm, Gothenburg);

            // Persons
            Person priya = new Person() { PersonId = 1, Name = "Priya", ContactNumber = "034-4242-657", CityId = London.CityId };
            Person Keerthi = new Person() { PersonId = 2, Name = "Keerthi", ContactNumber = "034-4242-658", CityId = Gothenburg.CityId };
            modelBuilder.Entity<Person>().HasData(priya, Keerthi);

            Language Swedish = new Language() { LanguageID = 1,  Name = "Swedish" };
            Language English = new Language() { LanguageID = 2, Name = "English" };
            modelBuilder.Entity<Language>().HasData(Swedish, English);
            LanguagePeople priyaswedish = new LanguagePeople() { LanguageID = Swedish.LanguageID, PersonId = priya.PersonId };
            modelBuilder.Entity<LanguagePeople>().HasData(priyaswedish);

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<City>()
                .HasOne(e => e.Country)
                .WithMany(e => e.Cities);

            modelBuilder.Entity<Person>()
                .HasOne(e => e.City)
                .WithMany(e => e.Peoples);

            modelBuilder.Entity<Country>()
                .HasMany(e => e.Cities)
                .WithOne(e => e.Country);

            modelBuilder.Entity<LanguagePeople>()
                .HasKey(e => new { e.LanguageID, e.PersonId});

            modelBuilder.Entity<LanguagePeople>()
                .HasOne(e => e.Language)
                .WithMany(e => e.LanguagePeople)
                .HasForeignKey(e => e.LanguageID);

            modelBuilder.Entity<LanguagePeople>()
                .HasOne(e => e.Person)
                .WithMany(e => e.LanguagePeople)
                .HasForeignKey(e => e.PersonId);
        }
        public DbSet<Person> Person { get; set; }
        public DbSet<City> cities { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<LanguagePeople> LanguagePeople { get; set; }
    }
}
