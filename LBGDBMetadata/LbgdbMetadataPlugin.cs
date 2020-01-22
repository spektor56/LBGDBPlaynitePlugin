using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;
using EFCore.BulkExtensions;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox.Api;
using LBGDBMetadata.LaunchBox.Metadata;
using Microsoft.EntityFrameworkCore;
using Playnite.SDK;
using Playnite.SDK.Plugins;
using Playnite.ViewModels;
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
            var newMetadataHash = await _lbgdbApi.GetMetadataHash();
            return !Settings.OldMetadataHash.Equals(newMetadataHash, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<bool> ImportXml<T>(Stream metaDataStream, int bufferSize = 10000) where T : class
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
                        deserializedObject = (T)xmlSerializer.Deserialize(reader);
                    }

                    switch (deserializedObject)
                    {
                        case LaunchBox.Metadata.Game game:
                            game.NameSearch = game.Name.Sanitize();
                            game.PlatformSearch = game.Platform.Sanitize();
                            if (game.CommunityRating != null)
                            {
                                game.CommunityRating = Math.Round(((decimal)game.CommunityRating / 5) * 100, 0);
                            }
                            break;
                        case GameAlternateName game:
                            game.NameSearch = game.AlternateName.Sanitize();
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

        public bool HasData()
        {
            using (var metaDataContext = new MetaDataContext())
            {
                if (metaDataContext.Games.Any())
                {
                    return true;
                }
            }

            return false;
        }


        public async Task<string> UpdateMetadata(ProgressViewViewModel progress)
        {
            var newMetadataHash = await _lbgdbApi.GetMetadataHash();
            var zipFile = await _lbgdbApi.DownloadMetadata();

            await Task.Run(async () =>
            {
                using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Read))
                {
                    var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                        entry.Name.Equals(Settings.MetaDataFileName, StringComparison.OrdinalIgnoreCase));

                    if (metaData != null)
                    {
                        progress.ProgressText = "Updating database...";
                        using (var context = new MetaDataContext())
                        {
                            await context.Database.EnsureDeletedAsync();
                            await context.Database.MigrateAsync();
                        }

                        progress.ProgressText = "Importing games...";
                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<LaunchBox.Metadata.Game>(metaDataStream);
                        }

                        progress.ProgressText = "Importing alternate game names...";
                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<GameAlternateName>(metaDataStream);
                        }

                        progress.ProgressText = "Importing media...";
                        using (var metaDataStream = metaData.Open())
                        {
                            await ImportXml<GameImage>(metaDataStream);
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
                {
                    using (var metaDataStream = metaData.Open())
                    {
                        var games = metaDataStream.AsEnumerableXml("Game");
                        var xmlSerializer = new XmlSerializer(typeof(LaunchBox.Metadata.Game));

                        var i = 0;
                        var context = new MetaDataContext();
                        context.ChangeTracker.AutoDetectChangesEnabled = false;

                        foreach (var xElement in games)
                        {
                            var gameMetaData =
                                (LaunchBox.Metadata.Game) xmlSerializer.Deserialize(xElement.CreateReader());
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
            MetadataField.Icon,
            MetadataField.CoverImage,
            MetadataField.BackgroundImage

        };
    }
}
