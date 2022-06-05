using Microsoft.Practices.EnterpriseLibrary.Caching;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSaver.Data
{
    [Serializable()]
    public class FileDBResource
    {
        #region Properties
        public Int16 ResourceId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public bool Active { get; set; }

        #endregion

        #region "Base Methods / Functions"

        private FileDBResource(IDataReader reader)
        {
            ResourceId = reader["ResourceId"].Convert<Int16>();
            ResourceKey = reader["ResourceKey"].Convert<String>();
            ResourceValue = reader["ResourceValue"].Convert<String>();
            Active = reader["Active"].Convert<Boolean>();
        }

        public static void LoadCache()
        {
            string sql = "select * from FileDBResource where Active = 1";
            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql);
            CacheManager fileCache = CacheFactory.GetCacheManager("FileDBResource");

            using (IDataReader reader = Utils.FileDB.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    fileCache.Add(reader["ResourceKey"].Convert<String>(), reader["ResourceValue"].Convert<String>());
                }
            }

        }

        public static string GetResource(string resourceKey)
        {
            return CacheFactory.GetCacheManager("FileDBResource").GetData(resourceKey)?.ToString();
        }
        #endregion
    }
}
