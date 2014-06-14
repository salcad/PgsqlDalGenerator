using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PgsqlMetadataExtract;

namespace PgsqlDalGenerator
{
    class Class1
    {
         Class1()
         {
             string tableName = "test_completes";
             string connectionString =
                 "Server=192.168.8.88;Port=5432;User Id=postgres;Password=1234;Database=postgres;";

             ExtractTableMetadatas metadatas = new ExtractTableMetadatas(connectionString, tableName);

             foreach (TableField item in metadatas.Table.TableFields)
             {
                 //item.ColumnName;
                 //item.DataType;
                 //item.IsNullable;
                if (metadatas.Table.PrimaryKey.Trim().ToLower() != item.ColumnName.Trim().ToLower())
                {
                    
                }
             }
         }
    }
}
