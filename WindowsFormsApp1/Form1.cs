using LBGDBMetadata;
using LBGDBMetadata.LaunchBox.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using EFCore.BulkExtensions;
using LBGDBMetadata.Extensions;
using System.IO.Compression;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async Task<bool> ImportXml<T>(Stream metaDataStream, int bufferSize) where T : class
        {
            var xElementList = metaDataStream.AsEnumerableXml(typeof(T).Name);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            using (var context = new MetaDataContext())
            {
                context.ChangeTracker.AutoDetectChangesEnabled = false;
                var objectList = new List<T>(bufferSize);
                foreach (var xElement in xElementList)
                {
                    T deserializedObject;
                    using (var reader = xElement.CreateReader())
                    {
                        deserializedObject = (T) xmlSerializer.Deserialize(reader);
                    }

                    switch (deserializedObject)
                    {
                        case LBGDBMetadata.LaunchBox.Metadata.Game game:
                            game.NameSearch = game.Name;
                            game.PlatformSearch = game.Platform;
                            if (game.CommunityRating != null)
                            {
                                game.CommunityRating = Math.Round(((decimal) game.CommunityRating / 5) * 100, 0);
                            }

                            break;
                        case GameAlternateName game:
                            game.NameSearch = game.AlternateName;
                            break;
                    }

                    objectList.Add(deserializedObject);

                    if (objectList.Count >= bufferSize)
                    {
                        await context.BulkInsertAsync(objectList);
                        objectList.Clear();
                    }

                }

                if (objectList.Count > 0)
                {
                    await context.BulkInsertAsync(objectList);
                }
            }

            return true;
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            await RunBenchmark(1,10000);
            await RunBenchmark(1, 20000);
            //await RunBenchmark(1, 30000);
            //await RunBenchmark(1, 40000);
            await RunBenchmark(1, 50000);






        }

        private async Task<long> RunBenchmark(int runs, int bufferSize)
        {
            using (var context = new MetaDataContext())
            {
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }

            var stopwatch = new Stopwatch();
            using (var zipArchive = ZipFile.Open(@"C:\zipTest\metadata.zip", ZipArchiveMode.Read))
            {
                var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                    entry.Name.Equals("MetaData.xml", StringComparison.OrdinalIgnoreCase));

                if (metaData != null)
                {
                    for (int i = 0; i < runs; i++)
                    {
                        stopwatch.Start();
                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<LBGDBMetadata.LaunchBox.Metadata.Game>(metaDataStream, bufferSize);
                        }

                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<GameAlternateName>(metaDataStream, bufferSize);
                        }

                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<GameImage>(metaDataStream, bufferSize);
                        }
                        stopwatch.Stop();
                        using (var context = new MetaDataContext())
                        {
                            context.Database.EnsureDeleted();
                            context.Database.Migrate();
                        }
                    }
                    Console.WriteLine(stopwatch.ElapsedMilliseconds);
                }
            }

            return stopwatch.ElapsedMilliseconds;
        }
    }
}
