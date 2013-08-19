using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHidrator.Entities
{
    public class Table
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }
        public List<PrimaryKey> PKs { get; set; }
        public List<Column> Columns { get; set; }
        public Table()
        {
            PKs = new List<PrimaryKey>();
            Columns = new List<Column>();
        }

        public System.Data.Common.DbCommand InsertCommand { get; set; }

        public System.Data.Common.DbCommand UpdateCommand { get; set; }

        public System.Data.Common.DbCommand DeleteCommand { get; set; }
    }
}
