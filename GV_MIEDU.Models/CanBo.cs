using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH TRỪU TƯỢNG] (Abstraction): Lớp abstract, không cho phép khởi tạo đối tượng CanBo chung chung.
    public abstract class CanBo
    {
        // [TÍNH ĐÓNG GÓI] (Encapsulation): Che giấu dữ liệu (private) và giao tiếp qua Properties.
        private string _maCB;
        private string _hoTen;

        public string MaCB
        {
            get => _maCB;
            set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Mã không hợp lệ!"); _maCB = value; }
        }
        public string HoTen
        {
            get => _hoTen;
            set { if (string.IsNullOrWhiteSpace(value)) throw new ArgumentException("Tên không hợp lệ!"); _hoTen = value; }
        }
        public string Khoa { get; set; }

        public CanBo(string maCB, string hoTen, string khoa)
        {
            MaCB = maCB; HoTen = hoTen; Khoa = khoa;
        }

        // [TÍNH ĐA HÌNH] (Polymorphism): Khai báo virtual để lớp con có thể ghi đè (override)
        public virtual string LayThongTinChiTiet()
        {
            return $"Mã: {MaCB} | Tên: {HoTen} | Khoa: {Khoa}";
        }
    }
}