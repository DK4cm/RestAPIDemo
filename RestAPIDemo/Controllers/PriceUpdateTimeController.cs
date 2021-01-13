using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;

namespace RestAPIDemo.Controllers
{
    public class PriceUpdateTimeController : ApiController
    {

        private string getUpdateTime()
        {
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("SELECT priceUpdateTime from dbo.Setting");
            DataTable dt = sql.executeSQLQuery(cmd);
            DateTime date = Convert.ToDateTime(dt.Rows[0]["priceUpdateTime"].ToString());
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }
        // GET: api/PriceUpdateTime
        public string Get()
        {
            return getUpdateTime();
        }

        // GET: api/PriceUpdateTime/5
        public string Get(int id)
        {
            return getUpdateTime();
        }
        
    }
}
