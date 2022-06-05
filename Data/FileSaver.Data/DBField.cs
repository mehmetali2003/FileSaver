using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSaver.Data
{
    [Serializable()]
    public class DBField
    {
        private Object _Value;
        private bool _IsDirty;

        public string Name { get; set; }

        public Object Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (_IsDirty == false)
                {
                    if ((_Value == null) || (_Value == DBNull.Value))
                    {
                        _IsDirty = !((value == null) || (value == DBNull.Value));
                    }
                    else
                    {
                        _IsDirty = ((value == null) || (value == DBNull.Value)) || (!_Value.Equals(value));
                    }
                }
                _Value = value;
            }
        }

        public bool IsDirty
        {
            get
            {
                return _IsDirty;
            }
            set
            {
                _IsDirty = value;
            }
        }

        public DBField(string name, Object value)
        {
            Name = name;
            _Value = value;
            _IsDirty = false;
        }
    }
}
