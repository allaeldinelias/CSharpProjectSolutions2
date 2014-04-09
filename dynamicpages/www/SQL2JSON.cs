using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace www
{
    public class SQL2JSON
    {
        public static String toJSON(String sSQL, string[] aIn = null)
        {
            String sRc = "[";
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString);
            oConn.Open();
            SqlCommand oCmd = new SqlCommand(sSQL, oConn);
            if(aIn != null) for (int n = 0; n < aIn.Length; n++)
            {
                oCmd.Parameters.AddWithValue("@col" + n, aIn[n]);
            }
            SqlDataReader oReader = oCmd.ExecuteReader();
            int nRow = 0;
            while (oReader.Read())
            {
                if (nRow++ != 0)
                {
                    sRc += ", ";
                }
                sRc += "{";
                for (int n = 0; n < oReader.FieldCount; n++)
                {
                    if (n != 0)
                    {
                        sRc += ", ";
                    }
                    sRc += "\"" + oReader.GetName(n) + "\":";
                    sRc += "\"" + oReader[n] + "\"";
                }
                sRc += "}";
            }
            sRc += "]";
            return sRc;
        }
    }
}