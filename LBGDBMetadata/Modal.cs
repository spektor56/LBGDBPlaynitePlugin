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
    }
}
