using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using LBGDBMetadata.LaunchBox.Api;
using LBGDBMetadata.LaunchBox.Metadata;
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
            var newMetadataHash = await _lbgdbApi.GetMetadataHash();
            var zipFile = await _lbgdbApi.DownloadMetadata();
            await Task.Run(async () =>
            {
                using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Read))
                {
                    var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                        entry.Name.Equals(Settings.MetaDataFileName, StringComparison.OrdinalIgnoreCase));

                    Metadata gameMetaData;

                    if (metaData != null)
                    {

                        using (var metaDataStream = metaData.Open())
                        using (XmlReader reader = XmlReader.Create(metaDataStream))
                        {
                            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Metadata));
                            gameMetaData = (Metadata)xmlSerializer.Deserialize(reader);
                            MetaDataContext context = new MetaDataContext();
                            context.Games.AddRange(gameMetaData.Games);
                            context.SaveChanges();
                        }
                    }
                }
            });
            Settings.OldMetadataHash = newMetadataHash;
            Settings.EndEdit();
            return newMetadataHash;
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
