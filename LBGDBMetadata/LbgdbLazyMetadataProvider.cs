using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox.Metadata;
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
        private LaunchBox.Metadata.Game _game;
        private List<MetadataField> availableFields;

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

        private GameImage GetBestImage(List<GameImage> images, HashSet<string> imageTypes)
        {
            if (images.Count < 1)
            {
                return null;
            }
            IOrderedEnumerable<GameImage> filteredImages;
            foreach (var coverType in imageTypes)
            {
                if (images.All(image => image.Type != coverType))
                {
                    continue;
                }

                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.Canada, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
             
                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.NorthAmerica, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
                    
                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.UnitedStates, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
                        
                filteredImages = images.Where(image => image.Type == coverType && string.IsNullOrWhiteSpace(image.Region)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
            
                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.World, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
                        
                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.UnitedKingdom, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
                       
                filteredImages = images.Where(image => image.Type == coverType && image.Region != null && image.Region.Equals(LaunchBox.Region.Europe, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
                if (filteredImages.Any())
                {
                    return filteredImages.First();
                }
            }
            return images.FirstOrDefault();
        }

        private LaunchBox.Metadata.Game GetGame()
        {
            if (_game is null)
            {
                using (var context = new MetaDataContext())
                {
                    var gameSearchName = options.GameData.Name.Sanitize();
                    var platformSearchName = options.GameData.Platform.Name.Sanitize();
                    _game = context.Games.FirstOrDefault(game => game.PlatformSearch == platformSearchName && (game.NameSearch == gameSearchName || game.AlternateNames.Any(alternateName => alternateName.NameSearch == gameSearchName)));
                    return _game;
                }
            }
            else
            {
                return _game;
            }
        }

        public override string GetName()
        {
            var game = GetGame();
            
            if (game != null)
            {
                if (!string.IsNullOrWhiteSpace(game.Name))
                {
                    return game.Name;
                }
            }
            
            return base.GetName();
        }

        public override List<string> GetGenres()
        {
            var game = GetGame();

            if (game != null)
            {
                if (!string.IsNullOrWhiteSpace(game.Genres))
                {
                    return game.Genres.Split(';').Select(genre => genre.Trim()).ToList();
                }
            }

            return base.GetGenres();
        }

        public override DateTime? GetReleaseDate()
        {
            var game = GetGame();

            if (game != null)
            {
                if (game.ReleaseDate != null)
                {
                    return game.ReleaseDate;
                }
            }

            return base.GetReleaseDate();
        }

        public override List<string> GetDevelopers()
        {
            var game = GetGame();

            if (game != null)
            {
                if (!string.IsNullOrWhiteSpace(game.Developer))
                {
                    return game.Developer.Split(';').Select(developer => developer.Trim()).ToList();
                }
            }

            return base.GetDevelopers();
        }

        public override List<string> GetPublishers()
        {
            var game = GetGame();

            if (game != null)
            {
                if (!string.IsNullOrWhiteSpace(game.Publisher))
                {
                    return game.Publisher.Split(';').Select(publisher => publisher.Trim()).ToList();
                }
            }

            return base.GetPublishers();
        }


        public override string GetDescription()
        {
            var game = GetGame();

            if (game != null)
            {
                if (!string.IsNullOrWhiteSpace(game.Overview))
                {
                    return game.Overview;
                }
            }

            return base.GetDescription();
        }

        public override int? GetCommunityScore()
        {
            var game = GetGame();

            if (game != null)
            {
                if (game.CommunityRating != null)
                {
                    return (int)game.CommunityRating;
                }
            }

            return base.GetCommunityScore();
        }

        public override MetadataFile GetCoverImage()
        {
            var game = GetGame();
            
            if (game != null)
            {
                using (var context = new MetaDataContext())
                {
                    var coverImage = GetBestImage(context.GameImages.Where(image => image.DatabaseID == game.DatabaseID && LaunchBox.Image.ImageType.Cover.Contains(image.Type)).ToList(), LaunchBox.Image.ImageType.Cover);
                    if (coverImage != null)
                    {
                        return new MetadataFile("https://images.launchbox-app.com/" + coverImage.FileName);
                    }
                }
            }

            return base.GetCoverImage();
        }

        public override MetadataFile GetBackgroundImage()
        {
            var game = GetGame();

            if (game != null)
            {
                using (var context = new MetaDataContext())
                {
                    var backgroundImage = GetBestImage(context.GameImages.Where(image => image.DatabaseID == game.DatabaseID && LaunchBox.Image.ImageType.Background.Contains(image.Type)).ToList(), LaunchBox.Image.ImageType.Background);
                    if (backgroundImage != null)
                    {
                        return new MetadataFile("https://images.launchbox-app.com/" + backgroundImage.FileName);
                    }
                }
            }       

            return base.GetBackgroundImage();
        }

        public override List<Link> GetLinks()
        {
            var game = GetGame();

            if (game != null)
            {
                var links = new List<Link>
                {
                    new Link("LaunchBox", "https://gamesdb.launchbox-app.com/games/dbid/" + game.DatabaseID)
                };

                if (!string.IsNullOrWhiteSpace(game.WikipediaURL))
                {
                    links.Add(new Link("Wikipedia", game.WikipediaURL));
                }

                if (!string.IsNullOrWhiteSpace(game.VideoURL))
                {
                    links.Add(new Link("Video", game.VideoURL));
                }

                return links;
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
