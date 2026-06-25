using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.Models
{
    // [TÍNH TRỪU TƯỢNG]: Chỉ định nghĩa "làm gì", không quan tâm "làm như thế nào"
    public interface IQuanLyCanBo
    {
        List<CanBo> LayDanhSach(string query = "SELECT * FROM CanBo");
        void Them(CanBo cb);
        void Sua(CanBo cb);
        void Xoa(string maCB);
        List<CanBo> TimKiem(string tuKhoa);
        List<CanBo> LocTheoKhoa(string khoa);
        List<CanBo> SapXepTheoTen();
    }
}