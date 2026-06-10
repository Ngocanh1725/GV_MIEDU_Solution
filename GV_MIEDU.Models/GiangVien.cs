using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH KẾ THỪA] (Inheritance): Giảng Viên LÀ MỘT Cán Bộ
    public class GiangVien : CanBo
    {
        public string MonHocDay { get; set; }

        public GiangVien(string maCB, string hoTen, string khoa, string monHocDay)
            : base(maCB, hoTen, khoa) // Dùng 'base' để tái sử dụng Constructor lớp cha
        {
            MonHocDay = monHocDay;
        }

        // [TÍNH ĐA HÌNH]: Ghi đè phương thức hiển thị
        public override string LayThongTinChiTiet()
        {
            return base.LayThongTinChiTiet() + $" | Môn dạy: {MonHocDay}";
        }
    }
}
