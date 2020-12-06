using Microsoft.EntityFrameworkCore;
using Shortener.Data.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Shortener.Interfaces
{
    public interface IDbContext
    {
        DbSet<ShortUrl> ShortUrls { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
