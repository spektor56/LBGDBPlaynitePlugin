using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Serialization;
using LBGDBMetadata.LaunchBox.Api;
using LBGDBMetadata.LaunchBox.Metadata;


namespace LBGDBMetadata
{
    /// <summary>
    /// Interaction logic for LbgdbMetadataSettingsView.xaml
    /// </summary>
    public partial class LbgdbMetadataSettingsView : UserControl
    {
        private static Options _options = new Options();
        private LbgdbApi _lbgdbApi = new LbgdbApi(_options);
        public LbgdbMetadataSettingsView()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;

            try
            {
                var zipFile = await _lbgdbApi.DownloadMetadata();
                Task.Run(() =>
                {
                    using (var zipArchive = new ZipArchive(zipFile, ZipArchiveMode.Read))
                    {
                        var metaData = zipArchive.Entries.FirstOrDefault(entry =>
                            entry.Name.Equals(_options.MetaDataFileName, StringComparison.OrdinalIgnoreCase));

                        Metadata gameMetaData;

                        if (metaData != null)
                        {

                            using (var metaDataStream = metaData.Open())
                            using (XmlReader reader = XmlReader.Create(metaDataStream))
                            {
                                XmlSerializer xmlSerializer = new XmlSerializer(typeof(Metadata));
                                gameMetaData = (Metadata) xmlSerializer.Deserialize(reader);
                                var games = gameMetaData.Game.Where(game => game.Name.Length < 2);
                            }
                        }
                    }
                });

            }
            catch (Exception)
            {
                btnRefresh.IsEnabled = true;
            }
            
            
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var oldHash = new Options().OldHash;
            var newHash = await _lbgdbApi.GetMetadataHash();

            txtOldHash.Text = newHash;
            btnRefresh.IsEnabled = !oldHash.Equals(newHash, StringComparison.OrdinalIgnoreCase);

        }
    }
}
