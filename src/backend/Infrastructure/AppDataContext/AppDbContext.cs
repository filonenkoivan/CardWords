using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AppDataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserStats> UserStats { get; set; }
        public DbSet<CardCollection> CardCollections { get; set; }
        public DbSet<Card> Cards { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserStatsConfiguration());
            modelBuilder.ApplyConfiguration(new CardCollectionConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            base.OnModelCreating(modelBuilder);
        }

    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(x => x.Stats).WithOne(x => x.User).HasForeignKey<UserStats>(x => x.UserId);
        builder.HasMany(x => x.Collections).WithOne(x => x.User).HasForeignKey(x => x.UserId);
    }
}
public class UserStatsConfiguration : IEntityTypeConfiguration<UserStats>
{
    public void Configure(EntityTypeBuilder<UserStats> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
public class CardCollectionConfiguration : IEntityTypeConfiguration<CardCollection>
{
    public void Configure(EntityTypeBuilder<CardCollection> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasMany(x => x.CardList).WithOne(x => x.CardCollection).HasForeignKey(x => x.CardCollectionId);
    }
}

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasOne(c => c.CardCollection).WithMany(x => x.CardList).HasForeignKey(x => x.CardCollectionId);
    }
}