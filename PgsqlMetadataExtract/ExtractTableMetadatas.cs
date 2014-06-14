using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Extend.Lib.Npgsql;
using Npgsql;



namespace PgsqlMetadataExtract
{
    public class ExtractTableMetadatas : PgsqlBusinessBase
    {
        private Table _Table;

        public Table Table
        {
            get { return _Table; }
            set { _Table = value; }
        }

        public ExtractTableMetadatas(string connectionString, string tableName)
            : base("PgsqlGetMetadata")
        {
            this.ConnectionString = connectionString;
            _Table = new Table();
            _Table.TableFields = TableFieldsGetAll(tableName);
            _Table.ForeignKeys = ForeignKeysGetAll(tableName);
            _Table.PrimaryKey = GetPrimaryKey(tableName);
        }


        public List<TableField> TableFieldsGetAll(
            string tableName)
        {

            List<TableField> tableFields = new List<TableField>();
            DataTable dT = null;

            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            try
            {
                string strCmd = "meta_extract_table_fields";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_tablename", DbType.String, tableName));

                con.Open();
                NpgsqlTransaction t = con.BeginTransaction();

                NpgsqlDataReader reader = cmd.ExecuteReader();

                dT = new DataTable("TableFields");
                dT.Load(reader);

                foreach (DataRow row in dT.Rows)
                {
                    var item = new TableField();
                    item.ColumnName = row["column_name"].ToString().Trim();
                    item.DataType = row["data_type"].ToString().Trim();
                    item.IsNullable = Convert.ToBoolean(row["is_nullable"].ToString().Trim());
                    tableFields.Add(item);
                }

                t.Commit();

            }
            catch (Exception excp)
            {
                throw excp;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con = null;
                }
            }

            return tableFields;
        }


        public List<ForeignKey> ForeignKeysGetAll(
           string tableName)
        {

            List<ForeignKey> foreignKeys = new List<ForeignKey>();
            DataTable dT = null;

            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            try
            {
                string strCmd = "meta_extract_table_fks";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_tablename", DbType.String, tableName));

                con.Open();
                NpgsqlTransaction t = con.BeginTransaction();

                NpgsqlDataReader reader = cmd.ExecuteReader();

                dT = new DataTable("ForeignKeys");
                dT.Load(reader);

                foreach (DataRow row in dT.Rows)
                {
                    var item = new ForeignKey();
                    item.RefTable = row["ref_table"].ToString().Trim();
                    item.ForeignCol = row["foreign_col"].ToString().Trim();
                    item.RefCol = row["ref_col"].ToString().Trim();
                    foreignKeys.Add(item);
                }

                t.Commit();

            }
            catch (Exception excp)
            {
                throw excp;
            }
            finally
            {
                if (con != null)
                {
                    con.Close();
                    con = null;
                }
            }

            return foreignKeys;
        }



        public string GetPrimaryKey(
           string tableName)
        {
            string result = string.Empty;

            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();
                string strCmd = "meta_extract_table_pk";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_table_name", DbType.String, tableName));

                result = (string)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return result;

        }

        public string GetDataTypeByColumnName(string columnName)
         {
             string result = "Unknown";

             foreach (TableField item in _Table.TableFields)
             {
                 if (item.ColumnName.ToLower().Trim()==columnName.ToLower().Trim())
                 {
                     result = item.DataType;
                     break;
                 }

             }

             return result;

         }

        public int GetQtyUniqueIndexOtherThanPK(
        string tableName)
        {
            int rowsAffected = 0;

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();
                string strCmd = "meta_get_qty_unique_index_other_than_pk";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_table_name", DbType.String, tableName));

                rowsAffected = (int)cmd.ExecuteScalar();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return rowsAffected;

        }  

        public string GetFirstTimeStampColumn()
        {
            string columnName = string.Empty;
            foreach (TableField item in _Table.TableFields)
            {
                if (item.DataType.Trim().IndexOf("timestamp") >= 0)
                {
                    columnName = item.ColumnName;
                    break;
                }
            }
            return columnName;
        }
        
    }



    public class Table
    {
        private List<TableField> _TableFields;
        private string _PrimaryKey;
        private List<ForeignKey> _ForeignKeys;

        public List<TableField> TableFields
        {
            get { return _TableFields; }
            set { _TableFields = value; }
        }

        public string PrimaryKey
        {
            get { return _PrimaryKey; }
            set { _PrimaryKey = value; }
        }

        public List<ForeignKey> ForeignKeys
        {
            get { return _ForeignKeys; }
            set { _ForeignKeys = value; }
        }

    }

    public class ForeignKey
    {
        private string _RefTable;
        private string _ForeignCol;
        private string _RefCol;

        public string RefTable
        {
            get { return _RefTable; }
            set { _RefTable = value; }
        }

        public string ForeignCol
        {
            get { return _ForeignCol; }
            set { _ForeignCol = value; }
        }

        public string RefCol
        {
            get { return _RefCol; }
            set { _RefCol = value; }
        }
    }


    public class TableField
    {
        private string _ColumnName;
        private string _DataType;
        private bool   _IsNullable;
       
        public string ColumnName
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }

        public string DataType
        {
            get { return _DataType; }
            set { _DataType = value; }
        }

        public bool IsNullable
        {
            get { return _IsNullable; }
            set { _IsNullable = value; }
        }
       
    }


}
