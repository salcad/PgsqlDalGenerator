



/*
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
       
		//INPUT
		            // i 	 p_order_by  character varying  string
				
		//OUTPUT
		  	
	
*/