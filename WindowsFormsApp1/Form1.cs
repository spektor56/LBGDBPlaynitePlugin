using LBGDBMetadata;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            using (var context = new MetaDataContext())
            {
                var gameSearchName = Regex.Replace("abadox", "[^A-Za-z0-9]", "").ToLower();
                var selectedGame = context.Games.FirstOrDefault(game => game.Platform == "Nintendo Entertainment System" && (game.Name == gameSearchName || game.AlternateNames.Any(alternateName => alternateName.AlternateName == gameSearchName)));

                if (selectedGame != null)
                {
                    var coverImages = context.GameImages.FirstOrDefault(image => image.DatabaseID == selectedGame.DatabaseID && LBGDBMetadata.LaunchBox.Image.ImageType.Cover.Contains(image.Type));
                    if (coverImages != null)
                    {

                    }

                }

            }
        }
    }
}
