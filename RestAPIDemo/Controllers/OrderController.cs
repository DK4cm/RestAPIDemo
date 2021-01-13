using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;

namespace comp5527.Controllers
{
    public class OrderController : ApiController
    {
        // GET: api/Order
        public string Get(string token, int tableID,int menuID,int num)
        {
            return OrderItem(token, tableID, menuID, num);
        }

        // POST: api/Order
        public string Post([FromBody]string token, [FromBody]int tableID, [FromBody]int menuID, [FromBody]int num)
        {
            return OrderItem(token, tableID, menuID, num);
        }

        private static string OrderItem(string token, int tableID, int menuID, int num)
        {
            SecurityFunction sf = new SecurityFunction();
            if (sf.checkToken(token))   //login success
            {
                SQL sql = new SQL();
                //Check if table is open
                SqlCommand cmd = new SqlCommand("SELECT id from dbo.TableTbl where id=@tableid and flag=1");
                cmd.Parameters.AddWithValue("@tableid", tableID);
                DataTable dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count <= 0)//not open
                {
                    return "Table not Open";
                }

                //get  orderid from tableid
                cmd = new SqlCommand("SELECT id FROM dbo.OrderTbl where tableID=@tableID order by id desc");
                cmd.Parameters.AddWithValue("@tableID", tableID);
                dt = sql.executeSQLQuery(cmd);
                int orderID = int.Parse(dt.Rows[0][0].ToString());

                //get menu name/unit price from menuID
                cmd = new SqlCommand("SELECT name,price FROM dbo.MenuTbl where id=@menuID");
                cmd.Parameters.AddWithValue("@menuID", menuID);
                dt = sql.executeSQLQuery(cmd);
                if (dt.Rows.Count <= 0)
                {
                    return "Menu not exists";
                }
                string itemName = dt.Rows[0]["name"].ToString();
                float unitPrice = float.Parse(dt.Rows[0]["price"].ToString());

                //calculate total price
                float totalPrice = unitPrice * num;

                //update the orderdetail Table
                cmd = new SqlCommand("INSERT into dbo.OrderDetail(orderID,menuID,num,unitPrice,totalPrice,itemName) VALUES (@orderID,@menuID,@num,@unitPrice,@totalPrice,@itemName)");
                cmd.Parameters.AddWithValue("@orderID", orderID);
                cmd.Parameters.AddWithValue("@menuID", menuID);
                cmd.Parameters.AddWithValue("@num", num);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@totalPrice", totalPrice);
                cmd.Parameters.AddWithValue("@itemName", itemName);
                sql.executeSQL(cmd);

                return "true";
            }
            return "Login Session Expired.";
        }

    }
}
