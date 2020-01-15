using System;
using System.Collections.Generic;
using Playnite.SDK;

namespace LBGDBMetadata
{
    public class LbgdbMetadataSettings : ObservableObject, ISettings
    {
        private LbgdbMetadataSettings editingClone;
        private readonly LbgdbMetadataPlugin plugin;

        private string oldMetadataHash = "";
        public string OldMetadataHash
        {
            get => oldMetadataHash;
            set
            {
                oldMetadataHash = value;
                OnPropertyChanged();
            }
        }
        
        private string metaDataURL = @"https://gamesdb.launchbox-app.com/Metadata.zip";
        public string MetaDataURL
        {
            get => metaDataURL;
            set
            {
                metaDataURL = value;
                OnPropertyChanged();
            }
        }

        private string metaDataFileName = @"Metadata.xml";
        public string MetaDataFileName
        {
            get => metaDataFileName;
            set
            {
                metaDataFileName = value;
                OnPropertyChanged();
            }
        }

        public LbgdbMetadataSettings()
        {
        }

        public LbgdbMetadataSettings(LbgdbMetadataPlugin plugin)
        {
            this.plugin = plugin;
            
            var settings = plugin.LoadPluginSettings<LbgdbMetadataSettings>();
            if (settings != null)
            {
                LoadValues(settings);
            }
            
        }

        public void BeginEdit()
        {
            editingClone = this.GetClone();
        }

        public void EndEdit()
        {
            plugin.SavePluginSettings(this);
        }

        public void CancelEdit()
        {
            LoadValues(editingClone);
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = null;
            return true;
        }

        private void LoadValues(LbgdbMetadataSettings source)
        {
            source.CopyProperties(this, false, null, true);
        }
    }
}