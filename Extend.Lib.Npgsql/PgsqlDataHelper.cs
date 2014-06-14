using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using Extend.Lib.Common;
using Npgsql;
using NpgsqlTypes;

namespace Extend.Lib.Npgsql
{
    public class PgsqlDataHelper
    {
        public static NpgsqlParameter CreateInputParam(
            string paramName, NpgsqlDbType dbType, object objValue)
        {
            NpgsqlParameter parameter = new NpgsqlParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = paramName;
            parameter.NpgsqlDbType = dbType;

            if (objValue == null)
            {
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
                return parameter;
            }

            parameter.Value = objValue;
            return parameter;
        }

        public static NpgsqlParameter CreateInputParam(
            string paramName, DbType dbType, object objValue)
        {
            NpgsqlParameter parameter = new NpgsqlParameter();
            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = paramName;
            parameter.DbType = dbType;

            if (objValue == null)
            {
                parameter.IsNullable = true;
                parameter.Value = DBNull.Value;
                return parameter;
            }

            parameter.Value = objValue;
            return parameter;
        }

        public static NpgsqlParameter CreateOutputParam(
            string paramName, NpgsqlDbType dbType)
        {
            NpgsqlParameter parameter = new NpgsqlParameter();
            parameter.ParameterName = paramName;
            parameter.NpgsqlDbType = dbType;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        public static NpgsqlParameter CreateOutputParam(
            string paramName, DbType dbType)
        {
            NpgsqlParameter parameter = new NpgsqlParameter();
            parameter.ParameterName = paramName;
            parameter.DbType = dbType;
            parameter.Direction = ParameterDirection.Output;

            return parameter;
        }

        public static string GetNullableString(
            IDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetString(col);
            }
            return null;
        }

        public static int? GetNullableInt32(
            IDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetInt32(col);
            }
            return null;
        }

        public static decimal? GetNullableDecimal(
            IDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetDecimal(col);
            }
            return null;
        }

        public static double? GetNullableDouble(
            IDataReader reader, int col)
        {
            if (!reader.IsDBNull(col))
            {
                return reader.GetDouble(col);
            }
            return null;
        }


        public static NpgsqlConnection GetConnection(
            string connectionStringName)
        {
            string connectionString = string.Empty;
            bool isUseEncryptedConnectionString = false;

            try
            {
                try
                {
                    isUseEncryptedConnectionString =
                    Convert.ToBoolean(
                    ConfigurationManager.AppSettings["IsUseEncryptedConnectionString"].ToLower().Trim());
                }
                catch
                {
                    isUseEncryptedConnectionString = false;
                }

                if (isUseEncryptedConnectionString)
                {
                    ConnectionStringSettings setting =
                        ConfigurationManager.ConnectionStrings[connectionStringName];
                    string tmpStr = setting.ConnectionString;
                    connectionString = MiscHelper.Decrypt(tmpStr, "encryptstring");
                }
                else
                {
                    ConnectionStringSettings setting =
                        ConfigurationManager.ConnectionStrings[connectionStringName];
                    connectionString = setting.ConnectionString;
                }
            }
            catch (Exception ex)
            {

                connectionString = ex.Message;
            }

            NpgsqlConnection con = new NpgsqlConnection(connectionString);
            return con;
        }


        /// <summary>
        /// Load DataRowView
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataRowView LoadDataRowViewByPK(DataTable dt)
        {
            DataRowView result = null;

            if (dt != null)
            {
                DataView vw = new DataView(dt);
                if (vw.Count > 0)
                {
                    result = vw[0];
                }
            }

            return result;
        }


        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="filterQuery"></param>
        /// <param name="sortQuery"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(
            NpgsqlConnection conn, string filterQuery, string sortQuery, string tableName)
        {
            DataTable dt = null;

            NpgsqlCommand cmd = new NpgsqlCommand();

            string query = "SELECT * FROM " + tableName;
            query += String.IsNullOrEmpty(filterQuery) ? String.Empty : " WHERE "    + filterQuery;
            query += String.IsNullOrEmpty(sortQuery)   ? String.Empty : " ORDER BY " + sortQuery;

            dt = ExecuteReaderByText(conn, query, tableName);

            return dt;
        }

        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="sortQuery"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable GetDataTable(
            NpgsqlConnection conn, string sortQuery, string tableName)
        {
            DataTable dt = null;

            NpgsqlCommand cmd = new NpgsqlCommand();

            string query = "SELECT * FROM " + tableName;
            query += String.IsNullOrEmpty(sortQuery) ? String.Empty : " ORDER BY " + sortQuery;

            dt = ExecuteReaderByText(conn, query, tableName);

            return dt;
        }

        /// <summary>
        /// Get DataTable
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="query"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ExecuteReaderByText(
            NpgsqlConnection conn, string query, string tableName)
        {
            DataTable dt = null;

            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 6 * 3600;
                IDataReader reader = cmd.ExecuteReader();
                dt = new DataTable(tableName);
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                dt.Load(reader);
            }
            catch (ConstraintException ex)
            {
                if (dt != null)
                {
                    DataRow[] rowsErr = dt.GetErrors();
                }
                string msg = ex.Message;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                    conn = null;
                }
            }

            return dt;
        }

        /// <summary>
        /// Converts one or more resultsets returned in a NpgsqlDataReader to a DataSet
        /// </summary>
        /// <param name="reader">NpgsqlDataReader</param>
        /// <returns>System.Data.DataSet</returns>
        public static DataSet ConvertDataReaderToDataSet(NpgsqlDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table
                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"];
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records returned

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }

        /// <summary>
        /// converts NpgsqlDataReader to a DataTable
        /// </summary>
        /// <param name="reader">NpgsqlDataReader</param>
        /// <returns>System.Data.DataTable</returns>
        public static DataTable ConvertDataReaderToDataTable(NpgsqlDataReader reader)
        {
            System.Data.DataTable table = reader.GetSchemaTable();
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.DataColumn dc;
            System.Data.DataRow row;
            System.Collections.ArrayList al = new System.Collections.ArrayList();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                dc = new System.Data.DataColumn();
                if (!dt.Columns.Contains(table.Rows[i]["ColumnName"].ToString()))
                {
                    dc.ColumnName = table.Rows[i]["ColumnName"].ToString();
                    dc.Unique = Convert.ToBoolean(table.Rows[i]["IsUnique"]);
                    dc.AllowDBNull = Convert.ToBoolean(table.Rows[i]["AllowDBNull"]);
                    dc.ReadOnly = Convert.ToBoolean(table.Rows[i]["IsReadOnly"]);
                    al.Add(dc.ColumnName);
                    dt.Columns.Add(dc);
                }
            }
            while (reader.Read())
            {
                row = dt.NewRow();
                for (int i = 0; i < al.Count; i++)
                {
                    row[((System.String)al[i])] = reader[(System.String)al[i]];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        
    }
}
