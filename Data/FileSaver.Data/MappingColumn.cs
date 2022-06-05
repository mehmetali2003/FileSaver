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
    public class MappingColumn
    {
        #region Properties
        public Int16 IdColumn { get; set; }

        public Int16 IdTable { get; set; }

        public string DsColumnName { get; set; }

        public string DsDbType { get; set; }

        public string DsClassMember { get; set; }

        public string DsRequiredMsg { get; set; }

        #endregion

        #region "Base Methods / Functions"
        public static List<MappingColumn> RetrieveAll(long idTable)
        {
            string sql = "select * from MappingColumn where IdTable = @IdTable order by 1";
            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql);
            Utils.FileDB.AddInParameter(cmd, "IdTable", DbType.Int64, idTable);

            List<MappingColumn> mappingColumns = new List<MappingColumn>();
            using (IDataReader reader = Utils.FileDB.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    mappingColumns.Add(new MappingColumn(reader));
                }
            }

            return mappingColumns;
        }

        private MappingColumn(IDataReader reader)
        {
            IdColumn = reader["IdColumn"].Convert<Int16>();
            IdTable = reader["IdTable"].Convert<Int16>();
            DsColumnName = reader["DsColumnName"].Convert<String>();
            DsDbType = reader["DsDbType"].Convert<String>();
            DsClassMember = reader["DsClassMember"].Convert<String>();
            DsRequiredMsg = reader["DsRequiredMsg"].Convert<String>();
        }
        #endregion
    }
}
