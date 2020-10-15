using System;
using System.Windows;
using System.Windows.Controls;
using Playnite.SDK;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            btnRefresh.IsEnabled = false;
            var progressOptions =
                new GlobalProgressOptions("Downloading LaunchBox Metadata...", false) {IsIndeterminate = false};
            _plugin.PlayniteApi.Dialogs.ActivateGlobalProgress((progressAction) =>
            {
                try
                {
                    progressAction.ProgressMaxValue = 4;
                    var result = _plugin.UpdateMetadata(progressAction).Result;
                }
                catch (Exception)
                {
                    btnRefresh.IsEnabled = true;
                }
            }, progressOptions);
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //btnRefresh.IsEnabled = true;
            btnRefresh.IsEnabled = !_plugin.HasData() || await _plugin.NewMetadataAvailable();
        }
    }
}
