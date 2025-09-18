using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace TMS.DATA.Model
{
    public class TMSDbContext : DbContext, IDisposable
    {
        public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
        {
        }
        public virtual DbSet<MailData> tblEmails { get; set; }
        public virtual DbSet<MailAttachment> tblMailAttachment { get; set; }
        public TMSDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DbConnection");
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            object value = modelBuilder.HasDefaultSchema("TMS");
            modelBuilder.Entity<MailData>().HasKey(am => new
            {
                am.MailDataId
            });
            modelBuilder.Entity<MailAttachment>().HasKey(am => new
            {
                am.MailAttachmentId
            });
            modelBuilder.Entity<MailData>().Property(l => l.MailDataId).ValueGeneratedOnAdd();
            modelBuilder.Entity<MailAttachment>().Property(l => l.MailAttachmentId).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }






    }
}
