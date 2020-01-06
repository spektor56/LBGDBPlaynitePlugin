using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace LBGDBMetadata
{
    public class MetaDataContext : DbContext
    {
        public DbSet<Metadata.Game> Games { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=lbgdb.db");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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
