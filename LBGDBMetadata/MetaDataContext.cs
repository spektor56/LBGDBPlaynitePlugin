using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using LBGDBMetadata.LaunchBox.Metadata;
using Microsoft.EntityFrameworkCore;

namespace LBGDBMetadata
{
    internal class MetaDataContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite(
                $@"Data Source={Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lbgdb.db")}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameImage>()
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Game>()
            .HasIndex(p => new { p.PlatformSearch, p.NameSearch });

            modelBuilder.Entity<GameAlternateName>()
            .HasIndex(p => new { p.NameSearch });

            modelBuilder.Entity<GameAlternateName>()
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            /*
            modelBuilder.Entity<Metadata.GameAlternateName>()
                .HasKey(c => new { c.DatabaseID, c.Region });
                */
        }
    }
}
