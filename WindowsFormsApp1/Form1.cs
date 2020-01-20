using LBGDBMetadata;
using LBGDBMetadata.LaunchBox.Metadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private GameImage GetBestImage(List<GameImage> images)
        {
            if (images.Count < 1)
            {
                return null;
            }

            var filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.Canada, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return filteredImages.First();
            }
            filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.NorthAmerica, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return images.First();
            }
            filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.UnitedStates, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return images.First();
            }
            filteredImages = images.Where(image => string.IsNullOrWhiteSpace(image.Region)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return images.First();
            }
            filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.World, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any()) 
            {
                return images.First();
            }
            filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.UnitedKingdom, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return images.First();
            }
            filteredImages = images.Where(image => image.Region != null && image.Region.Equals(LBGDBMetadata.LaunchBox.Region.Europe, StringComparison.OrdinalIgnoreCase)).OrderByDescending(image => image.ID);
            if (filteredImages.Any())
            {
                return images.First();
            }
            return images.FirstOrDefault();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            List<GameImage> gameImages = new List<GameImage>();
            gameImages.Add(new GameImage() { Region = "" });
            gameImages.Add(new GameImage() { Region = LBGDBMetadata.LaunchBox.Region.UnitedStates });
            gameImages.Add(new GameImage() { Region = LBGDBMetadata.LaunchBox.Region.Canada });
            gameImages.Add(new GameImage() { Region = LBGDBMetadata.LaunchBox.Region.Canada });
            gameImages.Add(new GameImage() { Region = LBGDBMetadata.LaunchBox.Region.Canada });
            gameImages.Add(new GameImage() { Region = null });

            var ismage = GetBestImage(gameImages);

            using (var context = new MetaDataContext())
            {
                var gameSearchName = Regex.Replace("Mega Man X3", "[^A-Za-z0-9]", "").ToLower();
                var selectedGame = context.Games.FirstOrDefault(game => game.Platform == "Super Nintendo Entertainment System" && (game.NameSearch == gameSearchName || game.AlternateNames.Any(alternateName => alternateName.NameSearch == gameSearchName)));

                if (selectedGame != null)
                {
                    var coverImages = context.GameImages.Where(image => image.DatabaseID == selectedGame.DatabaseID && LBGDBMetadata.LaunchBox.Image.ImageType.Cover.Contains(image.Type)).ToList();
                    if (coverImages != null)
                    {
                        
                        foreach (var coverImage in coverImages)
                        {
                            Console.WriteLine(string.Format("{0} {1} {2}", coverImage.DatabaseID, coverImage.FileName, coverImage.Region));
                        }
                    }

                }

            }
        }
    }
}
