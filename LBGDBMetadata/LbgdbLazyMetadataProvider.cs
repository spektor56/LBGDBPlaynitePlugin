using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    if (!string.IsNullOrWhiteSpace(selectedGame.Name))
                    {
                        return selectedGame.Name;
                    }

                }
            }

            return base.GetName();
        }

        public override List<string> GetGenres()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    if (!string.IsNullOrWhiteSpace(selectedGame.Genres))
                    {
                        return new List<string>() { selectedGame.Genres };
                    }

                }
            }

            return base.GetGenres();
        }

        public override DateTime? GetReleaseDate()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    if (selectedGame.ReleaseDate != null)
                    {
                        return selectedGame.ReleaseDate;
                    }

                }
            }

            return base.GetReleaseDate();
        }

        public override List<string> GetDevelopers()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    if (!string.IsNullOrWhiteSpace(selectedGame.Developer))
                    {
                        return new List<string>() { selectedGame.Developer };
                    }

                }
            }

            return base.GetDevelopers();
        }

        public override List<string> GetPublishers()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {

                    if (!string.IsNullOrWhiteSpace(selectedGame.Publisher))
                    {
                        return new List<string>() { selectedGame.Publisher };
                    }

                }
            }

            return base.GetPublishers();
        }


        public override string GetDescription()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    
                    if (!string.IsNullOrWhiteSpace(selectedGame.Overview))
                    {
                        return selectedGame.Overview;
                    }
                    
                }
            }

            return base.GetDescription();
        }

        public override int? GetCommunityScore()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    if (selectedGame.CommunityRating != null)
                    {
                        return (int)selectedGame.CommunityRating;
                    }
                }
            }

            return base.GetCommunityScore();
        }

        public override MetadataFile GetCoverImage()
        {
            using(var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name,"[^A-Za-z0-9]","") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));
                
                if (selectedGame != null)
                {
                    var coverImages = context.GameImages.FirstOrDefault(image => image.DatabaseID == selectedGame.DatabaseID && LaunchBox.Image.ImageType.Cover.Contains(image.Type));
                    if (coverImages != null)
                    {
                        return new MetadataFile("https://images.launchbox-app.com/" + coverImages.FileName);
                    }
                    
                }
                
            }

            return base.GetCoverImage();
        }



        public override MetadataFile GetBackgroundImage()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {
                    var backgroundImages = context.GameImages.FirstOrDefault(image => image.DatabaseID == selectedGame.DatabaseID && LaunchBox.Image.ImageType.Background.Contains(image.Type));
                    if (backgroundImages != null)
                    {
                        return new MetadataFile("https://images.launchbox-app.com/" + backgroundImages.FileName);
                    }

                }

            }

            return base.GetBackgroundImage();
        }

        public override List<Link> GetLinks()
        {
            using (var context = new MetaDataContext())
            {
                var selectedGame = context.Games.Include(x => x.AlternateNames).FirstOrDefault(game => game.Platform == options.GameData.Platform.Name && (game.Name == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", "") || game.AlternateNames.Any(alternateName => alternateName.AlternateName == Regex.Replace(options.GameData.Name, "[^A-Za-z0-9]", ""))));

                if (selectedGame != null)
                {

                    if (!string.IsNullOrWhiteSpace(selectedGame.WikipediaURL))
                    {
                        return new List<Link>() { new Link("Wikipedia", selectedGame.WikipediaURL) };
                    }

                }
            }

            return base.GetLinks();
        }

        public override List<MetadataField> AvailableFields
        {
            get
            {
                return plugin.SupportedFields;
            }
        }

        public override MetadataFile GetIcon()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                LBGDBMetadata.Properties.Resources.launchbox.Save(ms);
                return new MetadataFile("LaunchBox", ms.ToArray());
            }
        }

        public override int? GetCriticScore()
        {

            return base.GetCriticScore();
        }

        public override List<string> GetTags()
        {
            return base.GetTags();
        }




    }
}
