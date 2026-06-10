using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    public class TaiKhoan
    {
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Quyen { get; set; }

        public TaiKhoan(string tenDangNhap, string matKhau, string hoTen, string quyen)
        {
            TenDangNhap = tenDangNhap; MatKhau = matKhau; HoTen = hoTen; Quyen = quyen;
        }
    }
}

