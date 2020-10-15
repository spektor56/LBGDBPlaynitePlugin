using System;
using System.Collections.Generic;
using Playnite.SDK;

namespace LBGDBMetadata
{
    public class LbgdbMetadataSettings : ObservableObject, ISettings
    {
        private LbgdbMetadataSettings _editingClone;
        private readonly LbgdbMetadataPlugin _plugin;

        private string _oldMetadataHash = "";
        public string OldMetadataHash
        {
            get => _oldMetadataHash;
            set
            {
                _oldMetadataHash = value;
                OnPropertyChanged();
            }
        }
        
        private string _metaDataUrl = @"https://gamesdb.launchbox-app.com/Metadata.zip";
        public string MetaDataURL
        {
            get => _metaDataUrl;
            set
            {
                _metaDataUrl = value;
                OnPropertyChanged();
            }
        }

        private string _metaDataFileName = @"Metadata.xml";
        public string MetaDataFileName
        {
            get => _metaDataFileName;
            set
            {
                _metaDataFileName = value;
                OnPropertyChanged();
            }
        }

        public LbgdbMetadataSettings()
        {
        }

        public LbgdbMetadataSettings(LbgdbMetadataPlugin plugin)
        {
            this._plugin = plugin;
            var savedSettings = plugin.LoadPluginSettings<LbgdbMetadataSettings>();
            if (savedSettings != null)
            {
                RestoreSettings(savedSettings);
            }
        }

        public void BeginEdit()
        {
            _editingClone = new LbgdbMetadataSettings(_plugin);
        }


        public void CancelEdit()
        {
            RestoreSettings(_editingClone);
        }


        public void EndEdit()
        {
            _plugin.SavePluginSettings(this);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = null;
            return true;
        }

        private void RestoreSettings(LbgdbMetadataSettings source)
        {
            MetaDataFileName = source.MetaDataFileName;
            MetaDataURL = source.MetaDataURL;
            OldMetadataHash = source.OldMetadataHash;
        }
    }
}