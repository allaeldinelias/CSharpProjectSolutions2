using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Helpers;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace www
{
    public class Archetype
    {
        private string sNewInspectionObject;

        public Archetype(string sNewInspectionObject)
        {
            // TODO: Complete member initialization
            this.sNewInspectionObject = sNewInspectionObject;
        }

        public void save()
        {
            var oInput = Json.Decode(this.sNewInspectionObject);
            // need to make sure name is a valid identifier to prevent SQL injection and to generate SQL that works
            String sName = oInput.name;
            if (!SourceVersion.isName(sName))
                throw new Exception("name " + sName + " is not a valid identifier");
            SqlConnection oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["MyDatabase"].ConnectionString);
            oConn.Open();
            SqlTransaction oTransaction = oConn.BeginTransaction();
            try
            {
                SqlCommand oCmd = new SqlCommand("INSERT INTO DispClass(name) VALUES(@sName)", oConn, oTransaction);
                oCmd.Parameters.AddWithValue("@sName", sName);
                int nRows = oCmd.ExecuteNonQuery();
                if (nRows != 1) throw new Exception("0 dispclass rows inserted");
                oCmd = new SqlCommand("SELECT @@Identity", oConn, oTransaction);
                var nId = oCmd.ExecuteScalar();
			    // build the SQL for the new table
                oCmd = new SqlCommand("INSERT INTO DispAttribute(idDispClass, name, SQLType, formType) VALUES(@nId, @sName, @sSQLType, @sFormType)",
                    oConn, oTransaction);
			    String sSQL = "CREATE TABLE ";
                sSQL += sName + "(\nid" + sName + "  INT IDENTITY NOT NULL PRIMARY KEY";
                foreach (var oItem in oInput.archetypeAttributes)
                {
                    String sAttributeName = (String)oItem.name;
                    	        	// make sure name is a valid identifier
	        	    if(!SourceVersion.isName(sAttributeName))
	    			    throw new Exception("name " + sAttributeName + " is not a valid java identifier");

	        	    // make sure that type is valid SQL type
                    String sSQLType = ((String)oItem.SQLType).ToUpper();
                    if (sSQLType != "INT" &&
                        sSQLType != "DATETIME" &&
                        1 != Regex.Matches(sSQLType, "NVARCHAR[(][0-9]{1,5}[)]").Count)
                        throw new Exception("sqlType " + sSQLType + " is not a valid SQLType");
                    sSQL += ",\n" + sAttributeName + " " + sSQLType;
                    oCmd.Parameters.AddWithValue("@nId", nId);
                    oCmd.Parameters.AddWithValue("@sName", sAttributeName);
                    oCmd.Parameters.AddWithValue("@sSQLType", sSQLType);
                    oCmd.Parameters.AddWithValue("@sFormType", oItem.formType);
                    nRows = oCmd.ExecuteNonQuery();
                    oCmd.Parameters.Clear();
                    if (nRows != 1) throw new Exception("0 dispattribute rows inserted");
                }
                sSQL += ")";
                oCmd = new SqlCommand(sSQL, oConn, oTransaction);
                oCmd.ExecuteNonQuery();
                oTransaction.Commit();
            }
            catch (Exception e)
            {
                oTransaction.Rollback();
                throw e;
            }
            finally
            {
                oConn.Close();
            }
        }
    }
}
