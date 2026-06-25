using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    public abstract class CanBo
    {
        // [TÍNH ĐÓNG GÓI]: Thuộc tính private, giao tiếp qua bộ get/set để kiểm soát dữ liệu
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

        // [TÍNH ĐA HÌNH - Chuẩn bị]: Phương thức ảo cho phép lớp con ghi đè
        public virtual string LayThongTinChiTiet()
        {
            return $"Khoa: {Khoa}";
        }
    }
}