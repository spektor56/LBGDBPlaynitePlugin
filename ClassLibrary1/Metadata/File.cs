using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "File")]
    public class File
    {
        [XmlElement(ElementName = "Platform")]
        public string Platform { get; set; }
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }
        [XmlElement(ElementName = "GameName")]
        public string GameName { get; set; }
    }
}