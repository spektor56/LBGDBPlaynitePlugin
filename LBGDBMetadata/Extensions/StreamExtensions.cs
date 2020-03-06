using System.Collections.Generic;
using System.IO;
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
                bool found = false;
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            if (reader.Name == element)
                            {
                                found = true;
                                if (XNode.ReadFrom(reader) is XElement el)
                                {
                                    yield return el;
                                }
                            }
                            else
                            {
                                if (found)
                                {
                                    goto finished;
                                }
                            }
                            break;
                    }
                }
                finished:;
            }
        }
    }
}
