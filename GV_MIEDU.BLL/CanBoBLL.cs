using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV_MIEDU.DAL;
using GV_MIEDU.Models;

namespace GV_MIEDU.BLL
{
    public class CanBoBLL
    {
        private CanBoDAL _dal = new CanBoDAL();

        public List<CanBo> LayDanhSach() => _dal.LayDanhSach();
        public List<CanBo> TimKiem(string tk) => _dal.TimKiem(tk);
        public List<CanBo> LocTheoKhoa(string k) => _dal.LocTheoKhoa(k);
        public List<CanBo> SapXepTheoTen() => _dal.SapXepTheoTen();

        public void Them(CanBo cb)
        {
            if (string.IsNullOrWhiteSpace(cb.MaCB)) throw new Exception("Nghiệp vụ từ chối: Mã không được để trống!");
            if (string.IsNullOrWhiteSpace(cb.HoTen)) throw new Exception("Nghiệp vụ từ chối: Tên không được để trống!");
            _dal.Them(cb);
        }

        public void Sua(CanBo cb)
        {
            if (string.IsNullOrWhiteSpace(cb.HoTen)) throw new Exception("Nghiệp vụ từ chối: Tên không được để trống!");
            _dal.Sua(cb);
        }

        public void Xoa(string maCB)
        {
            if (string.IsNullOrWhiteSpace(maCB)) throw new Exception("Lỗi: Mã cán bộ rỗng!");
            _dal.Xoa(maCB);
        }
    }
}