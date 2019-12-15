using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Xml;
using System.Xml.Serialization;

namespace LBGDBMetadata
{
    public class LBGDBApi
    {
        static string metaDataURL = @"https://gamesdb.launchbox-app.com/Metadata.zip";
        static string metaDataDirectory = @"C:\zipTest\";
        static string metaDataArchiveName = @"Metadata.zip";
        static string metaDataFileName = @"Metadata.xml";
        static string oldHash = "\"574e3a1d5db2d51:0\"";
        static readonly HttpClient _client = new HttpClient();
        public static async void DownloadMetadata()
        {
            var metaDataFullPath = Path.Combine(metaDataDirectory, metaDataArchiveName);

            try
            {
                var headerResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head,metaDataURL));
                var fileHash = headerResponse.Headers.ETag.Tag;

                if (!fileHash.Equals(oldHash))
                {
                    Directory.CreateDirectory(metaDataDirectory);
                    using (var file = await _client.GetStreamAsync(metaDataURL))
                    {
                        using (var fileStream = File.Create(metaDataFullPath))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            var zipFile = ZipFile.OpenRead(metaDataFullPath);
            var metaData = zipFile.Entries.FirstOrDefault(entry =>
                entry.Name.Equals(metaDataFileName, StringComparison.OrdinalIgnoreCase));

            Metadata.Metadata gameMetaData;

            if (metaData != null)
            {
                
                using (var metaDataStream = metaData.Open())
                using (XmlReader reader = XmlReader.Create(metaDataStream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Metadata.Metadata));
                    gameMetaData = (Metadata.Metadata)xmlSerializer.Deserialize(reader);
                }
            }


            /*
            var file = await _client.GetStreamAsync("https://gamesdb.launchbox-app.com/Metadata.zip");
            using (var zipArchive = new ZipArchive(file, ZipArchiveMode.Read))
            {
                zipArchive.ExtractToDirectory(@"C:\zipTest");
            }
            */
        }
    }
}
