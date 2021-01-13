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
    public class MenuListController : ApiController
    {
        public struct menu
        {
            public int foodId;
            public string name;
            public string price;
            public string picture;
        }

        // GET: api/MenuList
        public menu[] Get()
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT id,name,price,pic,remark from dbo.MenuTbl WHERE valid=1");
            DataTable dt = sql.executeSQLQuery(cmd);
            menu[] Menu = new menu[dt.Rows.Count];
            if (dt.Rows.Count >= 1)
            {
                int tmp = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Menu[tmp].foodId = int.Parse(dr["id"].ToString());
                    Menu[tmp].name = dr["name"].ToString();
                    Menu[tmp].price = dr["price"].ToString();
                    Menu[tmp].picture = dr["pic"].ToString();
                    tmp++;
                }
            }
            return Menu;
        }

    }
}
