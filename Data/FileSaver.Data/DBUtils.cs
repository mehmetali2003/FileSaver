using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSaver.Data
{
    public class DBUtils
    {
        public static void ProActiveCache()
        {
            MappingTable.LoadCache();
            FileDBResource.LoadCache();
        }
    }
}
