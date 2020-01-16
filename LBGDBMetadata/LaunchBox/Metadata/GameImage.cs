using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace LBGDBMetadata.LaunchBox.Metadata
{
    [XmlRoot(ElementName = "GameImage")]
    public class GameImage
    {
        [Key]
        public int ID { get; set; }

        [XmlElement(ElementName = "DatabaseID")]
        [ForeignKey("Game")]
        public long DatabaseID { get; set; }
        public Game Game { get; set; }
        
        [XmlElement(ElementName = "FileName")]
        public string FileName { get; set; }
        
        [XmlElement(ElementName = "Type")]
        public string Type { get; set; }
        
        [XmlElement(ElementName = "Region")]
        public string Region { get; set; }

    }
}