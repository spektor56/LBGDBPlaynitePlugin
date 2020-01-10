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
        private LbgdbMetadataSettings _settings;
        private LbgdbMetadataPlugin _plugin;

        public LbgdbMetadataSettingsView()
        {
            InitializeComponent();
        }

        public LbgdbMetadataSettingsView(LbgdbMetadataPlugin plugin)
        {
            _plugin = plugin;
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
           
            try
            {
                var currentVersion = await _plugin.UpdateMetadata();
            }
            catch (Exception)
            {
                btnRefresh.IsEnabled = true;
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            MetaDataContext context = new MetaDataContext();
            context.Games.Add(new Game());
            context.SaveChanges();
            // btnRefresh.IsEnabled = await _plugin.NewMetadataAvailable();
        }
    }
}
