﻿using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace LBGDBMetadata.LaunchBox.Api
{
    public class LbgdbApi
    {
        readonly HttpClient _client = new HttpClient();
        private Options _options = null;
        
        public LbgdbApi(Options options)
        {
            _options = options;
        }

        public async Task<string> GetMetadataHash()
        {
            var headerResponse = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Head, _options.MetaDataURL));
            return headerResponse.Headers.ETag.Tag;
        }

        public async Task<Stream> DownloadMetadata()
        {
            return await _client.GetStreamAsync(_options.MetaDataURL);
        }

        private void SomeOtherMethod()
        {
            /*
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
             */



            /*
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
