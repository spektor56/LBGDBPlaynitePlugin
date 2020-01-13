using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace LBGDBMetadata.Extensions
{
    public static class StreamExtensions
    {
        public static IEnumerable<XElement> AsEnumerableXml(this Stream metaDataStream, string element)
        {
            using (var reader = XmlReader.Create(metaDataStream))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == element)
                            {
                                if (XNode.ReadFrom(reader) is XElement el)
                                {
                                    yield return el;
                                }
                            }

                            break;
                    }
                }
            }
        }
    }
}
