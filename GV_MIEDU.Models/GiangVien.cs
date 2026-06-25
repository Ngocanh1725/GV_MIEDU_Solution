using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH KẾ THỪA]
    public class GiangVien : CanBo
    {
        public string MonHocDay { get; set; }

        public GiangVien(string maCB, string hoTen, string khoa, string monHocDay) : base(maCB, hoTen, khoa)
        {
            MonHocDay = monHocDay;
        }

        // [TÍNH ĐA HÌNH]: Ghi đè phương thức hiển thị thông tin
        public override string LayThongTinChiTiet()
        {
            return base.LayThongTinChiTiet() + $" | Môn dạy: {MonHocDay}";
        }
    }
}
