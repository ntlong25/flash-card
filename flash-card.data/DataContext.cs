using flash_card.data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace flash_card.data
{
    public class DataContext : IdentityDbContext<User, Role, int>
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region Entities
        public override DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<FlashCard> FlashCards { get; set; }
        public override DbSet<Role> Roles { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Fluent API
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<User>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<User>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(u => u.Status).IsRequired().HasDefaultValue(true);
            modelBuilder.Entity<User>().HasOne<Role>(s => s.Role)
               .WithMany(g => g.Users)
               .HasForeignKey(x => x.RoleId)
               .OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Role>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Topic>().ToTable("Topic");
            modelBuilder.Entity<Topic>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Topic>()
               .HasKey(s => s.Id);
            modelBuilder.Entity<Topic>()
                .HasOne<User>(s => s.User)
                .WithMany(g => g.Topics)
                .HasForeignKey(s => s.UserId);

            modelBuilder.Entity<FlashCard>().ToTable("FlashCard");
            modelBuilder.Entity<FlashCard>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<FlashCard>()
                .HasKey(s => s.Id);
            modelBuilder.Entity<FlashCard>()
                .HasOne<Topic>(s => s.Topic)
                .WithMany(g => g.FlashCards)
                .HasForeignKey(s => s.TopicId);
            #endregion

            // Remove AspNet prefix of tables: by default, tables in IdentityDbContext have
            // name with AspNet prefix like: AspNetUserRoles, AspNetUser ...
            // The following code runs when initializing the DbContext, creating the database will remove that prefix
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName[6..]);
                }
            }
        }
    }
}
