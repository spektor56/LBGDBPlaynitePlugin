﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox;
using LBGDBMetadata.LaunchBox.Metadata;
using Playnite.SDK.Metadata;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using Game = LBGDBMetadata.LaunchBox.Metadata.Game;

namespace LBGDBMetadata
{
    public class LbgdbMetadataProvider : OnDemandMetadataProvider
    {
        private readonly MetadataRequestOptions _options;
        private readonly LbgdbMetadataPlugin _plugin;
        private Game _game;
        private Dictionary<string, int> _regionPriority = new Dictionary<string, int>();
        
        public LbgdbMetadataProvider(MetadataRequestOptions options, LbgdbMetadataPlugin plugin)
        {
            _options = options;
            _plugin = plugin;
        }

        private int GetWeightedRating(double communityRatingCount, double communityRating )
        {
            double positiveVotes = Math.Floor((communityRating / 100) * communityRatingCount);
            double negativeVotes = communityRatingCount - positiveVotes;

            double totalVotes = positiveVotes + negativeVotes;
            double average = totalVotes < 1 ? 0 : positiveVotes / totalVotes;
            double score = average - (average - 0.5) * Math.Pow(2, -Math.Log10(totalVotes + 1));
            
            return (int)(score * 100);
        }

        private GameImage GetBestImage(List<GameImage> images, HashSet<string> imageTypes)
        {
            if (images.Count < 1)
            {
                return null;
            }

            foreach (var coverType in imageTypes)
            {
                if (images.All(image => image.Type != coverType))
                {
                    continue;
                }

                return images
                    .Where(image => image.Type == coverType && _regionPriority.ContainsKey(image.Region ?? ""))
                    .OrderBy((n) =>
                    {
                        if (_regionPriority.ContainsKey(n.Region ?? ""))
                        {
                            return _regionPriority[n.Region ?? ""];
                        }

                        return int.MaxValue;
                    }).FirstOrDefault();
            }
            return images.FirstOrDefault();
        }

        private Game GetGame()
        {
            if (_game is null)
            {
                var gameSearchName = "";
                if (!string.IsNullOrWhiteSpace(_options?.GameData?.Name))
                {
                    gameSearchName = _options.GameData.Name.Sanitize();
                }

                if (!string.IsNullOrWhiteSpace(gameSearchName))
                {
                    if (_options?.GameData != null && _regionPriority.Count < 1)
                    {
                        if (_options.GameData.Region != null && !string.IsNullOrWhiteSpace(_options.GameData.Region.Name))
                        {
                            _regionPriority = _options.GameData.Region.Name.GetRegionPriorityList();
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(_options.GameData.GameImagePath))
                            {
                                var noIntoRegion = _options.GameData.GameImagePath.GetRegionNoIntro();
                                if (!string.IsNullOrWhiteSpace(noIntoRegion))
                                {
                                    _regionPriority = noIntoRegion.GetRegionPriorityList();
                                }
                            }
                        }
                    }

                    var platformSearchName = "";
                    if (!string.IsNullOrWhiteSpace(_options?.GameData?.Platform?.Name))
                    {
                        var sanitizedPlatform = _options.GameData.Platform.Name.Sanitize();
                        platformSearchName = _plugin.PlatformTranslationTable.ContainsKey(sanitizedPlatform)
                            ? _plugin.PlatformTranslationTable[sanitizedPlatform]
                            : sanitizedPlatform;
                    }
                    
                    using (var context = new MetaDataContext(_plugin.GetPluginUserDataPath()))
                    {
                        /* Can't tell which region the actual game name is from in the game object...
                        if (_regionPriority.Count > 0)
                        {
                            var alternateNames = context.GameAlternateName.Where(game =>
                                game.Game.PlatformSearch == platformSearchName && (game.NameSearch == gameSearchName || game.Game.NameSearch == gameSearchName)).ToList();

                            var regionGameName = alternateNames.Where(game => _regionPriority.ContainsKey(game.Region ?? "")).OrderBy((n) =>
                            {
                                if (_regionPriority.ContainsKey(n.Region ?? ""))
                                {
                                    return _regionPriority[n.Region ?? ""];
                                }

                                return int.MaxValue;
                            }).FirstOrDefault();

                            if (regionGameName != null)
                            {
                                _game = context.Games.FirstOrDefault(
                                    game => game.DatabaseID == regionGameName.DatabaseID);

                                if (_game != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(regionGameName.AlternateName))
                                    {
                                        _game.Name = regionGameName.AlternateName;
                                    }
                                }
                            }
                        }
                        */
                        if (_game is null)
                        {
                            _game = context.Games.FirstOrDefault(game =>
                                game.PlatformSearch == platformSearchName && (game.NameSearch == gameSearchName ||
                                                                              game.AlternateNames.Any(alternateName =>
                                                                                  alternateName.NameSearch ==
                                                                                  gameSearchName)));

                            if (_game?.NameSearch != null && _game?.NameSearch != gameSearchName)
                            {
                                var alternateGameNames = context.GameAlternateName.Where(alternateName =>
                                    alternateName.DatabaseID == _game.DatabaseID && alternateName.NameSearch ==
                                    gameSearchName);

                                var numberOfNames = alternateGameNames.Count();

                                if (numberOfNames > 0)
                                {
                                    var gameName = alternateGameNames.First();
                                    if (!string.IsNullOrWhiteSpace(gameName.AlternateName))
                                    {
                                        _game.Name = gameName.AlternateName;
                                    }

                                    if (numberOfNames < 2 && !string.IsNullOrWhiteSpace(gameName.Region))
                                    {
                                        if (_regionPriority.Count < 1)
                                        {
                                            _regionPriority = gameName.Region.GetRegionPriorityList();
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (_game != null && _regionPriority.Count < 1)
                    {
                        _regionPriority = LaunchBox.Region.GetRegionPriorityList(null);
                    }
                }
            }
            
            return _game;
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
                    return game.Genres.Split(';').Select(genre => genre.Trim()).OrderBy(genre => genre.Trim()).ToList();
                }
            }

            return base.GetGenres();
        }

        public override DateTime? GetReleaseDate()
        {
            var game = GetGame();

            if (game?.ReleaseDate != null)
            {
                return game.ReleaseDate;
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
                    return game.Developer.Split(';').Select(developer => developer.Trim()).OrderBy(developer => developer.Trim()).ToList();
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
                    return game.Publisher.Split(';').Select(publisher => publisher.Trim()).OrderBy(publisher => publisher.Trim()).ToList();
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
                if (game.CommunityRating != null && game.CommunityRatingCount > 0)
                {
                    return GetWeightedRating(game.CommunityRatingCount, (double)game.CommunityRating);
                }
            }

            return base.GetCommunityScore();
        }

        public override MetadataFile GetCoverImage()
        {
            var game = GetGame();
            
            if (game != null)
            {
                using (var context = new MetaDataContext(_plugin.GetPluginUserDataPath()))
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
                using (var context = new MetaDataContext(_plugin.GetPluginUserDataPath()))
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

        public override List<string> GetFeatures()
        {
            return base.GetFeatures();
        }

        public override List<MetadataField> AvailableFields
        {
            get
            {
                return _plugin.SupportedFields;
            }
        }

        public override MetadataFile GetIcon()
        {
            var game = GetGame();

            if (game != null)
            {
                using (var context = new MetaDataContext(_plugin.GetPluginUserDataPath()))
                {
                    var icon =
                        GetBestImage(
                            context.GameImages.Where(image =>
                                image.DatabaseID == game.DatabaseID &&
                                LaunchBox.Image.ImageType.Icon.Contains(image.Type)).ToList(),
                            LaunchBox.Image.ImageType.Icon);
                    if (icon != null)
                    {
                        var imageData = _plugin.HttpClient.GetByteArrayAsync("https://images.launchbox-app.com/" + icon.FileName).Result;

                        using (Image image = Image.Load(imageData))
                        {
                            image.Mutate(x =>
                            {
                                x.Resize(new ResizeOptions
                                {
                                    Size = new Size(256, 256),
                                    Mode = ResizeMode.Pad
                                });
                            });

                            using (MemoryStream ms = new MemoryStream())
                            {
                                image.Save(ms, new PngEncoder());
                                return new MetadataFile(icon.FileName, ms.ToArray(), "https://images.launchbox-app.com/" + icon.FileName);
                            }
                        }
                    }
                }
            }

            return base.GetIcon();
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
