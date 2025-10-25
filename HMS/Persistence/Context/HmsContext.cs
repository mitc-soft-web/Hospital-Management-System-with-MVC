using Microsoft.EntityFrameworkCore;

namespace HMS.Persistence.Context
{
    public class HmsContext : DbContext
    {
        public HmsContext(DbContextOptions<HmsContext> options) : base(options)
        {
          

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

        }
    }
}
