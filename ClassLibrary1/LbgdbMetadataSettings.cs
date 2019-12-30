using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Playnite.SDK;

namespace LBGDBMetadata
{
    public class LbgdbMetadataSettings : ObservableObject, ISettings
    {
        private LbgdbMetadataSettings editingClone;
        private readonly LbgdbMetadataPlugin plugin;

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