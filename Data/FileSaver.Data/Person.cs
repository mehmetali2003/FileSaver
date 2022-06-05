using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FileSaver.Data
{
    [Serializable()]
    public class Person : DBObject
    {
        #region "Public Properties"

        public Nullable<long> PersonId
        {
            get
            {
                return (Nullable<long>)this["PersonId"];
            }
            set
            {
                this["PersonId"] = value;
            }
        }

        public string FirstName
        {
            get
            {
                return (string)this["FirstName"];
            }
            set
            {
                this["FirstName"] = value;
            }
        }

        public string Surname
        {
            get
            {
                return (string)this["Surname"];
            }
            set
            {
                this["Surname"] = value;
            }
        }

        public int Age
        {
            get
            {
                return (int)this["Age"];
            }
            set
            {
                this["Age"] = value;
            }
        }

        public string Sex
        {
            get
            {
                return (string)this["Sex"];
            }
            set
            {
                this["Sex"] = value;
            }
        }

        public string Mobile
        {
            get
            {
                return (string)this["Mobile"];
            }
            set
            {
                this["Mobile"] = value;
            }
        }

        public bool Active
        {
            get
            {
                if ((byte)this["Active"] == 1)
                {
                    return true;
                }
                return false;
            }
            set
            {
                this["Active"] = (byte)(value ? 1 : 0);
            }
        }

        #endregion

        #region "Base Methods/Functions"

        public static List<Person> RetrieveAll()
        {
            List<Person> personList = new List<Person>();
            string query = "select * from Person order by PersonId";
            DbCommand cmd = Utils.FileDB.GetSqlStringCommand(query);

            using (IDataReader reader = Utils.FileDB.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    personList.Add(new Person(reader));
                }
            }

            return personList;
        }

        public override bool Save([Optional, DefaultParameterValue(null)] DbTransaction transaction)
        {
            return base.Save(transaction);
        }

        public static Person GetInstance(long? idPerson)
        {
            if (idPerson.HasValue)
            {
                var result =  (from p in RetrieveAll() where p.PersonId.Equals(idPerson) select p).FirstOrDefault();
                return result ?? new Person();
            }
            else
            {
                return new Person();
            }
        }

        private Person(IDataReader reader)
        {
            base.Initialize(reader);
        }

        private Person()
        {
            base.Initialize();
        }

        #endregion

        #region "Extended Methods / Functions"

        public List<Person> Search(string firstName, string surname)
        {
            List<Person> result =  (from p in RetrieveAll() select p).ToList();
            if (!string.IsNullOrEmpty(firstName))
            {
                result = result.Where(p => p.FirstName.Contains(firstName)).ToList();
            }

            if (!string.IsNullOrEmpty(surname))
            {
                result = result.Where(p => p.Surname.Contains(surname)).ToList();
            }

            return result;
        }
        #endregion
       
    }
}
