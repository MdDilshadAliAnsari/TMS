using System.Collections.Generic;
using System.Reflection.Emit; 
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using TMS.Authentication.Migrations;

namespace TMS.Authentication.Model
{
    public class TMSDbContext : DbContext, IDisposable
    {
        public TMSDbContext()
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {   optionsBuilder.UseSqlServer("DbConnection"); 
                base.OnConfiguring(optionsBuilder);
            }
            
           


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("TMS");
            modelBuilder.Entity<user>().HasKey(am => new
            {
                am.USERID
            });
            modelBuilder.Entity<Role>().HasKey(am => new
            {
                am.ROLEID
            });
            base.OnModelCreating(modelBuilder);

            #region Feed Data
            modelBuilder.Entity<user>().HasData(
         new user
         {
             USERID             = 1,
             USERNAME           = "Dilshad123", 
             PWD                = "123",
             ROLEID             = 1,
             FIRSTNAME          = "Dilshad",
             LASTNAME           = "ANSARI",
             EMAILID            = "dilshadali.md@navayugainfotech.com",
             MOBILENO           = "9911859088",
             PWDEXPIRTYDATE     = new DateTime(2025, 09, 30),
             INCORRECTLOGINS    = 0,
             LOGINTIME          = new DateTime(2025, 04, 30),
             DORMANTSTATUS      = 0,
             ISDELETED          = 0,
             CREATEDBY          = 1,
             CREATEDON          = new DateTime(2025, 04, 30),
             UPDATEDBY          = 1,
             UPDATEDON          = new DateTime(2025, 04, 30)

         }, new user
         {
             USERID = 2,
             USERNAME = "Venkat123",
             PWD = "123",
             ROLEID =2,
             FIRSTNAME = "Venkat",
             LASTNAME = "Tilla",
             EMAILID = "venkataramana.tilla@navayugainfotech.com",
             MOBILENO = "9911834023",
             PWDEXPIRTYDATE = new DateTime(2025, 09, 30),
             INCORRECTLOGINS = 0,
             LOGINTIME = new DateTime(2025, 04, 30),
             DORMANTSTATUS = 0,
             ISDELETED = 0,
             CREATEDBY = 1,
             CREATEDON = new DateTime(2025, 04, 30),
             UPDATEDBY = 1,
             UPDATEDON = new DateTime(2025, 04, 30)
         }, new user
         {
             USERID = 3,
             USERNAME = "Manu123",
             PWD = "123",
             ROLEID = 3,
             FIRSTNAME = "Venkat",
             LASTNAME = "Tilla",
             EMAILID = "manohar.panjala@navayugainfotech.com",
             MOBILENO = "9911675433",
             PWDEXPIRTYDATE = new DateTime(2025, 09, 30),
             INCORRECTLOGINS = 0,
             LOGINTIME = new DateTime(2025, 04, 30),
             DORMANTSTATUS = 0,
             ISDELETED = 0,
             CREATEDBY = 1,
             CREATEDON = new DateTime(2025, 04, 30),
             UPDATEDBY = 1,
             UPDATEDON = new DateTime(2025, 04, 30)
         }, new user
         {
             USERID = 4,
             USERNAME = "Venkat123",
             PWD = "123",
             ROLEID = 4,
             FIRSTNAME = "Karuna",
             LASTNAME = "Babu",
             EMAILID = "karunababu.kondisetti@navayugainfotech.com",
             MOBILENO = "9911834023",
             PWDEXPIRTYDATE = new DateTime(2025, 09, 30),
             INCORRECTLOGINS = 0,
             LOGINTIME = new DateTime(2025, 04, 30),
             DORMANTSTATUS = 0,
             ISDELETED = 0,
             CREATEDBY = 1,
             CREATEDON = new DateTime(2025, 04, 30),
             UPDATEDBY = 1,
             UPDATEDON = new DateTime(2025, 04, 30)
         });


            modelBuilder.Entity<Role>().HasData(
          new Role
          {
              ROLEID = 1,
              ROLENAME = "Super Admin",
              ROLEDESC = "It has all access right",
              ISDELETED = 0,
              CREATEDBY = 1,
              CREATEDON = new DateTime(2025, 04, 30),
              UPDATEDBY = 1,
              UPDATEDON = new DateTime(2025, 04, 30)

          }, new Role
          {
              ROLEID = 2,
              ROLENAME = "Admin",
              ROLEDESC = "It has all access right.",
              ISDELETED = 0,
              CREATEDBY = 1,
              CREATEDON = new DateTime(2025, 04, 30),
              UPDATEDBY = 1,
              UPDATEDON = new DateTime(2025, 04, 30)
          }, new Role
          {
              ROLEID = 3,
              ROLENAME = "Customer",
              ROLEDESC = "Customer access right to see their task and progress and create ticket as well.",
              ISDELETED = 0,
              CREATEDBY = 1,
              CREATEDON = new DateTime(2025, 04, 30),
              UPDATEDBY = 1,
              UPDATEDON = new DateTime(2025, 04, 30)
          }, new Role
          {
              ROLEID = 4,
              ROLENAME = "Developer",
              ROLEDESC = "Developer access right to see their task.",
              ISDELETED = 0,
              CREATEDBY = 1,
              CREATEDON = new DateTime(2025, 04, 30),
              UPDATEDBY = 1,
              UPDATEDON = new DateTime(2025, 04, 30)
          });
            #endregion

            modelBuilder.Entity<UserIdResultDTO>().HasNoKey();
            modelBuilder.Entity<ASSIGNEDEMAILDTO>().HasNoKey();
        }
        public TMSDbContext(DbContextOptions<TMSDbContext> options) : base(options)
        {

        }
        public virtual DbSet<user> USERS { get; set; }
        public virtual DbSet<Role> ROLES { get; set; }
        public virtual DbSet<AuthTokens> AuthToken { get; set; }
        public DbSet<UserIdResultDTO> UserIdResultDTO { get; set; }
        public DbSet<ASSIGNEDEMAILDTO> ASSIGNEDEMAILDTO { get; set; }




    }
}
