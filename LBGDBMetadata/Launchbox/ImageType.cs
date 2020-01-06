using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGDBMetadata.Launchbox
{
    public static class ImageType
    {
        public static HashSet<string> Cover = new HashSet<string>() { "Box - Front - Reconstructed", "Box - Front", "Fanart - Box - Front"};
        public static HashSet<string> Background = new HashSet<string>() {"Fanart - Background"};
    }
}
