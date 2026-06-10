using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;
using GV_MIEDU.Models;

namespace GV_MIEDU.ConsoleApp
{
    public class LoginDialog : Dialog
    {
        public TaiKhoan AuthUser { get; private set; }

        public LoginDialog(IQuanLyCanBo db) : base("HỆ THỐNG QUẢN LÝ MIEDU - ĐĂNG NHẬP", 60, 10)
        {
            this.ColorScheme = ThemeManager.HackerScheme;
            var txtUser = new TextField("") { X = 15, Y = 1, Width = 30, ColorScheme = ThemeManager.InputScheme };
            var txtPass = new TextField("") { X = 15, Y = 3, Width = 30, Secret = true, ColorScheme = ThemeManager.InputScheme };
            var btnLogin = new Button("Đăng nhập", true) { X = Pos.Center() - 5, Y = 5 };

            btnLogin.Clicked += () => {
                AuthUser = db.KiemTraDangNhap(txtUser.Text.ToString(), txtPass.Text.ToString());
                if (AuthUser != null) Application.RequestStop();
                else MessageBox.ErrorQuery("Lỗi", "Sai thông tin!", "OK");
            };
            this.Add(new Label("Tài khoản:") { X = 2, Y = 1 }, txtUser, new Label("Mật khẩu:") { X = 2, Y = 3 }, txtPass, btnLogin);
        }
    }
}
