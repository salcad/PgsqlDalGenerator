



/*   
using System;
using System.Data;
using Npgsql;
using Extend.Lib.Npgsql;

namespace Dal.Sample
{

    public class TestCompletesModel
    {

        #region < Members >
		
        long _TestCompletePK;
		string _TestVarchar;
		int _TestSmallint;
		bool _TestBoolean;
		DateTime _TestTimestamp;
		int _TestInteger;
		long _TestBigint;
		int? _TestListItemFK;
		decimal _TestNumeric;
		DateTime _TestDate;
		string _TestCompleteID;
		        
		#endregion

        #region < properties >
       
	   	public long TestCompletePK
		{
			get { return _TestCompletePK; }
			set { _TestCompletePK = value; } 
		}
		public string TestVarchar
		{
			get { return _TestVarchar; }
			set { _TestVarchar = value; } 
		}
		public int TestSmallint
		{
			get { return _TestSmallint; }
			set { _TestSmallint = value; } 
		}
		public bool TestBoolean
		{
			get { return _TestBoolean; }
			set { _TestBoolean = value; } 
		}
		public DateTime TestTimestamp
		{
			get { return _TestTimestamp; }
			set { _TestTimestamp = value; } 
		}
		public int TestInteger
		{
			get { return _TestInteger; }
			set { _TestInteger = value; } 
		}
		public long TestBigint
		{
			get { return _TestBigint; }
			set { _TestBigint = value; } 
		}
		public int? TestListItemFK
		{
			get { return _TestListItemFK; }
			set { _TestListItemFK = value; } 
		}
		public decimal TestNumeric
		{
			get { return _TestNumeric; }
			set { _TestNumeric = value; } 
		}
		public DateTime TestDate
		{
			get { return _TestDate; }
			set { _TestDate = value; } 
		}
		public string TestCompleteID
		{
			get { return _TestCompleteID; }
			set { _TestCompleteID = value; } 
		}
			   
        #endregion

    }


    public class TestCompletesDal : PgsqlBusinessBase
    {
       
        public TestCompletesDal(string connectionStringName): base("TestCompletes")
        {
            this.ConnectionStringName = connectionStringName;
        }
     
        public  int Insert(
			string testVarchar,
			int testSmallint,
			bool testBoolean,
			DateTime testTimestamp,
			int testInteger,
			long testBigint,
			int? testListItemFK,
			decimal testNumeric,
			DateTime testDate,
			string testCompleteID)
					
		{

            int identity = -1;
            
            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                string strCmd = "test_completes_insert";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;
				
                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_varchar", DbType.String, testVarchar));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_smallint", DbType.Int16, testSmallint));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_boolean", DbType.Boolean, testBoolean));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_timestamp", DbType.DateTime, testTimestamp));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_integer", DbType.Int32, testInteger));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_bigint", DbType.Int64, testBigint));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_list_item_fk", DbType.Int32, testListItemFK));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_numeric", DbType.Decimal, testNumeric));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_date", DbType.Date, testDate));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_complete_id", DbType.String, testCompleteID));
							    
                identity = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return identity;
        }

        public  int UpdateByPK(
			long testCompletePK,
			string testVarchar,
			int testSmallint,
			bool testBoolean,
			DateTime testTimestamp,
			int testInteger,
			long testBigint,
			int? testListItemFK,
			decimal testNumeric,
			DateTime testDate,
			string testCompleteID)
					
        {
            int rowsAffected = 0;

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                string strCmd = "test_completes_update_by_pk";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_complete_pk", DbType.Int64, testCompletePK));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_varchar", DbType.String, testVarchar));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_smallint", DbType.Int16, testSmallint));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_boolean", DbType.Boolean, testBoolean));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_timestamp", DbType.DateTime, testTimestamp));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_integer", DbType.Int32, testInteger));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_bigint", DbType.Int64, testBigint));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_list_item_fk", DbType.Int32, testListItemFK));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_numeric", DbType.Decimal, testNumeric));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_date", DbType.Date, testDate));
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_complete_id", DbType.String, testCompleteID));
				
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
        
        public  int DeleteByPK(
		        long testCompletePK)
        {
            int rowsAffected = 0;

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                string strCmd = "test_completes_delete_by_pk";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;
                
				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_complete_pk", DbType.Int64, testCompletePK));
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


        public DataTable GetByID(
			   string testCompleteID, MatchOptions matchOptions)
        {
            DataTable dt = new DataTable();

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                NpgsqlTransaction t = con.BeginTransaction();

                string strCmd = "test_completes_get_by_id";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_testCompleteID", DbType.String, testCompleteID));
                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_match_options", DbType.String, matchOptions.ToString()));

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

        public DataTable GetByPK(
		       long testCompletePK)
        {
            DataTable dt = new DataTable();

            NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionStringName);
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = con;

            try
            {
                con.Open();

                NpgsqlTransaction t = con.BeginTransaction();

                string strCmd = "test_completes_get_by_pk";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = strCmd;

                cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_test_complete_pk", DbType.Int64, testCompletePK));
             
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


        public bool GetRowByPK(
		       long testCompletePK, 
			   ref TestCompletesModel model, 
			   bool isConvertNull)
        {
           
            DataTable dt = this.GetByPK(testCompletePK);
            if (dt == null || dt.Rows.Count == 0) return false;

            DataRow row = dt.Rows[0];
            GetRow(row, ref model, isConvertNull);

            return true;
        }

        public bool GetRowByID(
		       string testCompleteID, 
			   ref TestCompletesModel model, 
			   bool isConvertNull)
        {
           
            DataTable dt = this.GetByID(testCompleteID, MatchOptions.Exact);
            if (dt == null || dt.Rows.Count == 0) return false;
                
            DataRow row = dt.Rows[0];
            GetRow(row, ref model, isConvertNull);

            return true;
        }

        public void GetRow(DataRow row, ref TestCompletesModel model, bool isconvertnull)
        {
		    
			model.TestCompletePK = Convert.ToInt64(row["test_complete_pk"]);
			if (isconvertnull) model.TestVarchar = row.IsNull("test_varchar") ? null : Convert.ToString(row["test_varchar"]);
			else model.TestVarchar = row.IsNull("test_varchar") ? string.Empty : Convert.ToString(row["test_varchar"]);
			model.TestSmallint = Convert.ToInt16(row["test_smallint"]);
			model.TestBoolean = Convert.ToBoolean(row["test_boolean"]);
			model.TestTimestamp = Convert.ToDateTime(row["test_timestamp"]);
			model.TestInteger = Convert.ToInt32(row["test_integer"]);
			model.TestBigint = Convert.ToInt64(row["test_bigint"]);
			if (isconvertnull) model.TestListItemFK = row.IsNull("test_list_item_fk") ? null : (int?)Convert.ToInt32(row["test_list_item_fk"]);
			else model.TestListItemFK = row.IsNull("test_list_item_fk") ? -1 : (int?)Convert.ToInt32(row["test_list_item_fk"]);
			model.TestNumeric = Convert.ToDecimal(row["test_numeric"]);
			model.TestDate = Convert.ToDate(row["test_date"]);
			model.TestCompleteID = Convert.ToString(row["test_complete_id"]);
						
		}
		
		
        public DataTable TestCompletesGetAll(
		string orderBy)
        {
		     DataTable dt = new DataTable();
 
			 NpgsqlConnection con = PgsqlDataHelper.GetConnection(this.ConnectionString);
			 NpgsqlCommand cmd = new NpgsqlCommand();
			 cmd.Connection = con;

			try
			{
				con.Open();
				NpgsqlTransaction t = con.BeginTransaction();
				string strCmd = "test_completes_get_all";
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.CommandText = strCmd;

				cmd.Parameters.Add(PgsqlDataHelper.CreateInputParam("p_order_by",DbType.String,orderBy));

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
   
*/