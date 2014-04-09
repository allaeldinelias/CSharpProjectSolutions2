using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Helpers;

namespace www
{
    public class InspectionObject
    {
        private string sNewInstance;

        public InspectionObject(string sNewInstance)
        {
            this.sNewInstance = sNewInstance;
        }


        public void save()
        {
            var oInput = Json.Decode(this.sNewInstance);
            // need to make sure name is a valid identifier to prevent SQL injection and to generate SQL that works
            String sName = oInput.archetype;
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString);
            oConn.Open();
            try
            {
                //get idDispClass
                SqlCommand oCmd = new SqlCommand("SELECT idDispClass FROM DispClass WHERE name = @sName", 
                    oConn);
                oCmd.Parameters.AddWithValue("@sName", sName);
                var nId = oCmd.ExecuteScalar();

                //get column names
                oCmd = new SqlCommand("SELECT name FROM DispAttribute where idDispClass = @nId", 
                    oConn);
                oCmd.Parameters.AddWithValue("@nId", nId);
                SqlDataReader oReader = oCmd.ExecuteReader();
                int nColNo = 0;
                String sSQL = "INSERT INTO " + sName + "(";
                String sValues = ") VALUES(";
                List<String> aBindVars = new List<String>();
                while (oReader.Read())
                {
                    String sColName = (String)oReader[0];
                    if (nColNo++ > 0)
                    {
                        sSQL += ", ";
                        sValues += ", ";
                    }
                    sSQL += sColName;
                    sValues += "@" + sColName;
                    aBindVars.Add(sColName);
                }
                oReader.Close();
                sSQL += sValues + ")";
                oCmd = new SqlCommand(sSQL, oConn);
                foreach (String sColName in aBindVars)
                {
                    oCmd.Parameters.AddWithValue("@" + sColName, oInput[sColName]);
                }
                int nRows = oCmd.ExecuteNonQuery();
                if (nRows != 1) throw new Exception(nRows + " objects inserted expected 1");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine(e.StackTrace);
                throw e;
            }
        }
    }
}
