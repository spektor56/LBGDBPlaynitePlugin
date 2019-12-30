using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "PlatformAlternateName")]
    public class PlatformAlternateName
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Alternate")]
        public string Alternate { get; set; }
    }

}