using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Data.Common;
using System.Data;
using System.Web;

namespace FileSaver.Data
{
    [Serializable()]
    public class DBObject
    {
        public string TransactionLogInfo;
        public long? TransactionLogId;
        public List<string> Errors;
        public DateTime? DtTimeStamp
        {
            get
            {
                if (this["DtTimeStamp"] != null && this["DtTimeStamp"] != DBNull.Value)
                {
                    return (DateTime)this["DtTimeStamp"];
                }
                return null;
            }
            set
            {
                this["DtTimeStamp"] = value;
            }
        }

        private MappingTable _MappingData;
        private List<DBField> _DirtyFields;
        private List<MappingColumn> _RequiredColumns;

        #region Public Properties

        public Object this[string key]
        {
            get { return FindField(key).Value; }
            set { FindField(key).Value = value; }
        }

        public List<DBField> Fields {get; set; }

        internal bool IsDirty
        {
            get
            {
                return (from ff in Fields where ff.IsDirty select ff).Any();
            }
        }
        #endregion

        #region "Private Properties..."
        private MappingTable MappingData
        {
            get
            {
                if (_MappingData == null)
                {
                    _MappingData = MappingTable.GetInstance(base.GetType().Name);
                }
                return _MappingData;
            }
            set
            {
                _MappingData = value;
            }
        }

        private List<DBField> DirtyFields
        {
            get
            {
                if (_DirtyFields == null)
                {
                    _DirtyFields = (from c in Fields where c.IsDirty select c).ToList();
                }

                return _DirtyFields;
            }
            set
            {
                _DirtyFields = value;

                if (value == null)
                {
                    for (int i = 0; i < this.Fields.Count; i++)
                    {
                        this.Fields[i].IsDirty = false;
                    }
                }
            }
        }

        private List<MappingColumn> RequiredColumns
        {
            get
            {
                if (_RequiredColumns == null)
                {
                    return (from c in this.MappingData.Columns where c.DsRequiredMsg != string.Empty select c).ToList();
                }
                return _RequiredColumns;
            }
            set
            {
                _RequiredColumns = value;
            }
        }
        #endregion

        #region "Public Overridable Functions..."
        public virtual bool Save([Optional, DefaultParameterValue(null)] DbTransaction transaction)
        {
            return IsValid() && BaseSave(transaction);
        }

        public virtual bool Delete(long primaryKeyValue)
        {
            bool success = false;

            if (IsDeletable(primaryKeyValue))
            {
                string sql = "update " + this.MappingData.DsTable + " set Active = 0, DtTimeStamp = @DtTimeStamp where " + this.MappingData.DsColumnPk + "=@" + this.MappingData.DsColumnPk + " AND Active=1";
                DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql);
                Utils.FileDB.AddInParameter(cmd, this.MappingData.DsColumnPk, DbType.Int64, primaryKeyValue);
                Utils.FileDB.AddInParameter(cmd, "DtTimeStamp", DbType.DateTime, System.DateTime.Now);
                success = Utils.FileDB.ExecuteNonQuery(cmd) == 1;
                if (!success)
                {
                    Errors.Add(FileDBResource.GetResource("DeleteFail"));

                }
            }
            else
            {
                Errors.Add(FileDBResource.GetResource("DeleteFailConstraint"));
            }

            return success;
        }

        #endregion
        #region "Friend Overridable Functions..."
        internal virtual bool IsValid()
        {
            Errors.Clear();
            for (int i = 0; i < this.RequiredColumns.Count; i++)
            {
                if (this.RequiredColumns[i].DsDbType == "DbType.String")
                {
                    if (this.RequiredColumns[i].DsClassMember.Equals(string.Empty))
                    {
                        Errors.Add(FileDBResource.GetResource(this.RequiredColumns[i].DsRequiredMsg));
                    }
                }
                else
                {
                    if (this.RequiredColumns[i].DsClassMember == null)
                    {
                        Errors.Add(FileDBResource.GetResource(this.RequiredColumns[i].DsRequiredMsg));
                    }
                }
            }

            this.RequiredColumns = null;

            return Errors.Count == 0;

        }

        internal virtual bool IsDeletable(long primaryKeyValue)
        {
            return true;
        }
        #endregion

        #region "Private Sub/Functions..."
        private DBField FindField(string name)
        {
            return (from s in Fields where s.Name.Equals(name) select s).First();
        }

        private bool DBSave()
        {
            bool success = false;
            string dbOperation = string.Empty;

            DbConnection conn = Utils.FileDB.CreateConnection();
            conn.Open();
            DbTransaction transaction = conn.BeginTransaction();


            try
            {
                if (this[this.MappingData.DsMemberPk] == null)
                {
                    success = Insert(transaction, false);
                    dbOperation = "I";
                }
                else
                {
                    success = Update(transaction);
                    dbOperation = "U";
                }
            }
            catch (Exception ex)
            {
                if (transaction.Connection.State == ConnectionState.Open)
                {
                    transaction.Rollback();
                }

                success = false;
                Errors.Add(ex.Message);
            }
            finally
            {
                if (success)
                {
                    transaction.Commit();
                }
                transaction.Dispose();
            }

            return success;
        }

        private bool DBSave(DbTransaction transaction)
        {
            bool success = false;
            string dbOperation = string.Empty;

            try
            {
                if (this[this.MappingData.DsMemberPk] == null)
                {
                    success = Insert(transaction, false);
                    dbOperation = "I";
                }
                else
                {
                    success = Update(transaction);
                    dbOperation = "U";
                }
            }
            catch (Exception ex)
            {
                success = false;
                Errors.Add(ex.Message);
            }

            return success;
        }

        private bool Insert(DbTransaction transaction, bool isPKAlreadySet)
        {
            bool success = false;
            StringBuilder sql = new StringBuilder();
            StringBuilder sqlParam = new StringBuilder();
            string columnName = string.Empty;
            sql.Append("INSERT INTO " + this.MappingData.DsTable + "(");

            for (int i = 0; i < this.DirtyFields.Count; i++)
            {
                columnName = FindColumn(_DirtyFields[i].Name).DsColumnName;
                sql.Append(columnName + ", ");
                sqlParam.Append("@" + columnName + ", ");
            }

            sql.Append("DtTimeStamp");
            sqlParam.Append("@DtTimeStamp");

            sql.Append(") ");
            sql.Append(" VALUES (");
            sql.Append(sqlParam.ToString());
            sql.Append(") ");


            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql.ToString());
            Utils.FileDB.AddInParameter(cmd, this.MappingData.DsColumnPk, DbType.Int64, this[this.MappingData.DsMemberPk]);
            Utils.FileDB.AddInParameter(cmd, "DtTimeStamp", DbType.DateTime, DateTime.Now);
            MappingColumn column;

            for (int i = 0; i < this.DirtyFields.Count; i++)
            {
                column = FindColumn(_DirtyFields[i].Name);

                if (column.DsColumnName != this.MappingData.DsColumnPk)
                {
                    Utils.FileDB.AddInParameter(cmd, column.DsColumnName, GetDBColumnType(column.DsDbType), _DirtyFields[i].Value);
                }
            }
            column = null;

            using (cmd)
            {
                if (transaction == null)
                {
                    if (Utils.FileDB.ExecuteNonQuery(cmd).Equals(1))
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                else
                {
                    if (Utils.FileDB.ExecuteNonQuery(cmd, transaction).Equals(1))
                    {
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
            }

            this.DirtyFields = null;
            return success;
        }

        private bool Update(DbTransaction transaction)
        {
            if ((from fld in this.Fields where fld.Name.Equals(this.MappingData.DsMemberPk) select fld).First().IsDirty)
            {
                return Insert(transaction, true);
            }
            else
            {
                bool success = false;
                StringBuilder sql = new StringBuilder();
                string columnName = string.Empty;

                sql.Append("UPDATE " + this.MappingData.DsTable + " SET ");

                for (int i = 0; i < this.DirtyFields.Count; i++)
                {
                    columnName = FindColumn(_DirtyFields[i].Name).DsColumnName;
                    sql.Append(columnName + " = @" + columnName + ", ");
                }

                sql.Append("DtTimeStamp = @DtTimeStamp");
                sql.Append(" WHERE " + this.MappingData.DsColumnPk + " = @" + this.MappingData.DsColumnPk);

                DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql.ToString());
                Utils.FileDB.AddInParameter(cmd, this.MappingData.DsColumnPk, DbType.Int64, this[this.MappingData.DsMemberPk]);
                Utils.FileDB.AddInParameter(cmd, "DtTimeStamp", DbType.DateTime, DateTime.Now);

                MappingColumn column;
                for (int i = 0; i < this.DirtyFields.Count; i++)
                {
                    column = FindColumn(_DirtyFields[i].Name);
                    Utils.FileDB.AddInParameter(cmd, column.DsColumnName, GetDBColumnType(column.DsDbType), _DirtyFields[i].Value);
                }

                column = null;

                using (cmd)
                {
                    if (transaction == null)
                    {
                        if (Utils.FileDB.ExecuteNonQuery(cmd).Equals(1))
                        {
                            success = true;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    else
                    {
                        if (Utils.FileDB.ExecuteNonQuery(cmd, transaction).Equals(1))
                        {
                            success = true;
                        }
                        else
                        {
                            success = false;
                        }
                    }
                    if (!success)
                    {
                        Errors.Add(FileDBResource.GetResource("UpdateFail"));
                    }
                }

                this.DirtyFields = null;
                return success;
            }
        }

        private DbType GetDBColumnType(string dbTypeName)
        {
            DbType dbType = new DbType();

            switch (dbTypeName)
            {
                case "DbType.String":
                    dbType = DbType.String;
                    break;
                case "DbType.Int64":
                    dbType = DbType.Int64;
                    break;
                case "DbType.DateTime":
                    dbType = DbType.DateTime;
                    break;
                case "DbType.Decimal":
                    dbType = DbType.Decimal;
                    break;
                case "DbType.Byte":
                    dbType = DbType.Byte;
                    break;
                default:
                    break;
            }
            return dbType;
        }

        private List<DBField> CopyFields()
        {
            List<DBField> flds = new List<DBField>();

            foreach (DBField field in Fields)
            {
                flds.Add(new DBField(field.Name, field.Value));
            }

            return flds;

        }

        private object ToType(object value, string dbType)
        {
            object toValue = null;

            if (dbType.Equals("DbType.String"))
            {
                toValue = value + string.Empty;
            }
            else
            {
                if (value == null || object.ReferenceEquals(value, DBNull.Value))
                {
                    toValue = null;
                }
                else
                {
                    switch (dbType)
                    {
                        case "DbType.Int64":
                            toValue = Convert.ToInt64(value);
                            break;
                        case "DbType.Decimal":
                            toValue = Convert.ToDecimal(value);
                            break;
                        case "DbType.DateTime":
                            toValue = Convert.ToDateTime(value);
                            break;
                        case "DbType.Byte":
                            toValue = Convert.ToByte(value);
                            break;
                        default:
                            toValue = value;
                            break;
                    }
                }
            }

            return toValue;

        }

        private MappingColumn FindColumn(string dsClassMember)
        {
            return (from c in this.MappingData.Columns where c.DsClassMember.Equals(dsClassMember) select c).First<MappingColumn>();
        }

        #endregion

        #region "Friend Sub/Functions"
        internal void Initialize()
        {
            this.Errors = new List<string>();
            this.Fields = new List<DBField>();

            for (int i = 0; i < this.MappingData.Columns.Count; i++)
            {
                this.Fields.Add(new DBField(this.MappingData.Columns[i].DsClassMember, (this.MappingData.Columns[i].DsDbType == "DbType.String" ? "" : null)));
            }
        }

        internal void Initialize(IDataReader row)
        {
            this.Errors = new List<string>();
            this.Fields = new List<DBField>();

            for (int i = 0; i < this.MappingData.Columns.Count; i++)
            {
                this.Fields.Add(new DBField(this.MappingData.Columns[i].DsClassMember, ToType(row[this.MappingData.Columns[i].DsColumnName], this.MappingData.Columns[i].DsDbType)));
            }
        }

        internal void GetIt(long? pk)
        {
            string sql = "SELECT * FROM " + this.MappingData.DsTable + " WHERE " + this.MappingData.DsColumnPk + " = @" + this.MappingData.DsColumnPk;
            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(sql);
            Utils.FileDB.AddInParameter(cmd, this.MappingData.DsColumnPk, DbType.Int64, pk);

            using (IDataReader reader = Utils.FileDB.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    Initialize(reader);
                }
                else
                {
                    Initialize();
                }
            }

        }

        internal object Clone()
        {
            DBObject newBase = (DBObject)base.MemberwiseClone();
            newBase.Fields = CopyFields();
            newBase.Errors = new List<string>();
            newBase.TransactionLogId = null;
            newBase.TransactionLogInfo = string.Empty;
            return newBase;
        }

        private bool BaseSave(DbTransaction transaction)
        {
            bool dBSuccess = false;

            if (this.DirtyFields.Count > 0)
            {
                if (transaction == null)
                {
                    dBSuccess = DBSave();
                }
                else
                {
                    dBSuccess = DBSave(transaction);
                }
            }

            this.DirtyFields = null;

            return dBSuccess;
        }
        #endregion
    }
}
