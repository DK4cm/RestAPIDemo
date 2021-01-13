using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace comp5527
{
    class SecurityFunction
    {
        public string CalculateMD5Hash(string input)
        {

            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();

            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)

            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }

        public string GenToken(int id)
        {
            string token = CalculateMD5Hash(id.ToString() + DateTime.Now.ToString());
            string expire = DateTime.Now.AddHours(8).ToString();
            SQL sql = new SQL();
            SqlCommand cmd = new SqlCommand("Update StaffTbl set token=@token,tokenExpire=@expire where id = @id");
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@expire", expire);
            cmd.Parameters.AddWithValue("@id", id);
            sql.executeSQL(cmd);
            return token;
        }

        public bool checkToken(string token)
        {
            if (token.Equals("ABCDEFG"))//backdoor for test,shoulbe remove
            {
                return true;
            }
            string expire = DateTime.Now.ToString();
            SQL sql = new SQL();
            SecurityFunction sf = new SecurityFunction();
            SqlCommand cmd = new SqlCommand("SELECT * from dbo.StaffTbl WHERE token = @token and tokenExpire > @expire");
            cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@expire", expire);
            DataTable dt = sql.executeSQLQuery(cmd);
            if (dt.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }
    }
}
