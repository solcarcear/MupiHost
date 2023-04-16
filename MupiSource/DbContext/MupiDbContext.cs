using Microsoft.EntityFrameworkCore;
using MupiSource.Model;

namespace MupiSource.DbContext
{
    public class MupiDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public MupiDbContext(DbContextOptions options)
       : base(options)
        {
        }

        public DbSet<Contact> Contact { get; set; }


    }
}
