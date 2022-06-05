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
    public class MappingTable
    {
        #region Properties
        public Int16 IdTable { get; set; }

        public string DsTable { get; set; }

        public string DsColumnPk { get; set; }

        public string DsClassName { get; set; }

        public bool FlCached { get; set; }

        public string DsMemberPk { get; set; }

        public List<MappingColumn> Columns { get; set; }
        #endregion

        #region "Base Methods / Functions"

        private MappingTable(IDataReader reader)
        {
            IdTable = reader["IdTable"].Convert<Int16>();
            DsTable = reader["DsTable"].Convert<String>();
            DsColumnPk = reader["DsColumnPk"].Convert<String>();
            DsClassName = reader["DsClassName"].Convert<String>();
            DsMemberPk = reader["DsMemberPk"].Convert<String>();
            FlCached = reader["FlCached"].Convert<Boolean>();
            Columns = MappingColumn.RetrieveAll(IdTable);
        }

        public static void LoadCache()
        {
            string sql = "select * from MappingTable order by DsTable";
            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql);
            CacheManager fileCache = CacheFactory.GetCacheManager("MappingTable");

            using (IDataReader reader = Utils.FileDB.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    fileCache.Add(reader["DsTable"].Convert<String>(), new MappingTable(reader));
                }
            }

        }

        public static MappingTable GetInstance(string className)
        {
            return (MappingTable)CacheFactory.GetCacheManager("MappingTable").GetData(className);
        }
        #endregion

    }

}
