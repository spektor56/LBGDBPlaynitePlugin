using System.Collections.Generic;

namespace LBGDBMetadata.LaunchBox.Image
{
    public static class ImageType
    {
        public static HashSet<string> Cover = new HashSet<string>() { "Box - Front", "Box - Front - Reconstructed", "Fanart - Box - Front"};
        public static HashSet<string> Background = new HashSet<string>() {"Fanart - Background"};
        public static HashSet<string> Icon = new HashSet<string>() { "Clear Logo" };
    }
}
