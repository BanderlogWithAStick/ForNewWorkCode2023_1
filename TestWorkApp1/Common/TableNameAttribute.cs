using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkApp1.Common
{
    public class TableNameAttribute : System.Attribute
    {
        public string TableName;
        /// <summary>
        /// Название таблицы
        /// </summary>
        /// <param name="tableName"></param>
        public TableNameAttribute(string tableName)
        {
            TableName = tableName;
        }
    }
}
