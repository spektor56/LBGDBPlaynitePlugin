using System;
using System.Windows;
using System.Windows.Controls;
using Playnite.ViewModels;
using Playnite.Windows;


namespace LBGDBMetadata
{
    /// <summary>
    /// Interaction logic for LbgdbMetadataSettingsView.xaml
    /// </summary>
    public partial class LbgdbMetadataSettingsView : UserControl
    {
        private readonly LbgdbMetadataPlugin _plugin;
        private ProgressViewViewModel _downloadProgress;

        public LbgdbMetadataSettingsView()
        {
            InitializeComponent();
        }

        public LbgdbMetadataSettingsView(LbgdbMetadataPlugin plugin)
        {
            _plugin = plugin;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            

            _downloadProgress = new ProgressViewViewModel(new ProgressWindowFactory(),
                () =>
                {
                    try
                    {
                        var result = _plugin.UpdateMetadata(_downloadProgress).Result;
                    }
                    catch (Exception)
                    {
                        btnRefresh.IsEnabled = true;
                    }
                },"Downloading LaunchBox Metadata..." );
            _downloadProgress.ActivateProgress();

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //btnRefresh.IsEnabled = true;
            btnRefresh.IsEnabled = !_plugin.HasData() || await _plugin.NewMetadataAvailable();
        }
    }
}
