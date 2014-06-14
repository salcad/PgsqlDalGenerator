using System;
using System.Data;
using System.Data.SqlClient;
using Npgsql;

namespace Extend.Lib.Npgsql
{
    interface ICrud
    {
        void Insert();
        void UpdateByPK();
        void DeleteByPK();
        void SelectAll();
        void SelectByPK();
    }

    public enum MatchOptions
    {
        Exact, 
        Phrase 
    }

    public abstract class PgsqlBusinessBase 
    {
        public PgsqlBusinessBase(string tableName)
        {
            this.TableName = tableName;
            this._ConnectionStringName = "Default";
        }

        public const string PChr = "@";
        public const int ReportTimeout = 2 * 3600;

        protected virtual string CountQuery
        {
            get { return ("SELECT COUNT(*) FROM " + this.TableName); }
        }

        protected virtual string SelectAllQuery
        {
            get { return "SELECT * FROM " + TableName; }
        }

        private string _TableName;
        public string TableName
        {
            get { return _TableName; }
            set { _TableName = value; }
        }

        private string _ConnectionStringName;
        public string ConnectionStringName
        {
            get { return _ConnectionStringName; }
            set { _ConnectionStringName = value; }
        }

        private string _ConnectionString;
        public string ConnectionString
        {
            get { return _ConnectionString; }
            set { _ConnectionString = value; }
        }

        /// <summary>
        /// Load Data Table
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable LoadDataTable(string query)
        {
            DataTable dt = null;
            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            dt = PgsqlDataHelper.ExecuteReaderByText(con, query, this.TableName);
            return dt;
        }

        /// <summary>
        /// Load Data Table All And Sort
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public DataTable LoadDataTableAll(string sort)
        {
            DataTable dt = null;
            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            dt = PgsqlDataHelper.GetDataTable(con, sort, this.TableName);
            return dt;
        }

        /// <summary>
        /// Load Data Table Filter And Sort
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public DataTable LoadDataTableByFilter(string filter,string sort)
        {
            DataTable dt = null;
            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            dt = PgsqlDataHelper.GetDataTable(con, filter, sort, this.TableName);
            return dt;
        }


        public void ExecuteStoredProcedure(string storedProcedureName)
        {
            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            try
            {
                string strCmd = storedProcedureName;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;
                //Seconds
                cmd.CommandTimeout = 6*3600;

                con.Open();
                cmd.ExecuteNonQuery();
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
        }

        public DataTable ExecuteDynamicSelect(string queryString)
        {
            DataTable dt = new DataTable();

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                NpgsqlTransaction t = (NpgsqlTransaction) con.BeginTransaction();

                string strCmd = "execute_dynamic_select";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("query_string", DbType.String, queryString));
            
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);

                t.Commit();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return dt;
        }

       
    }
}
