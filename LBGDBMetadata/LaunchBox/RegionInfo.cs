using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBGDBMetadata.LaunchBox
{
    public class RegionInfo
    {
        public RegionInfo(string regionName, string parentRegion)
        {
            RegionName = regionName;
            ParentRegion = parentRegion;
        }

        public string RegionName { get; private set; }
        public string ParentRegion { get; private set; }
    }
}
