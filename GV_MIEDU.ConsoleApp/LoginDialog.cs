using System;
using Terminal.Gui;
using GV_MIEDU.Models;
using GV_MIEDU.BLL; // Thêm thư viện BLL

namespace GV_MIEDU.ConsoleApp
{
    public class LoginDialog : Dialog
    {
        public TaiKhoan AuthUser { get; private set; }

        // Khởi tạo BLL để xử lý nghiệp vụ đăng nhập
        private AuthBLL _authBLL = new AuthBLL();

        public LoginDialog() : base("HỆ THỐNG QUẢN LÝ MIEDU - ĐĂNG NHẬP", 60, 10)
        {
            this.ColorScheme = ThemeManager.HackerScheme;
            var txtUser = new TextField("") { X = 15, Y = 1, Width = 30, ColorScheme = ThemeManager.InputScheme };
            var txtPass = new TextField("") { X = 15, Y = 3, Width = 30, Secret = true, ColorScheme = ThemeManager.InputScheme };
            var btnLogin = new Button("Đăng nhập", true) { X = Pos.Center() - 5, Y = 5 };

            btnLogin.Clicked += () => {
                try
                {
                    // Lớp BLL kiểm tra tính hợp lệ rồi mới gọi DAL để chọc SQL
                    AuthUser = _authBLL.Login(txtUser.Text.ToString(), txtPass.Text.ToString());

                    if (AuthUser != null)
                        Application.RequestStop();
                    else
                        MessageBox.ErrorQuery("Lỗi", "Sai thông tin tài khoản hoặc mật khẩu!", "OK");
                }
                catch (Exception ex)
                {
                    // Bắt các ngoại lệ nghiệp vụ (ví dụ: để trống ô nhập)
                    MessageBox.ErrorQuery("Lỗi Nghiệp Vụ", ex.Message, "OK");
                }
            };

            this.Add(new Label("Tài khoản:") { X = 2, Y = 1 }, txtUser, new Label("Mật khẩu:") { X = 2, Y = 3 }, txtPass, btnLogin);
        }
    }
}