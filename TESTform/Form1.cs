using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LBGDBMetadata;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox.Metadata;

namespace TESTform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        



        private void button1_Click(object sender, EventArgs e)
        {
            var zipFile = @"C:\zipTest\MetaData.zip";

            using (var zipArchive = ZipFile.Open(zipFile, ZipArchiveMode.Read))
            {
                var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                    entry.Name.Equals("MetaData.xml", StringComparison.OrdinalIgnoreCase));

                if (metaData != null)
                    using (var metaDataStream = metaData.Open())
                    {
                        var games = metaDataStream.AsEnumerableXml("Game");
                        var xmlSerializer = new XmlSerializer(typeof(Game));

                        var i = 0;
                        var context = new MetaDataContext();
                        context.ChangeTracker.AutoDetectChangesEnabled = false;

                        foreach (var xElement in games)
                        {
                            var gameMetaData = (Game) xmlSerializer.Deserialize(xElement.CreateReader());
                            i++;
                            if (i++ > 1000)
                            {
                                context.SaveChanges();
                                i = 0;
                                context.Dispose();
                                context = new MetaDataContext();
                                context.ChangeTracker.AutoDetectChangesEnabled = false;
                            }

                            context.Games.Add(gameMetaData);
                        }

                        context.Dispose();
                    }
            }
        }
    }
}