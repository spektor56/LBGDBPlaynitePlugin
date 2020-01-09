namespace LBGDBMetadata.LaunchBox.Api
{
    public class Options
    {
        public string MetaDataURL { get; set; } = @"https://gamesdb.launchbox-app.com/Metadata.zip";
        //public string MetaDataDirectory { get; set; } = @"C:\zipTest\";
        //public string MetaDataArchiveName { get; set; } = @"Metadata.zip";
        public string MetaDataFileName { get; set; } = @"Metadata.xml";
    }
}
