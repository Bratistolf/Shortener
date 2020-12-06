using Microsoft.EntityFrameworkCore;
using Shortener.Data.Entities;
using Shortener.Interfaces;

namespace Shortener.Data
{
    public class ApplicationDbContext : DbContext, IDbContext
    {
        public DbSet<ShortUrl> ShortUrls { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
