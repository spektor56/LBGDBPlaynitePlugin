using System.Xml.Serialization;

namespace LBGDBMetadata.LaunchBox.Metadata
{
    [XmlRoot(ElementName = "GameAlternateName")]
    public class GameAlternateName
    {
        [XmlElement(ElementName = "AlternateName")]
        public string AlternateName { get; set; }
        
        [XmlElement(ElementName = "DatabaseID")]
        public string DatabaseID { get; set; }
        
        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }
    }
}