using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace RestAPIDemo.Controllers
{
    public class LoginController : ApiController
    {

        // GET: api/Login?id=xxx&pwhash=yyy
        public string Get(int id,string pwhash)
        {
            return GetToken(id, pwhash);
        }

        // POST: api/Login
        public string Post([FromBody]int id, [FromBody]string pwhash)
        {
            return GetToken(id, pwhash);
        }

        private static string GetToken(int id, string pwhash)
        {
            string token = "Password Error";
            SQL sql = new SQL();
            SecurityFunction sf = new SecurityFunction();
            SqlCommand cmd = new SqlCommand("SELECT * from dbo.StaffTbl WHERE id = @id and passwordHash = @passwordHash");
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@passwordHash", pwhash);
            DataTable dt = sql.executeSQLQuery(cmd);
            if (dt.Rows.Count == 1)
            {
                token = sf.GenToken(id);
            }
            return token;
        }
    }
}
