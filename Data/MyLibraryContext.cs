using Microsoft.EntityFrameworkCore;
using MyLibrary.api.Entities;

namespace MyLibrary.api.Data
{
    public class MyLibraryContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Serie> Series { get; set; }
        public MyLibraryContext(DbContextOptions options) : base(options) {}
    }
}