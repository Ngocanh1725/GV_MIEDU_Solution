using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using GV_MIEDU.Models;
using System.Linq;


namespace GV_MIEDU.DAL
{
    public class CanBoDAL : IQuanLyCanBo
    {
        public List<CanBo> LayDanhSach(string query = "SELECT * FROM CanBo")
        {
            List<CanBo> ds = new List<CanBo>();
            using (SqlConnection conn = new SqlConnection(DatabaseConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader r = cmd.ExecuteReader();
                while (r.Read())
                {
                    string loai = r["LoaiCB"].ToString();
                    // [TÍNH ĐA HÌNH & KẾ THỪA] Khởi tạo đối tượng lớp con tùy vào DB
                    if (loai == "GiangVien") ds.Add(new GiangVien(r["MaCB"].ToString(), r["HoTen"].ToString(), r["Khoa"].ToString(), r["MonHocDay"].ToString()));
                    else ds.Add(new ChuyenVien(r["MaCB"].ToString(), r["HoTen"].ToString(), r["Khoa"].ToString(), r["ChucVu"].ToString()));
                }
            }
            return ds;
        }

        public void Them(CanBo cb)
        {
            using (SqlConnection conn = new SqlConnection(DatabaseConnection.ConnectionString))
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
            using (SqlConnection conn = new SqlConnection(DatabaseConnection.ConnectionString))
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
            using (SqlConnection conn = new SqlConnection(DatabaseConnection.ConnectionString))
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