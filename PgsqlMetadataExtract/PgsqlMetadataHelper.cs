using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PgsqlMetadataExtract
{
    public class PgsqlMetadataHelper
    {
        
        public static string Pg2Cs(string pgDataType)
        {
           
            if (pgDataType.IndexOf("smallint") >= 0)
            {
                return "int";
            }
            else if (pgDataType.IndexOf("integer") >= 0)
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
            else if (pgDataType.IndexOf("setof record") >= 0)
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


        public static string Cs2Pg(string csDataType)
        {

            if (csDataType.IndexOf("int") >= 0)
            {
                return "integer";
            }
            else if (csDataType.IndexOf("long") >= 0)
            {
                return "bigint";
            }
            else if (csDataType.IndexOf("bool") >= 0)
            {
                return "boolean"; 
            }
            else if (csDataType.IndexOf("decimal") >= 0)
            {
                return "numeric";
            }
            else if (csDataType.IndexOf("string") >= 0)
            {
                return "varchar";
            }
            else if (csDataType.IndexOf("DateTime") >= 0)
            {
                return "timestamp";
            }
            else
            {
                return "unknown";
            }

        }


        public static string LongPg2ShortPg(string pgDataType)
        {

            if (pgDataType.IndexOf("numeric") >= 0)
            {
                return "numeric";
            }
            else if (pgDataType.IndexOf("character") >= 0)
            {
                return "varchar";
            }
            else if (pgDataType.IndexOf("timestamp") >= 0)
            {
                return "timestamp";
            }
            else
            {
                return pgDataType;
            }

        }



        public static string Pg2NullableCs(string pgDataType, int mode)
        {
            if (pgDataType.IndexOf("smallint") >= 0)
            {
                if (mode == 1)
                {
                    return "(int?)";
                }
                else
                {
                    return "int?";
                }
                
            }
            else if (pgDataType.IndexOf("integer") >= 0)
            {
                if (mode == 1)
                {
                    return "(int?)";
                }
                else
                {
                    return "int?";
                }
            }
            else if (pgDataType.IndexOf("bigint") >= 0)
            {
                if (mode == 1)
                {
                    return "(long?)";
                }
                else
                {
                    return "long?";
                }
            }
            else if (pgDataType.IndexOf("boolean") >= 0)
            {
                if (mode == 1)
                {
                    return "(bool?)";
                }
                else
                {
                    return "bool?";
                }
            }
            else if (pgDataType.IndexOf("numeric") >= 0)
            {
                if (mode == 1)
                {
                    return "(decimal?)";
                }
                else
                {
                    return "decimal?";
                }
            }
            else if (pgDataType.IndexOf("character") >= 0)
            {
                if (mode==1)
                {
                    return "";
                }
                else
                {
                    return "string";
                }
            }
            else if (pgDataType.IndexOf("timestamp") >= 0)
            {
                if (mode == 1)
                {
                    return "(DateTime?)";
                }
                else
                {
                    return "DateTime?";
                }
            }
            else if (pgDataType.IndexOf("date") >= 0)
            {
                if (mode == 1)
                {
                    return "(DateTime?)";
                }
                else
                {
                    return "DateTime?";
                }
            }
            else if (pgDataType.IndexOf("setof record") >= 0)
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

        public static string Pg2DefaultValueCs(string pgDataType)
        {
            if (pgDataType.IndexOf("smallint") >= 0)
            {
                return "-1";
            }
            else if (pgDataType.IndexOf("integer") >= 0)
            {
                return "-1";
            }
            else if (pgDataType.IndexOf("bigint") >= 0)
            {
                return "-1";
            }
            else if (pgDataType.IndexOf("boolean") >= 0)
            {
                return "false";
            }
            else if (pgDataType.IndexOf("numeric") >= 0)
            {
                return "0.0M";
            }
            else if (pgDataType.IndexOf("character") >= 0)
            {
                return "string.Empty";
            }
            else if (pgDataType.IndexOf("timestamp") >= 0)
            {
                return "DateTime.MinValue";
            }
            else if (pgDataType.IndexOf("date") >= 0)
            {
                return "DateTime.MinValue";
            }
            else if (pgDataType.IndexOf("setof record") >= 0)
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


        public static string Pg2DbType(string pgDataType)
        {
            
            if (pgDataType.IndexOf("smallint") >= 0)
            {
                return "Int16";
            }
            else if (pgDataType.IndexOf("integer") >= 0)
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
            else if (pgDataType.IndexOf("setof record") >= 0)
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

}
