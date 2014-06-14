using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Extend.Lib.Npgsql;
using Npgsql;


namespace PgsqlMetadataExtract
{
    public class ExtractFunctionArgs : PgsqlBusinessBase
    {

        public ExtractFunctionArgs(string connectionString)
            : base("PgsqlGetMetadata")
        {
            this.ConnectionString = connectionString;
        }


        public List<FuncArgItem> GetAll(
            string funcname, string schema)
        {
            List<FuncArgItem> funcArgsList = new List<FuncArgItem>();
            DataTable dT = null;

            NpgsqlConnection con = new NpgsqlConnection(ConnectionString);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;
            try
            {
                string strCmd = "meta_extract_function_args";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_funcname", DbType.String, funcname));
                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_schema", DbType.String, schema));

                con.Open();
                NpgsqlTransaction t = con.BeginTransaction();

                NpgsqlDataReader reader = cmd.ExecuteReader();

                dT = new DataTable("FunctionArgs");
                dT.Load(reader);

                foreach (DataRow row in dT.Rows)
                {
                    var item = new FuncArgItem();
                    item.Direction = row["direction"].ToString().Trim();
                    item.ArgName = row["argname"].ToString().Trim();
                    item.PgDataType = row["datatype"].ToString().Trim();
                    item.CsDataType = Pg2Cs(item.PgDataType).Trim();
                    item.DbType = Pg2DbType(item.PgDataType).Trim(); 
                    funcArgsList.Add(item);
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

            return funcArgsList;
        }

        public List<FuncArgItem> GetAllMinusReturnValue(List<FuncArgItem> funcArgList)
        {
            List<FuncArgItem> tmpList = new List<FuncArgItem>(); 
            foreach (FuncArgItem item in funcArgList)
            {
                if (item.ArgName.IndexOf("RETURN VALUE") == -1)
                {
                    tmpList.Add(item);
                }
            }
            return tmpList;
        }

        public List<FuncArgItem> GetAllForInputDirection(List<FuncArgItem> funcArgList)
        {
            List<FuncArgItem> tmpList = new List<FuncArgItem>();
            foreach (FuncArgItem item in funcArgList)
            {
                if (item.Direction=="i")
                {
                    tmpList.Add(item);
                }
            }
            return tmpList;
        }

        public List<FuncArgItem> GetAllForOutputDirection(List<FuncArgItem> funcArgList)
        {
            List<FuncArgItem> tmpList = new List<FuncArgItem>();
            foreach (FuncArgItem item in funcArgList)
            {
                if (item.Direction=="o")
                {
                    tmpList.Add(item);
                }
            }
            return tmpList;
        }

        public string GetCsDataTypeForReturnValue(List<FuncArgItem> funcArgList)
        {
            foreach (FuncArgItem item in funcArgList)
            {
                if (item.ArgName.IndexOf("RETURN VALUE")>=0)
                {
                    return item.CsDataType;
                }
			}

            return "void";

        }

       
        private string Pg2Cs(string pgDataType)
        {
            if (pgDataType.IndexOf("integer") >= 0)
            {
                return "int";
            }
            else if (pgDataType.IndexOf("bigint") >= 0)
            {
                return "long";
            }
            else if (pgDataType.IndexOf("boolean") >= 0)
            {
                return "bool";
            }
            else if (pgDataType.IndexOf("numeric") >= 0)
            {
                return "decimal";
            }
            else if (pgDataType.IndexOf("character") >= 0)
            {
                return "string";
            }
            else if (pgDataType.IndexOf("timestamp") >= 0)
            {
                return "DateTime";
            }
            else if (pgDataType.IndexOf("date") >= 0)
            {
                return "DateTime";
            }
            else if (pgDataType.IndexOf("setof") >= 0)
            {
                return "DataTable";
            }
            else if (pgDataType.IndexOf("refcursor") >= 0)
            {
                return "DataTable";
            } 
            else
            {
                return "unknown";
            }
        
        }

        private string Pg2DbType(string pgDataType)
        {
            if (pgDataType.IndexOf("integer") >= 0)
            {
                return "Int32";
            }
            else if (pgDataType.IndexOf("bigint") >= 0)
            {
                return "Int64";
            }
            else if (pgDataType.IndexOf("boolean") >= 0)
            {
                return "Boolean";
            }
            else if (pgDataType.IndexOf("numeric") >= 0)
            {
                return "Decimal";
            }
            else if (pgDataType.IndexOf("character") >= 0)
            {
                return "String";
            }
            else if (pgDataType.IndexOf("timestamp") >= 0)
            {
                return "DateTime";
            }
            else if (pgDataType.IndexOf("date") >= 0)
            {
                return "Date";
            }
            else if (pgDataType.IndexOf("setof") >= 0)
            {
                return "DataTable";
            }
            else if (pgDataType.IndexOf("refcursor") >= 0)
            {
                return "DataTable";
            }
            else
            {
                return "unknown";
            }

        }

       

    }

    public class FuncArgItem
    {
        private string _Direction;
        private string _ArgName;
        private string _PgDataType;
        private string _CsDataType;
        private string _DbType;
        private string _ReturnValueCsDataType;

        public string ArgName
        {
            get { return _ArgName; }
            set { _ArgName = value; }
        }

        public string Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        public string PgDataType
        {
            get { return _PgDataType; }
            set { _PgDataType = value; }
        }

        public string CsDataType
        {
            get { return _CsDataType; }
            set { _CsDataType = value; }
        }

        public string ReturnValueCsDataType
        {
            get { return _ReturnValueCsDataType; }
            set { _ReturnValueCsDataType = value; }
        }

        public string DbType
        {
            get { return _DbType; }
            set { _DbType = value; }
        }


    }
}
