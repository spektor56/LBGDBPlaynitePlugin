using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using EFCore.BulkExtensions;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox.Api;
using LBGDBMetadata.LaunchBox.Metadata;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Playnite.SDK;
using Playnite.SDK.Plugins;
using Game = Playnite.SDK.Models.Game;

namespace LBGDBMetadata
{
    public class LbgdbMetadataPlugin : MetadataPlugin
    {
        private readonly LbgdbApi _lbgdbApi;
        internal readonly LbgdbMetadataSettings Settings;

        public LbgdbMetadataPlugin(IPlayniteAPI playniteAPI) : base(playniteAPI)
        {
            using (var metadataContext = new MetaDataContext())
            {
                metadataContext.Database.Migrate();
            }
            Settings = new LbgdbMetadataSettings(this);
            var apiOptions = new Options
            {
                MetaDataFileName = Settings.MetaDataFileName,
                MetaDataURL = Settings.MetaDataURL
            };
            _lbgdbApi = new LbgdbApi(apiOptions);
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return Settings;
        }

        public override UserControl GetSettingsView(bool firstRunView)
        {
            return new LbgdbMetadataSettingsView(this);
        }


        public override IEnumerable<ExtensionFunction> GetFunctions()
        {
            return base.GetFunctions();
        }

        public override void OnGameStarting(Game game)
        {
            base.OnGameStarting(game);
        }

        public override void OnGameStarted(Game game)
        {
            base.OnGameStarted(game);
        }

        public override void OnGameStopped(Game game, long ellapsedSeconds)
        {
            base.OnGameStopped(game, ellapsedSeconds);
        }

        public override void OnGameInstalled(Game game)
        {
            base.OnGameInstalled(game);
        }

        public override void OnGameUninstalled(Game game)
        {
            base.OnGameUninstalled(game);
        }

        public override void OnApplicationStarted()
        {
            base.OnApplicationStarted();
        }

        public async Task<bool> NewMetadataAvailable()
        {
            var newMetadataHash= await _lbgdbApi.GetMetadataHash();
            return !Settings.OldMetadataHash.Equals(newMetadataHash, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<string> UpdateMetadata()
        {
            string newMetadataHash = "";
            /*
            var newMetadataHash = await _lbgdbApi.GetMetadataHash();
            var zipFile = await _lbgdbApi.DownloadMetadata();*/
            await Task.Run(async () =>
            {
                using (var zipArchive = ZipFile.Open(@"C:\zipTest\Metadata.zip", ZipArchiveMode.Read))
                {
                    var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                        entry.Name.Equals(Settings.MetaDataFileName, StringComparison.OrdinalIgnoreCase));

                    if (metaData != null)
                    {
                        using (var metaDataStream = metaData.Open())
                        {
                            var gameXmlList = metaDataStream.AsEnumerableXml("Game");
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(LaunchBox.Metadata.Game));
                            int i = 0;
                            var context = new MetaDataContext();
                            await context.Database.ExecuteSqlRawAsync("DELETE FROM GAMES");
                            context.ChangeTracker.AutoDetectChangesEnabled = false;
                            foreach (var gameXml in gameXmlList)
                            {
                                using (var reader = gameXml.CreateReader())
                                {
                                    var game = (LaunchBox.Metadata.Game) xmlSerializer.Deserialize(reader);

                                    if (i++ > 1000)
                                    {
                                        await context.SaveChangesAsync();
                                        i = 0;
                                        context.Dispose();
                                        context = new MetaDataContext();
                                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                                    }

                                    context.Games.Add(game);
                                }
                            }

                            await context.SaveChangesAsync();
                        }

                        using (var metaDataStream = metaData.Open())
                        {
                            var imageXmlList = metaDataStream.AsEnumerableXml("GameImage");
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(GameImage));
                            int i = 0;
                            var context = new MetaDataContext();
                            await context.Database.ExecuteSqlRawAsync("DELETE FROM GAMEIMAGEs");
                            context.ChangeTracker.AutoDetectChangesEnabled = false;
                            foreach (var imageXml in imageXmlList)
                            {
                                using (var reader = imageXml.CreateReader())
                                {
                                    var image = (GameImage)xmlSerializer.Deserialize(reader);

                                    if (i++ > 1000)
                                    {
                                        await context.SaveChangesAsync();
                                        i = 0;
                                        context.Dispose();
                                        context = new MetaDataContext();
                                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                                    }

                                    context.GameImages.Add(image);
                                }
                            }

                            await context.SaveChangesAsync();
                        }
                    }
                }
            });
            Settings.OldMetadataHash = newMetadataHash;
            Settings.EndEdit();

            return newMetadataHash;
        }

        public void UpdateMetadata(string filename)
        {
            using (var zipArchive = ZipFile.Open(filename, ZipArchiveMode.Read))
            {
                var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                    entry.Name.Equals("MetaData.xml", StringComparison.OrdinalIgnoreCase));

                if (metaData != null)
                    using (var metaDataStream = metaData.Open())
                    {
                        var games = metaDataStream.AsEnumerableXml("Game");
                        var xmlSerializer = new XmlSerializer(typeof(LaunchBox.Metadata.Game));

                        var i = 0;
                        var context = new MetaDataContext();
                        context.ChangeTracker.AutoDetectChangesEnabled = false;
                        
                        foreach (var xElement in games)
                        {
                            var gameMetaData = (LaunchBox.Metadata.Game)xmlSerializer.Deserialize(xElement.CreateReader());
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

        public override Guid Id { get; } = Guid.Parse("000001D9-DBD1-46C6-B5D0-B1BA559D10E4");
        public override OnDemandMetadataProvider GetMetadataProvider(MetadataRequestOptions options)
        {
            return new LbgdbLazyMetadataProvider(options, this);
        }

        public override string Name { get; } = "Launchbox";
        public override List<MetadataField> SupportedFields { get; } = new List<MetadataField>
        {
            MetadataField.Name,
            MetadataField.Genres,
            MetadataField.ReleaseDate,
            MetadataField.Developers,
            MetadataField.Publishers,
            //MetadataField.Tags,
            MetadataField.Description,
            MetadataField.Links,
            MetadataField.CriticScore,
            MetadataField.CommunityScore,
            //MetadataField.Icon,
            MetadataField.CoverImage,
            MetadataField.BackgroundImage
            
        };
    }
}
