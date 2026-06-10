using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    public class DatabaseHelper : IQuanLyCanBo
    {
        // LƯU Ý: Thay đổi tên Server cho khớp với máy của bạn
        private string connStr = @"Server=localhost;Database=QuanLyMIEDU;Trusted_Connection=True;TrustServerCertificate=True;";

        public TaiKhoan KiemTraDangNhap(string user, string pass)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM TaiKhoan WHERE TenDangNhap=@u AND MatKhau=@p", conn);
                cmd.Parameters.AddWithValue("@u", user); cmd.Parameters.AddWithValue("@p", pass);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                if (r.Read()) return new TaiKhoan(r["TenDangNhap"].ToString(), r["MatKhau"].ToString(), r["HoTen"].ToString(), r["Quyen"].ToString());
                return null;
            }
        }

        public List<CanBo> LayDanhSach(string query = "SELECT * FROM CanBo")
        {
            List<CanBo> ds = new List<CanBo>();
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string loai = r["LoaiCB"].ToString();
                    if (loai == "GiangVien")
                        ds.Add(new GiangVien(r["MaCB"].ToString(), r["HoTen"].ToString(), r["Khoa"].ToString(), r["MonHocDay"].ToString()));
                    else
                        ds.Add(new ChuyenVien(r["MaCB"].ToString(), r["HoTen"].ToString(), r["Khoa"].ToString(), r["ChucVu"].ToString()));
                }
            }
            return ds;
        }

        public void Them(CanBo cb)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string q = "INSERT INTO CanBo VALUES (@ma, @ten, @khoa, @loai, @mon, @cv)";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@ma", cb.MaCB); cmd.Parameters.AddWithValue("@ten", cb.HoTen); cmd.Parameters.AddWithValue("@khoa", cb.Khoa);
                if (cb is GiangVien gv) { cmd.Parameters.AddWithValue("@loai", "GiangVien"); cmd.Parameters.AddWithValue("@mon", gv.MonHocDay); cmd.Parameters.AddWithValue("@cv", DBNull.Value); }
                else if (cb is ChuyenVien cv) { cmd.Parameters.AddWithValue("@loai", "ChuyenVien"); cmd.Parameters.AddWithValue("@mon", DBNull.Value); cmd.Parameters.AddWithValue("@cv", cv.ChucVu); }
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Sua(CanBo cb)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string q = "UPDATE CanBo SET HoTen=@ten, Khoa=@khoa, MonHocDay=@mon, ChucVu=@cv WHERE MaCB=@ma";
                SqlCommand cmd = new SqlCommand(q, conn);
                cmd.Parameters.AddWithValue("@ma", cb.MaCB); cmd.Parameters.AddWithValue("@ten", cb.HoTen); cmd.Parameters.AddWithValue("@khoa", cb.Khoa);
                if (cb is GiangVien gv) { cmd.Parameters.AddWithValue("@mon", gv.MonHocDay); cmd.Parameters.AddWithValue("@cv", DBNull.Value); }
                else if (cb is ChuyenVien cv) { cmd.Parameters.AddWithValue("@mon", DBNull.Value); cmd.Parameters.AddWithValue("@cv", cv.ChucVu); }
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void Xoa(string maCB)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd = new SqlCommand("DELETE FROM CanBo WHERE MaCB=@ma", conn);
                cmd.Parameters.AddWithValue("@ma", maCB);
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        public List<CanBo> TimKiem(string tk) => LayDanhSach($"SELECT * FROM CanBo WHERE HoTen LIKE N'%{tk}%' OR MaCB LIKE '%{tk}%'");
        public List<CanBo> LocTheoKhoa(string khoa) => LayDanhSach($"SELECT * FROM CanBo WHERE Khoa = N'{khoa}'");
        public List<CanBo> SapXepTheoTen() => LayDanhSach().OrderBy(c => c.HoTen.Split(' ').Last()).ToList();
    }
}
