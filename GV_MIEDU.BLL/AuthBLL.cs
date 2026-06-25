using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV_MIEDU.DAL;
using GV_MIEDU.Models;

namespace GV_MIEDU.BLL
{
    public class AuthBLL
    {
        private AuthDAL _dal = new AuthDAL();

        public TaiKhoan Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                throw new Exception("Lỗi: Tài khoản và mật khẩu không được để trống!");
            return _dal.KiemTraDangNhap(username, password);
        }
    }
}