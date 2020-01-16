using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Playnite.SDK.Metadata;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;

namespace LBGDBMetadata
{
    public class LbgdbLazyMetadataProvider : OnDemandMetadataProvider
    {
        private readonly MetadataRequestOptions options;
        private readonly LbgdbMetadataPlugin plugin;
        private readonly ulong gameId = 0;
        public LbgdbLazyMetadataProvider(MetadataRequestOptions options, LbgdbMetadataPlugin plugin)
        {
            //Game object is in the options class
            //This class will search for the game once (name + platform), then use gameid on subsequent lookups to load each metadata field.
            this.options = options;
            this.plugin = plugin;
        }

        public LbgdbLazyMetadataProvider(ulong gameId, LbgdbMetadataPlugin plugin)
        {
            this.gameId = gameId;
            this.plugin = plugin;
        }

        private List<MetadataField> availableFields;
        public override string GetName()
        {
            return base.GetName();
        }

        public override List<string> GetGenres()
        {
            return base.GetGenres();
        }

        public override DateTime? GetReleaseDate()
        {
            return base.GetReleaseDate();
        }

        public override List<string> GetDevelopers()
        {
            return base.GetDevelopers();
        }

        public override List<string> GetPublishers()
        {
            return base.GetPublishers();
        }

        public override List<string> GetTags()
        {
            return base.GetTags();
        }

        public override string GetDescription()
        {
            return base.GetDescription();
        }

        public override int? GetCriticScore()
        {
            return base.GetCriticScore();
        }

        public override int? GetCommunityScore()
        {
            return base.GetCommunityScore();
        }

        public override MetadataFile GetCoverImage()
        {
            using(var context = new MetaDataContext())
            {
                var selectedGame = context.Games.FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name,"[^A-Za-z0-9]","") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));
                if (selectedGame != null)
                {
                    var coverImages = context.GameImages.FirstOrDefault(image => image.DatabaseID == selectedGame.DatabaseID && LaunchBox.Image.ImageType.Cover.Contains(image.Type));
                    if (coverImages != null)
                    {
                        return new MetadataFile("https://images.launchbox-app.com/" + coverImages.FileName);
                    }
                    
                }
                
            }

            return new MetadataFile();
        }

        public override MetadataFile GetIcon()
        {
            return base.GetIcon();
        }

        public override MetadataFile GetBackgroundImage()
        {
            return base.GetBackgroundImage();
        }

        public override List<Link> GetLinks()
        {
            return base.GetLinks();
        }

        public override List<MetadataField> AvailableFields
        {
            get
            {
                return plugin.SupportedFields;
            }
        }



    }
}
