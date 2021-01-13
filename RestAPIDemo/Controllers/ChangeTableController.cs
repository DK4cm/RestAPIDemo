using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RestAPIDemo.Controllers
{
    public class ChangeTableController : ApiController
    {
        // GET: api/ChangeTable
        public string Get(string token, int fromTableID, int toTableID)
        {
            return ChangeTable(token, fromTableID, toTableID);
        }


        // POST: api/ChangeTable
        public string Post([FromBody]string token, [FromBody]int fromTableID, [FromBody]int toTableID)
        {
            return ChangeTable(token, fromTableID, toTableID);
        }

        private static string ChangeTable(string token, int fromTableID, int toTableID)
        {
            SecurityFunction sf = new SecurityFunction();
            if (sf.checkToken(token))   //login success
            {
                //check if destination table is not in use
                SQL sql = new SQL();
                SqlCommand cmd = new SqlCommand("select * from dbo.TableTbl where id=@tableID");
                cmd.Parameters.AddWithValue("@tableID", toTableID);
                DataTable dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count == 0) //no data
                {
                    return "Destination table does not exists";
                }
                DataRow dr = dt.Rows[0];
                if (bool.Parse(dr["flag"].ToString()))  //true, has client inside
                {
                    return "Destination table in use";
                }

                //update current table to empty
                cmd = new SqlCommand("update dbo.TableTbl set flag=0 where id=@tableID");
                cmd.Parameters.AddWithValue("@tableID", fromTableID);
                sql.executeSQL(cmd);
                //update destination table to in use
                cmd = new SqlCommand("update dbo.TableTbl set flag=1 where id=@tableID");
                cmd.Parameters.AddWithValue("@tableID", toTableID);
                sql.executeSQL(cmd);
                //get Orderid from fromTableID 
                cmd = new SqlCommand("SELECT id FROM dbo.OrderTbl where tableID=@tableID order by id desc");
                cmd.Parameters.AddWithValue("@tableID", fromTableID);
                dt = sql.executeSQLQuery(cmd);
                int orderid = int.Parse(dt.Rows[0][0].ToString());
                //update tableid in orderTbl
                cmd = new SqlCommand("update dbo.OrderTbl set tableID=@tableID where id=@id");
                cmd.Parameters.AddWithValue("@tableID", toTableID);
                cmd.Parameters.AddWithValue("@id", orderid);
                sql.executeSQL(cmd);
                return "true";
            }
            return "Login Session Expired.";
        }

    }
}
