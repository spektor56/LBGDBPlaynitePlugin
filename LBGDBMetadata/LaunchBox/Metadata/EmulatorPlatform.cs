using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace LBGDBMetadata.LaunchBox.Metadata
{
    [XmlRoot(ElementName = "EmulatorPlatform")]
    public class EmulatorPlatform
    {
        [Key]
        [XmlElement(ElementName = "Emulator")]
        public string Emulator { get; set; }
        
        [XmlElement(ElementName = "Platform")]
        public string Platform { get; set; }
        
        [XmlElement(ElementName = "CommandLine")]
        public string CommandLine { get; set; }
        
        [XmlElement(ElementName = "ApplicableFileExtensions")]
        public string ApplicableFileExtensions { get; set; }
        
        [XmlElement(ElementName = "Recommended")]
        public string Recommended { get; set; }
    }
}