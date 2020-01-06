using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "LaunchBox")]
    public class Metadata
    {
        [XmlElement(ElementName = "Game")]
        public List<Game> Game { get; set; }
        [XmlElement(ElementName = "Platform")]
        public List<Platform> Platform { get; set; }
        [XmlElement(ElementName = "PlatformAlternateName")]
        public List<PlatformAlternateName> PlatformAlternateName { get; set; }
        [XmlElement(ElementName = "Emulator")]
        public List<Emulator> Emulator { get; set; }
        [XmlElement(ElementName = "EmulatorPlatform")]
        public List<EmulatorPlatform> EmulatorPlatform { get; set; }
        [XmlElement(ElementName = "GameAlternateName")]
        public List<GameAlternateName> GameAlternateName { get; set; }
        [XmlElement(ElementName = "GameImage")]
        public List<GameImage> GameImage { get; set; }
    }
}