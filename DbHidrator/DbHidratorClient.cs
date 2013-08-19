using DbHidrator.Entities;
using DbHidrator.Generator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbHidrator
{
    public class DbHidratorClient
    {
        private string connStr;
        private string provName;
        private List<Table> tables;
        private DbProviderFactory factory;
        private DbConnection conn;
        private static Random random = new Random((int)DateTime.Now.Ticks);
        private static object insertLock = new object();
        public DbHidratorClient(string connectionString, string providerName)
        {
            this.connStr = connectionString;
            this.provName = providerName;
            this.tables = new List<Table>();
            
        }

        public void GenerateSchema()
        {
            TypeGenerator generator = new TypeGenerator();
            DataTable dtTables = null, dtColumns = null;
            GetSchema(out dtTables, out dtColumns);
            CreateTables(dtTables);
            CreateColumns(dtColumns);
            foreach (var table in tables)
            {
                table.Type = generator.CreateType(table);

            }
        }

        private void CreateColumns(DataTable dtColumns)
        {
            int rowCount = dtColumns.Rows.Count;
            int ordTableName = dtColumns.Columns["TABLE_NAME"].Ordinal;
            int ordColumnName = dtColumns.Columns["COLUMN_NAME"].Ordinal;
            int ordOrdPos = dtColumns.Columns["ORDINAL_POSITION"].Ordinal;
            int ordType = dtColumns.Columns["DATA_TYPE"].Ordinal;
            int ordMaxLength = dtColumns.Columns["CHARACTER_MAXIMUM_LENGTH"].Ordinal;
            int ordPrecision = dtColumns.Columns["NUMERIC_PRECISION"].Ordinal;
            int ordScale = dtColumns.Columns["NUMERIC_SCALE"].Ordinal;
            for (int i = 0; i < rowCount; i++)
            {
                var tmp = new Column();
                tmp.TypeName = dtColumns.Rows[i][ordType].ToString();
                if (tmp.TypeName.Equals("timestamp", StringComparison.InvariantCultureIgnoreCase) || tmp.TypeName.Equals("rowversion", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                string tableName = dtColumns.Rows[i][ordTableName].ToString();
                tmp.Id = (int)dtColumns.Rows[i][ordOrdPos];
                tmp.Name = dtColumns.Rows[i][ordColumnName].ToString();

                tmp.MaxLength = dtColumns.Rows[i].IsNull(ordMaxLength) ? 0 : (int)dtColumns.Rows[i][ordMaxLength];
                tmp.Precision = dtColumns.Rows[i].IsNull(ordPrecision) ? 0 : (byte)dtColumns.Rows[i][ordPrecision];
                tmp.Scale = dtColumns.Rows[i].IsNull(ordScale) ? 0 : (int)dtColumns.Rows[i][ordScale];
                var table = tables.FirstOrDefault(x => x.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));

                table.Columns.Add(tmp);
            }
        }

        private void CreateTables(DataTable dtTables)
        {
            int rowCount = dtTables.Rows.Count;
            int ordTableName = dtTables.Columns["TABLE_NAME"].Ordinal;
            for (int i = 0; i < rowCount; i++)
            {
                var tmp = new Table();
                tmp.Name = dtTables.Rows[i][ordTableName].ToString();
                tmp.Id = i + 1;
                tables.Add(tmp);
            }
        }

        private void GetSchema(out DataTable dtTables, out DataTable dtColumns)
        {
            //var factories=DbProviderFactories.GetFactoryClasses();
            this.factory = DbProviderFactories.GetFactory(provName);
            conn = factory.CreateConnection();
            conn.ConnectionString = connStr;
            conn.Open();
            dtTables = conn.GetSchema("Tables");
            dtColumns = conn.GetSchema("Columns");
            conn.Close();
        }

        private void InsertRows(Table table, DbConnection conn, IList list)
        {
            int i = 0;
            var command = table.InsertCommand;
            var columns = table.Columns.OrderBy(x => x.Id).ToList();
            int cantColumns = columns.Count;
            conn.Open();
            using (var trans = conn.BeginTransaction())
            {
                command.Transaction = trans;
                foreach (var reg in list)
                {
                    for (i = 0; i < cantColumns; i++)
                    {
                        command.Parameters[i].Value = columns[i].ClassProperty.GetValue(reg, null);
                    }
                    command.ExecuteNonQuery();
                }
                trans.Commit();
                command.Transaction = null;
            }
            conn.Close();
        }

        private object CreateRow(Table table)
        {
            var reg = Activator.CreateInstance(table.Type);
            foreach (var col in table.Columns)
            {
                object value = CreateRandomValue(col);
                col.ClassProperty.SetValue(reg, value, null);
            }
            return reg;
        }

        private object CreateRandomValue(Column col)
        {
            object value;
            switch (col.ClassProperty.PropertyType.ToString())
            {
                case "System.Int32":
                case "System.Int64":
                    value = random.Next(0, int.MaxValue);
                    break;
                case "System.Double":
                    value = random.NextDouble();
                    break;
                case "System.Decimal":
                    value = Convert.ToDecimal(random.NextDouble());
                    break;
                case "System.DateTime":
                    value = new DateTime(random.Next(1980, 2100), random.Next(1, 12), random.Next(1, 28));
                    break;
                case "System.Boolean":
                case "bool":
                case "Boolean":
                    value = Convert.ToBoolean(random.Next(0, 1));
                    break;
                case "System.String":
                    value = CreateRandomString(col.MaxLength);
                    break;
                default:
                    value = "";
                    break;
            }
            return value;
        }

        private string CreateRandomString(int maxLength)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < maxLength; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        private void CreateQueries(Table table)
        {
            DataSet tmp = new DataSet();
            if (conn == null)
            {
                this.conn = factory.CreateConnection();
                this.conn.ConnectionString = connStr;
            }

            DbDataAdapter adapter = factory.CreateDataAdapter();
            adapter.SelectCommand = factory.CreateCommand();
            adapter.SelectCommand.Connection = conn;
            adapter.SelectCommand.CommandText = "select * from " + table.Name;
            adapter.FillSchema(tmp, SchemaType.Source);
            var commBuilder = factory.CreateCommandBuilder();
            commBuilder.DataAdapter = adapter;

            table.InsertCommand = commBuilder.GetInsertCommand();
            table.UpdateCommand = commBuilder.GetUpdateCommand();
            table.DeleteCommand = commBuilder.GetDeleteCommand();


        }

        private IList CreateListType(Table table)
        {
            Type typeList = typeof(List<>);
            Type combType = typeList.MakeGenericType(table.Type);
            IList elements = (IList)Activator.CreateInstance(combType);
            foreach (var col in table.Columns)
            {
                var prop = table.Type.GetProperty(col.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                if (prop != null && prop.CanWrite)
                {
                    col.ClassProperty = prop;
                }
            }
            return elements;
        }

        public string[] GetTablesNames()
        {
            return tables.Select(x => x.Name).ToArray();
        }

        public void GenerateRowsInRandomTimes(int rows, string[] tablesNames)
        {
            Dictionary<string, IList> dataGenerated = new Dictionary<string, IList>();
            var filteredTables = this.tables.Where(x => tablesNames.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)).ToList();
            foreach (var table in filteredTables)
            {
                CreateQueries(table);
            }

            foreach (var table in filteredTables)
            {
                var list = CreateListType(table);
                for (int i = 0; i < rows; i++)
                {
                    var reg = CreateRow(table);
                    list.Add(reg);
                }
                dataGenerated.Add(table.Name, list);
            }
            conn.Open();
            Parallel.ForEach(dataGenerated, data =>
            {
                var table1 = filteredTables.FirstOrDefault(x => x.Name == data.Key);
                Parallel.ForEach(data.Value.Cast<object>(), reg =>
                {
                    Thread.Sleep(random.Next(100, 3000));
                    InsertRow(table1, conn, reg);
                });
            });
            
            conn.Close();
        }

        private void InsertRow(Table table, DbConnection conn, object reg)
        {
            int i = 0;
            var command = table.InsertCommand;
            var columns = table.Columns.OrderBy(x => x.Id).ToList();
            int cantColumns = columns.Count;
            lock (insertLock)
            {
                for (i = 0; i < cantColumns; i++)
                {
                    command.Parameters[i].Value = columns[i].ClassProperty.GetValue(reg, null);
                }
                command.ExecuteNonQuery();
            }

        }

        public void GenerateNRows(int rows, string[] tables)
        {
            var filteredTables = this.tables.Where(x => tables.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)).ToList();
            foreach (var table in filteredTables)
            {
                CreateQueries(table);
            }

            foreach (var table in filteredTables)
            {
                var list = CreateListType(table);
                for (int i = 0; i < rows; i++)
                {
                    var reg = CreateRow(table);
                    list.Add(reg);
                }
                InsertRows(table, conn, list);
            }

        }
    }
}
