using System;
using System.Collections.Generic;
using Playnite.SDK;

namespace LBGDBMetadata
{
    public class LbgdbMetadataSettings : ObservableObject, ISettings
    {
        private LbgdbMetadataSettings editingClone;
        private readonly LbgdbMetadataPlugin plugin;

        private string oldHash = "";
        public string OldHash
        {
            get => oldHash;
            set
            {
                oldHash = value;
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