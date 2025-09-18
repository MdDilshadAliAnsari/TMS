using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Xml;

namespace TMS.Logger.Model
{  
    public class TMSDbContext : DbContext, IDisposable
    {

        public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
        {
        }

        public virtual DbSet<LOGGERS> LOGGER { get; set; }
        public TMSDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            object value = modelBuilder.HasDefaultSchema("TMS");
            modelBuilder.Entity<LOGGERS>().HasKey(am => new
            {
                am.LoggerId
            });

            modelBuilder.Entity<LOGGERS>().Property(l => l.LoggerId).ValueGeneratedOnAdd();
           
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }


    }
}
