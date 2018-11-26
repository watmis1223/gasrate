using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OleDb;

namespace CalculationOilPrice.Library.Storage
{
    static partial class StorageOperator
    {
        static string GetValueString(DataColumn primaryKey, DataRow currentRow)
        {
            string valueString = string.Empty;

            if (primaryKey.DataType == typeof(string))
            {
                valueString = string.Format("'{0}'", (currentRow[primaryKey] != null ? currentRow[primaryKey].ToString() : ""));
            }
            else if (primaryKey.DataType == typeof(Guid))
            {
                valueString = string.Format("'{0}'", (currentRow[primaryKey] != null ? currentRow[primaryKey].ToString() : ""));
            }
            else if (primaryKey.DataType == typeof(DateTime))
            {
                valueString = string.Format("'{0}'", (currentRow[primaryKey] != null ? currentRow[primaryKey].ToString() : DateTime.Now.ToString()));
            }
            else
            {
                valueString = string.Format("{0}", (currentRow[primaryKey] != null ? currentRow[primaryKey].ToString() : "0"));
            }

            return valueString;
        }

        static bool IsExistKey(DataTable table, DataColumn primaryKey, DataRow currentRow, SqlConnection openingConnection)
        {
            bool isExist = false;
            StringBuilder query = new StringBuilder();

            query.AppendFormat("select [{0}] from [{1}]", primaryKey.ColumnName, table.TableName);
            query.AppendFormat(" where [{0}]={1}", primaryKey.ColumnName, GetValueString(primaryKey, currentRow));

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = openingConnection;
            cmd.CommandText = query.ToString();
            object returnKey = cmd.ExecuteScalar();
            cmd.Dispose();

            if (returnKey != null)
            {
                isExist = true;
            }

            return isExist;
        }

        static List<DataColumn> GetSaveColumns(DataTable table, DataColumn[] exceptSaveColumns)
        {
            List<DataColumn> saveColumns = new List<DataColumn>();

            if (exceptSaveColumns != null)
            {
                bool isExceptColumn = false;

                foreach (DataColumn dc in table.Columns)
                {
                    isExceptColumn = false;

                    foreach (DataColumn exceptDc in exceptSaveColumns)
                    {
                        if (dc.ColumnName == exceptDc.ColumnName)
                        {
                            isExceptColumn = true;
                            break;
                        }
                    }

                    if (!isExceptColumn)
                    {
                        saveColumns.Add(dc);
                    }
                }
            }
            else
            {
                foreach (DataColumn dc in table.Columns)
                {
                    saveColumns.Add(dc);
                }
            }

            return saveColumns;
        }

        static void UpdateRow(DataRow dr, DataColumn primaryKey, List<DataColumn> saveColumns, SqlConnection openingConnection)
        {
            StringBuilder query = new StringBuilder();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = openingConnection;
            query.AppendFormat("update [{0}] set ", dr.Table.TableName);

            foreach (DataColumn dc in saveColumns)
            {
                if (dc.ColumnName == primaryKey.ColumnName)
                {
                    continue;
                }

                query.AppendFormat("[{0}]={1},", dc.ColumnName, GetValueString(dc, dr));
            }
            query.Remove(query.Length - 1, 1);

            query.AppendFormat(" where [{0}]={1}", primaryKey.ColumnName, GetValueString(primaryKey, dr));

            cmd.CommandText = query.ToString();
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        static void UpdateRow(DataRow dr, DataColumn primaryKey, List<DataColumn> saveColumns, string connectionString = null)
        {
            StringBuilder query = new StringBuilder();
            string sConn = !String.IsNullOrWhiteSpace(connectionString) ? connectionString : ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(sConn))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                query.AppendFormat("update [{0}] set ", dr.Table.TableName);
                connection.Open();

                foreach (DataColumn dc in saveColumns)
                {
                    if (dc.ColumnName == primaryKey.ColumnName)
                    {
                        continue;
                    }

                    query.AppendFormat("[{0}]={1},", dc.ColumnName, GetValueString(dc, dr));
                }
                query.Remove(query.Length - 1, 1);

                query.AppendFormat(" where [{0}]={1}", primaryKey.ColumnName, GetValueString(primaryKey, dr));

                cmd.CommandText = query.ToString();
                cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
            }
        }

        public static void SaveTable(DataTable table, DataColumn primaryKey, DataColumn[] ignoreSaveColumns)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder queryValues = new StringBuilder();
            List<DataColumn> saveColumns = GetSaveColumns(table, ignoreSaveColumns);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                ///building columns
                query.AppendFormat("insert into [{0}](", table.TableName);
                foreach (DataColumn dc in saveColumns)
                {
                    query.AppendFormat("[{0}],", dc.ColumnName);
                }
                query.Remove(query.Length - 1, 1);
                query.Append(")");

                ///Building values routine below
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();
                foreach (DataRow dr in table.Rows)
                {
                    if (IsExistKey(table, primaryKey, dr, connection))
                    {
                        UpdateRow(dr, primaryKey, saveColumns, connection);
                        continue;
                    }

                    queryValues = new StringBuilder(query.ToString());
                    queryValues.Append(" values(");
                    foreach (DataColumn dc in saveColumns)
                    {
                        queryValues.AppendFormat("{0},", GetValueString(dc, dr));
                    }
                    queryValues.Remove(queryValues.Length - 1, 1);

                    queryValues.Append(")");

                    cmd.CommandText = queryValues.ToString();
                    cmd.ExecuteNonQuery();
                }
                connection.Close();
                cmd.Dispose();
            }
        }

        public static long InsertRowReturnIdentity(DataRow dr, DataColumn primaryKey, DataColumn[] ignoreSaveColumns)
        {
            long iIdentity = 0;

            StringBuilder query = new StringBuilder();
            StringBuilder queryValues = new StringBuilder();
            List<DataColumn> saveColumns = GetSaveColumns(dr.Table, ignoreSaveColumns);

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString))
            {
                ///building columns
                query.AppendFormat("insert into [{0}](", dr.Table.TableName);
                foreach (DataColumn dc in saveColumns)
                {
                    query.AppendFormat("[{0}],", dc.ColumnName);
                }
                query.Remove(query.Length - 1, 1);
                query.Append(")");

                ///Building values routine below
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();

                queryValues = new StringBuilder(query.ToString());
                queryValues.Append(" values(");
                foreach (DataColumn dc in saveColumns)
                {
                    queryValues.AppendFormat("{0},", GetValueString(dc, dr));
                }
                queryValues.Remove(queryValues.Length - 1, 1);
                queryValues.Append(");");

                //select identity
                queryValues.Append(" SELECT CAST(scope_identity() AS bigint);");

                cmd.CommandText = queryValues.ToString();
                iIdentity = Convert.ToInt64(cmd.ExecuteScalar());
                connection.Close();
                cmd.Dispose();
            }

            return iIdentity;
        }

        public static void InsertRowManualIncreaseID(DataRow dr, DataColumn primaryKey, DataColumn[] ignoreSaveColumns, string connectionString = null)
        {

            StringBuilder query = new StringBuilder();
            StringBuilder queryValues = new StringBuilder();
            List<DataColumn> saveColumns = GetSaveColumns(dr.Table, ignoreSaveColumns);

            string conn = !String.IsNullOrWhiteSpace(connectionString) ? connectionString : ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(conn))
            {
                ///building columns
                query.AppendFormat("insert into [{0}](", dr.Table.TableName);
                foreach (DataColumn dc in saveColumns)
                {
                    query.AppendFormat("[{0}],", dc.ColumnName);
                }
                query.Remove(query.Length - 1, 1);
                query.Append(")");

                ///Building values routine below
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();

                queryValues = new StringBuilder(query.ToString());
                queryValues.Append(" values(");
                foreach (DataColumn dc in saveColumns)
                {
                    if (dc.ColumnName == primaryKey.ColumnName)
                    {
                        queryValues.AppendFormat("(select max({0}) + 1 from {1} )  ,", dc.ColumnName, dr.Table.TableName);
                    }
                    else
                    {
                        queryValues.AppendFormat("{0},", GetValueString(dc, dr));
                    }
                }
                queryValues.Remove(queryValues.Length - 1, 1);
                queryValues.Append(");");

                cmd.CommandText = queryValues.ToString();
                cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
            }

        }

        public static DataTable LoadTable(string tableName, DataColumn[] specifyFields, DataColumn[] conditions, DataColumn[] orderFields, string connectionString = null)
        {
            StringBuilder query = new StringBuilder();
            StringBuilder specifyFieldString = new StringBuilder("*");
            StringBuilder conditionString = new StringBuilder();
            StringBuilder orderString = new StringBuilder();

            if (specifyFields != null)
            {
                specifyFieldString = new StringBuilder();
                foreach (DataColumn specifyField in specifyFields)
                {
                    specifyFieldString.AppendFormat("{0},", specifyField.ColumnName);
                }

                specifyFieldString.Remove(specifyFieldString.Length - 1, 1);
            }

            if (conditions != null)
            {
                conditionString = new StringBuilder(" where (");
                foreach (DataColumn condition in conditions)
                {
                    if (condition.DefaultValue.GetType() == typeof(string))
                    {
                        if (condition.DefaultValue.ToString().Contains("%"))
                        {
                            conditionString.AppendFormat(" {0} like '{1}' and", condition.ColumnName, condition.DefaultValue.ToString());
                        }
                        else
                        {
                            conditionString.AppendFormat(" {0}='{1}' and", condition.ColumnName, condition.DefaultValue.ToString());
                        }

                    }
                    else
                    {
                        conditionString.AppendFormat(" {0}={1} and", condition.ColumnName, condition.DefaultValue.ToString());
                    }

                }

                conditionString.Remove(conditionString.Length - 3, 3);
                conditionString.Append(")");
            }

            if (orderFields != null)
            {
                orderString = new StringBuilder(" order by ");
                foreach (DataColumn orderField in orderFields)
                {
                    orderString.AppendFormat("{0},", orderField.ColumnName, orderField.DefaultValue.ToString());
                }

                orderString.Remove(orderString.Length - 1, 1);
            }

            query.AppendFormat("select {0} from [{1}] {2} {3}", new object[] { specifyFieldString.ToString(), tableName, conditionString.ToString(), orderString.ToString() });

            DataTable selectTable = null;

            try
            {
                string sConn = !String.IsNullOrWhiteSpace(connectionString) ? connectionString : ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

                using (SqlConnection connection = new SqlConnection(sConn))
                {
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(query.ToString(), connection);
                    selectTable = new DataTable();
                    da.Fill(selectTable);
                    connection.Close();
                }
            }
            catch { }


            if (selectTable != null)
            {
                selectTable.TableName = tableName;
            }

            return selectTable;
        }
    }
}
