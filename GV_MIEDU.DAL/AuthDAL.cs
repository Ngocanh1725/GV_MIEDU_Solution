using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using GV_MIEDU.Models;

namespace GV_MIEDU.DAL
{
    public class AuthDAL
    {
        public TaiKhoan KiemTraDangNhap(string username, string password)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TaiKhoan WHERE TenDangNhap=@u AND MatKhau=@p", conn);
                cmd.Parameters.AddWithValue("@u", username); cmd.Parameters.AddWithValue("@p", password);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read()) return new TaiKhoan(r["TenDangNhap"].ToString(), r["MatKhau"].ToString(), r["HoTen"].ToString(), r["Quyen"].ToString());
                return null;
            }
        }
    }
}