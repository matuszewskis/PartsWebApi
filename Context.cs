using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

namespace WebApi
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options)
            : base(options)
        {
        }

        public DbSet<Part> Parts { get; set; }

        public DbSet<DestinationType?> DestinationTypes { get; set; }
    }
}