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
using LBGDBMetadata.LaunchBox.Metadata;


namespace LBGDBMetadata
{
    /// <summary>
    /// Interaction logic for LbgdbMetadataSettingsView.xaml
    /// </summary>
    public partial class LbgdbMetadataSettingsView : UserControl
    {
        private readonly LbgdbMetadataPlugin _plugin;

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
                var newMetadataHash = await _plugin.UpdateMetadata();
            }
            catch (Exception)
            {
                btnRefresh.IsEnabled = true;
            }
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = await _plugin.NewMetadataAvailable();
        }
    }
}
