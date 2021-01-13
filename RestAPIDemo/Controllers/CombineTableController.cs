using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace comp5527.Controllers
{
    public class CombineTableController : ApiController
    {
        // GET: api/CombineTable
        public string Get(string token, int fromTableID, int toTableID)
        {
            return CombineOrder(token, fromTableID, toTableID);
        }

        // POST: api/CombineTable
        public string Post([FromBody]string token, [FromBody]int fromTableID, [FromBody]int toTableID)
        {
            return CombineOrder(token, fromTableID, toTableID);
        }

        private static string CombineOrder(string token, int fromTableID, int toTableID)
        {
            SecurityFunction sf = new SecurityFunction();
            if (sf.checkToken(token))   //login success
            {
                SQL sql = new SQL();
                //check from table open
                SqlCommand cmd = new SqlCommand("select * from dbo.TableTbl where id=@tableID and flag=1");
                cmd.Parameters.AddWithValue("@tableID", fromTableID);
                DataTable dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count == 0)
                {
                    return "fromTable not open";
                }
                //check to table open
                cmd = new SqlCommand("select * from dbo.TableTbl where id=@tableID and flag=1");
                cmd.Parameters.AddWithValue("@tableID", toTableID);
                dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count == 0)
                {
                    return "toTable not open";
                }
                //update from table to not use
                cmd = new SqlCommand("update dbo.TableTbl set flag=0 where id=@tableID");
                cmd.Parameters.AddWithValue("@tableID", fromTableID);
                sql.executeSQL(cmd);
                //get orderid of from table
                cmd = new SqlCommand("SELECT id,personalNum FROM dbo.OrderTbl where tableID=@tableID order by id desc");
                cmd.Parameters.AddWithValue("@tableID", fromTableID);
                dt = sql.executeSQLQuery(cmd);
                int fromTableOrderID = int.Parse(dt.Rows[0][0].ToString());
                int fromPersonalNum = int.Parse(dt.Rows[0][1].ToString());
                //get orderid of to table
                cmd = new SqlCommand("SELECT id FROM dbo.OrderTbl where tableID=@tableID order by id desc");
                cmd.Parameters.AddWithValue("@tableID", toTableID);
                dt = sql.executeSQLQuery(cmd);
                int toTableOrderID = int.Parse(dt.Rows[0][0].ToString());

                //update OrderTbl from table isPay=true
                cmd = new SqlCommand("UPDATE OrderTbl SET isPay = 1,personalNum=0 WHERE id=@orderID");
                cmd.Parameters.AddWithValue("@orderID", fromTableOrderID);
                sql.executeSQL(cmd);
                //update OrderTbl to table's personalNum  
                cmd = new SqlCommand("UPDATE OrderTbl SET personalNum = personalNum + @fromPersonalNum WHERE id=@toOrderID");
                cmd.Parameters.AddWithValue("@toOrderID", toTableOrderID);
                cmd.Parameters.AddWithValue("@fromPersonalNum", fromPersonalNum);
                sql.executeSQL(cmd);

                //update OrderDetail orderID to toTable
                cmd = new SqlCommand("UPDATE OrderDetail SET orderID = @toID WHERE orderID = @fromID");
                cmd.Parameters.AddWithValue("@fromID", fromTableOrderID);
                cmd.Parameters.AddWithValue("@toID", toTableOrderID);
                cmd.Parameters.AddWithValue("@fromPersonalNum", fromPersonalNum);
                sql.executeSQL(cmd);
                return "true";
            }
            return "Login Session Expired.";
        }
        
    }
}
