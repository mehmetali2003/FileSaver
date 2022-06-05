using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSaver.Data
{
    public static class Utils
    {
        public static Microsoft.Practices.EnterpriseLibrary.Data.Database FileDB = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase("FileDB");

        public static bool IsDbNullOrZero(this object value)
        {
            return (value == null) || (value == DBNull.Value) || (0 == Convert.ToInt64(value));
        }

        public static bool IsDbNullOrStringEmpty(this object value)
        {
            return (value == null) || (value == DBNull.Value) || (Convert.ToString(value) == string.Empty);
        }

        public static bool HasItem(this object value)
        {
            if (value is DataTable table)
            {
                return (value == null) || table.Rows?.Count > 0;
            }
            else if (value is IList list)
            {
                return (value == null) || list.Count > 0;
            }

            return false;
        }
    }
}
