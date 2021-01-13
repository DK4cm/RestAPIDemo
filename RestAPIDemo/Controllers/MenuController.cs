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
    public class MenuController : ApiController
    {
        public struct menu
        {
            public int typeID;
            public string name;
            public string price;
            public string pic;
            public string remark;
        } 
        // GET: api/Menu
        public int[] Get()
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT id from dbo.MenuTbl WHERE valid=1");
            DataTable dt = sql.executeSQLQuery(cmd);
            int[] idlist = new int[dt.Rows.Count];

            int tmp = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idlist[tmp] = int.Parse(dr["id"].ToString());
                tmp++;
            }
            return idlist;
        }

        // GET: api/Menu/5
        public menu Get(int id)
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT typeID,name,price,pic,remark from dbo.MenuTbl WHERE valid=1 and id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = sql.executeSQLQuery(cmd);
            menu Menu = new menu();
            if (dt.Rows.Count == 1)
            {
                Menu.typeID = int.Parse(dt.Rows[0]["typeID"].ToString());
                Menu.name = dt.Rows[0]["name"].ToString();
                Menu.price = dt.Rows[0]["price"].ToString();
                Menu.pic = dt.Rows[0]["pic"].ToString();
                Menu.remark = dt.Rows[0]["remark"].ToString();
            }
            return Menu;
        }

    }
}
