using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using LBGDBMetadata.Api;
using Playnite.SDK;

namespace LBGDBMetadata
{
    public class LbgdbApi
    {
        readonly HttpClient _client = new HttpClient();
        private Options _options = null;
        public LbgdbApi(Options options)
        {
            _options = options;
        }
        
        public async Task<string> DownloadMetadata()
        {
            var metaDataFullPath = Path.Combine(_options.MetaDataDirectory, _options.MetaDataArchiveName);
            string fileHash = null;
            bool metaDataExists = File.Exists(metaDataFullPath);
            if (metaDataExists)
            {
                var headerResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _options.MetaDataURL));
                fileHash = headerResponse.Headers.ETag.Tag;
            }

            if (!metaDataExists || (fileHash != null && !fileHash.Equals(_options.OldHash)))
            {
                Directory.CreateDirectory(_options.MetaDataDirectory);
                using (var file = await _client.GetStreamAsync(_options.MetaDataURL))
                {
                    using (var fileStream = File.Create(metaDataFullPath))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
            }

            return fileHash;
        }

        private void SomeOtherMethod()
        {/*
            var zipFile = ZipFile.OpenRead(metaDataFullPath);
            var metaData = zipFile.Entries.FirstOrDefault(entry =>
                entry.Name.Equals(_options.MetaDataFileName, StringComparison.OrdinalIgnoreCase));

            Metadata.Metadata gameMetaData;

            if (metaData != null)
            {

                using (var metaDataStream = metaData.Open())
                using (XmlReader reader = XmlReader.Create(metaDataStream))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(Metadata.Metadata));
                    gameMetaData = (Metadata.Metadata)xmlSerializer.Deserialize(reader);
                    var games = gameMetaData.Game.Where(game => game.Name.Length < 2);
                }
            }
            */

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
