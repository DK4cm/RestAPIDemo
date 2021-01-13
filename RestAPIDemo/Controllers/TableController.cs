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
    public class TableController : ApiController
    {
        // GET: api/Table
        public int[] Get()
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT id from dbo.TableTbl WHERE valid=1");
            DataTable dt = sql.executeSQLQuery(cmd);
            int[] tableList = new int[dt.Rows.Count];

            int tmp = 0;
            foreach (DataRow dr in dt.Rows)
            {
                tableList[tmp] = int.Parse(dr["id"].ToString());
                tmp++;
            }
            return tableList;
        }

        public struct Table
        {
            public int num;
            public bool flag;
            public string description;
        }

        // GET: api/Table/5
        public Table Get(int id)
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT num,flag,description from dbo.TableTbl WHERE id=@id");
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = sql.executeSQLQuery(cmd);
            Table table = new Table();
            if (dt.Rows.Count == 1)
            {
                table.num= int.Parse(dt.Rows[0]["num"].ToString());
                table.flag = bool.Parse(dt.Rows[0]["flag"].ToString());
                table.description = dt.Rows[0]["description"].ToString();
            }
            return table;
        }

    }
}
