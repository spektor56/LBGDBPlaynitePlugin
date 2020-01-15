using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using LBGDBMetadata;
using LBGDBMetadata.Extensions;
using LBGDBMetadata.LaunchBox.Metadata;

namespace TESTform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        



        private async void button1_Click(object sender, EventArgs e)
        {
            var plugin = new LbgdbMetadataPlugin(null);
            plugin.UpdateMetadata(@"C:\zipTest\MetaData.zip");
        }
    }
}