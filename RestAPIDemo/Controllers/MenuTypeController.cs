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
    public class MenuTypeController : ApiController
    {
        // GET: api/MenuType
        public int[] Get()
        {
            
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT id from dbo.MenuTypeTbl where valid=1");
            DataTable dt = sql.executeSQLQuery(cmd);
            int[] idList = new int[dt.Rows.Count];

            int tmp = 0;
            foreach (DataRow dr in dt.Rows)
            {
                idList[tmp]  = int.Parse(dr["id"].ToString());
                tmp++;
            }
            return idList;
        }

        // GET: api/MenuType/5
        public string Get(int id)
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT name from dbo.MenuTypeTbl where id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = sql.executeSQLQuery(cmd);
            if (dt.Rows.Count <= 0)
            {
                return "Error: no such menuType id";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

    }
}
