using System;
using System.Collections.Generic;
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

            return base.GetCoverImage();
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
                return availableFields;
            }
        }



    }
}
