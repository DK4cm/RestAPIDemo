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
    public struct OrderDeatil
    {
        public int id;
        public int orderID;
        public int menuID;
        public int num;
        public string remark;
        public string unitPrice;
        public string totalPrice;
        public bool valid;
        public string itemName;
        public string pic;
    }

    public class OrderDetailController : ApiController
    {
        // GET: api/OrderDetail
        public int[] Get()
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT orderID from dbo.OrderDetail");
            DataTable dt = sql.executeSQLQuery(cmd);
            int[] idList = new int[dt.Rows.Count];

            int tmp = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idList[tmp] = int.Parse(dr["orderID"].ToString());
                tmp++;
            }
            return idList;
        }
        
        // GET: api/OrderDetail/5
        public OrderDeatil[] Get(int id)
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT * from dbo.OrderDetail a left join menuTbl b on a.menuID = b.id WHERE orderID=@id");
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = sql.executeSQLQuery(cmd);
            OrderDeatil[] detail = new OrderDeatil[dt.Rows.Count];
            int tmp = 0;
            foreach (DataRow dr in dt.Rows)
            {
                detail[tmp].id = int.Parse(dr["id"].ToString());
                detail[tmp].orderID = int.Parse(dr["orderID"].ToString());
                detail[tmp].menuID = int.Parse(dr["menuID"].ToString());
                detail[tmp].num = int.Parse(dr["num"].ToString());
                detail[tmp].remark = dr["remark"].ToString();
                detail[tmp].unitPrice = dr["unitPrice"].ToString();
                detail[tmp].totalPrice = dr["totalPrice"].ToString();
                detail[tmp].valid = bool.Parse(dr["valid"].ToString());
                detail[tmp].itemName = dr["itemName"].ToString();
                detail[tmp].pic = dr["pic"].ToString();
                tmp++;
            }
            return detail;
        }

        // POST: api/OrderDetail
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OrderDetail/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OrderDetail/5
        public void Delete(int id)
        {
        }
    }
}
