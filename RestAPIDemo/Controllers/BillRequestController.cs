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
    public class BillRequestController : ApiController
    {
        // GET: api/BillRequest
        public string Get(string token,int tableID)
        {
            return BillRequest(token, tableID);
        }


        // POST: api/BillRequest
        public string Post([FromBody]string token, [FromBody]int tableID)
        {
            return BillRequest(token, tableID);
        }

        private static string BillRequest(string token, int tableID)
        {
            SecurityFunction sf = new SecurityFunction();
            if (sf.checkToken(token))   //login success
            {
                //get orderid first
                SQL sql = new SQL();
                SqlCommand cmd = new SqlCommand("SELECT id FROM dbo.OrderTbl where tableID=@tableID order by id desc");
                cmd.Parameters.AddWithValue("@tableID", tableID);
                DataTable dt = sql.executeSQLQuery(cmd);
                int orderid = int.Parse(dt.Rows[0][0].ToString());
                //update the billrequest field to true
                cmd = new SqlCommand("UPDATE OrderTbl set billRequested = 1 where id=@orderID");
                cmd.Parameters.AddWithValue("@orderID", orderid);
                sql.executeSQL(cmd);
                

                ////set pay=1,bill request=0
                //cmd = new SqlCommand("UPDATE ordertbl SET isPay=1, billRequested=0 WHERE id=@orderid");
                //cmd.Parameters.AddWithValue("@orderid", orderid);
                //sql.executeSQL(cmd);
                ////set table state = 0 
                //cmd = new SqlCommand("UPDATE TableTbl SET flag=0 WHERE id=@tableID");
                //cmd.Parameters.AddWithValue("@tableID", tableID);
                //sql.executeSQL(cmd);

                return "true";
            }
            return "Login Session Expired.";
        }
    }
}
