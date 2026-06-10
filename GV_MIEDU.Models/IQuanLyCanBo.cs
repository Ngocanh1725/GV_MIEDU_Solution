using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH TRỪU TƯỢNG]: Giao diện quy định CÁC HÀNH ĐỘNG mà một lớp Quản lý phải có
    public interface IQuanLyCanBo
    {
        TaiKhoan KiemTraDangNhap(string username, string password);
        List<CanBo> LayDanhSach(string query = "SELECT * FROM CanBo");
        void Them(CanBo cb);
        void Sua(CanBo cb);
        void Xoa(string maCB);
        List<CanBo> TimKiem(string tuKhoa);
        List<CanBo> LocTheoKhoa(string khoa);
        List<CanBo> SapXepTheoTen();
    }
}
