using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBGDBMetadata.Metadata
{
    [XmlRoot(ElementName = "LaunchBox")]
    public class Files
    {
        [XmlElement(ElementName = "File")]
        public List<File> File { get; set; }
    }
}