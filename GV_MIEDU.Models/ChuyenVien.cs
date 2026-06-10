using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH KẾ THỪA]
    public class ChuyenVien : CanBo
    {
        public string ChucVu { get; set; }

        public ChuyenVien(string maCB, string hoTen, string khoa, string chucVu)
            : base(maCB, hoTen, khoa)
        {
            ChucVu = chucVu;
        }

        // [TÍNH ĐA HÌNH]
        public override string LayThongTinChiTiet()
        {
            return base.LayThongTinChiTiet() + $" | Chức vụ: {ChucVu}";
        }
    }
}