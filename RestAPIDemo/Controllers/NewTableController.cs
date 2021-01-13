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
    public class NewTableController : ApiController
    {
        // GET: api/NewTable?token=
        public string Get(string token,int tableID, int personalNum)
        {//token,tableid=>retrurn orderid
            return NewTable(token, tableID, personalNum);
        }


        // POST: api/NewTable
        public string Post([FromBody]string token, [FromBody]int tableID, [FromBody]int personalNum)
        {
            return NewTable(token, tableID, personalNum);
        }

        private static string NewTable(string token, int tableID, int personalNum)
        {
            SecurityFunction sf = new SecurityFunction();
            if (sf.checkToken(token))   //login success
            {
                //ckecktable,if no,create new orderid, else return message.
                SQL sql = new SQL();
                SqlCommand cmd = new SqlCommand("SELECT * from dbo.OrderTbl WHERE tableID = @tableID and isPay = @isPay");
                cmd.Parameters.AddWithValue("@tableID", tableID);
                cmd.Parameters.AddWithValue("@isPay", false);   //table exist and not pay
                DataTable dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count == 1) //table exist and table is is pay, new order and if created.
                {
                    return "Table in use.";
                }
                else
                {
                    //Get staffID
                    cmd = new SqlCommand("SELECT id from dbo.StaffTbl WHERE token = @token");
                    cmd.Parameters.AddWithValue("@token", token);
                    dt = sql.executeSQLQuery(cmd);
                    int staffID = 1;
                    if (token == "ABCDEFG")
                    {
                        staffID = 1;    ////backdoor for test
                    }
                    else
                    {
                        staffID = int.Parse(dt.Rows[0][0].ToString());
                    }

                    //insert a record to table
                    cmd = new SqlCommand("INSERT into OrderTbl(staffID,tableID,personalNum) Values (@staffID,@tableID,@personalNum)");
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.Parameters.AddWithValue("@tableID", tableID);
                    cmd.Parameters.AddWithValue("@personalNum", personalNum);
                    sql.executeSQL(cmd);

                    //update table state
                    cmd = new SqlCommand("UPDATE dbo.TableTbl SET flag=1 where id=@tableID");
                    cmd.Parameters.AddWithValue("@tableID", tableID);
                    sql.executeSQL(cmd);

                    //get table id
                    cmd = new SqlCommand("SELECT id from OrderTbl WHERE staffID=@staffID and tableID=@tableID order by id desc");//just need top id
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.Parameters.AddWithValue("@tableID", tableID);
                    dt = sql.executeSQLQuery(cmd);
                    return dt.Rows[0][0].ToString();
                }
            }
            return "Login Session Expired.";
        }

    }
}
