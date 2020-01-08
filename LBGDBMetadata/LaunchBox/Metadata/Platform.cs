using System.Xml.Serialization;

namespace LBGDBMetadata.LaunchBox.Metadata
{
    [XmlRoot(ElementName = "Platform")]
    public class Platform
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        
        [XmlElement(ElementName = "Emulated")]
        public string Emulated { get; set; }
        
        [XmlElement(ElementName = "ReleaseDate")]
        public string ReleaseDate { get; set; }
        
        [XmlElement(ElementName = "Developer")]
        public string Developer { get; set; }
        
        [XmlElement(ElementName = "Manufacturer")]
        public string Manufacturer { get; set; }
        
        [XmlElement(ElementName = "Cpu")]
        public string Cpu { get; set; }
        
        [XmlElement(ElementName = "Memory")]
        public string Memory { get; set; }
        
        [XmlElement(ElementName = "Graphics")]
        public string Graphics { get; set; }
        
        [XmlElement(ElementName = "Sound")]
        public string Sound { get; set; }
        
        [XmlElement(ElementName = "Display")]
        public string Display { get; set; }
        
        [XmlElement(ElementName = "Media")]
        public string Media { get; set; }

        [XmlElement(ElementName = "MaxControllers")]
        public string MaxControllers { get; set; }
        
        [XmlElement(ElementName = "Notes")]
        public string Notes { get; set; }
        
        [XmlElement(ElementName = "Category")]
        public string Category { get; set; }
        
        [XmlElement(ElementName = "UseMameFiles")]
        public string UseMameFiles { get; set; }
    }
}