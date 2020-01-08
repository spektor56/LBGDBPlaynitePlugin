using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

namespace LBGDBMetadata
{
    public class LbgdbMetadataPlugin : MetadataPlugin
    {
        internal readonly LbgdbMetadataSettings Settings;

        public LbgdbMetadataPlugin(IPlayniteAPI playniteAPI) : base(playniteAPI)
        {
            Settings = new LbgdbMetadataSettings(this);
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return Settings;
        }

        public override UserControl GetSettingsView(bool firstRunView)
        {
            return new LbgdbMetadataSettingsView();
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
