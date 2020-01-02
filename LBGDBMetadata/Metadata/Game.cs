using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "Game")]
    public class Game
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "ReleaseYear")]
        public string ReleaseYear { get; set; }
        [XmlElement(ElementName = "Overview")]
        public string Overview { get; set; }
        [XmlElement(ElementName = "MaxPlayers")]
        public string MaxPlayers { get; set; }
        [XmlElement(ElementName = "Cooperative")]
        public string Cooperative { get; set; }
        [Key]
        [XmlElement(ElementName = "DatabaseID")]
        public string DatabaseID { get; set; }
        [XmlElement(ElementName = "WikipediaURL")]
        public string WikipediaURL { get; set; }
        [XmlElement(ElementName = "Platform")]
        public string Platform { get; set; }
        [XmlElement(ElementName = "ESRB")]
        public string ESRB { get; set; }
        [XmlElement(ElementName = "CommunityRatingCount")]
        public string CommunityRatingCount { get; set; }
        [XmlElement(ElementName = "Genres")]
        public string Genres { get; set; }
        [XmlElement(ElementName = "Developer")]
        public string Developer { get; set; }
        [XmlElement(ElementName = "Publisher")]
        public string Publisher { get; set; }
        [XmlElement(ElementName = "ReleaseDate")]
        public string ReleaseDate { get; set; }
        [XmlElement(ElementName = "CommunityRating")]
        public string CommunityRating { get; set; }
    }
}