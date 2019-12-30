using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "Emulator")]
    public class Emulator
    {
        [Key]
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "CommandLine")]
        public string CommandLine { get; set; }
        [XmlElement(ElementName = "ApplicableFileExtensions")]
        public string ApplicableFileExtensions { get; set; }
        [XmlElement(ElementName = "URL")]
        public string URL { get; set; }
        [XmlElement(ElementName = "BinaryFileName")]
        public string BinaryFileName { get; set; }
        [XmlElement(ElementName = "NoQuotes")]
        public string NoQuotes { get; set; }
        [XmlElement(ElementName = "NoSpace")]
        public string NoSpace { get; set; }
        [XmlElement(ElementName = "HideConsole")]
        public string HideConsole { get; set; }
        [XmlElement(ElementName = "FileNameOnly")]
        public string FileNameOnly { get; set; }
        [XmlElement(ElementName = "AutoExtract")]
        public string AutoExtract { get; set; }
    }
}