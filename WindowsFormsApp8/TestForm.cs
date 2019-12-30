using LBGDBMetadata;
using System;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class TestForm : Form
    {
        
        public TestForm()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            LbgdbMetadataPlugin plugin = new LbgdbMetadataPlugin(null);
            var settings = plugin.GetSettings(true);
            

        }
    }
}
