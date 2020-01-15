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

            modelBuilder.Entity<GameAlternateName>()
                .Property(p => p.ID)
                .ValueGeneratedOnAdd();
            /*
            modelBuilder.Entity<Metadata.GameAlternateName>()
                .HasKey(c => new { c.DatabaseID, c.Region });
                */
        }
        
        /*

        private Dictionary<string, string> _imageTypeDictionary = new Dictionary<string,string>();
         * var translationTable = XElement.Load(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"translationTable.xml"));

            var imageTypes = translationTable.Descendants("asset").Select(item =>  item);
            foreach (var imageType in imageTypes)
            {
                if (imageType.Element("key") != null && imageType.Element("value") != null &&
                            !string.IsNullOrWhiteSpace(imageType.Element("key").Value) &&
                            !string.IsNullOrWhiteSpace(imageType.Element("value").Value))
                {
                    _imageTypeDictionary.Add(imageType.Element("key").Value, imageType.Element("value").Value);
                    
                }
            }
         */
    }
}
