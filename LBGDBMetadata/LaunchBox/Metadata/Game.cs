using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace LBGDBMetadata.LaunchBox.Metadata
{
    [XmlRoot(ElementName = "Game")]
    public class Game
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [XmlElement(ElementName = "DatabaseID")]
        public long DatabaseID { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        
        [XmlElement(ElementName = "WikipediaURL")]
        public string WikipediaURL { get; set; }
        
        [XmlElement(ElementName = "Platform")]
        public string Platform { get; set; }
        
        [XmlElement(ElementName = "ESRB")]
        public string ESRB { get; set; }
        
        [XmlElement(ElementName = "CommunityRatingCount")]
        public long CommunityRatingCount { get; set; }
        
        [XmlElement(ElementName = "Genres")]
        public string Genres { get; set; }
        
        [XmlElement(ElementName = "Developer")]
        public string Developer { get; set; }
        
        [XmlElement(ElementName = "Publisher")]
        public string Publisher { get; set; }
        
        [XmlElement(ElementName = "ReleaseDate")]
        public DateTime? ReleaseDate { get; set; }
        
        [XmlElement(ElementName = "CommunityRating")]
        public decimal? CommunityRating { get; set; }

        [XmlElement(ElementName = "Overview")]
        public string Overview { get; set; }

        [XmlElement(ElementName = "VideoURL")]
        public string VideoURL { get; set; }

        public string NameSearch { get; set; }
        public string PlatformSearch { get; set; }

        [InverseProperty("Game")]
        public List<GameImage> Images { get; } = new List<GameImage>();

        [InverseProperty("Game")]
        public List<GameAlternateName> AlternateNames { get; } = new List<GameAlternateName>();
    }
}