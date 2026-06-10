using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GV_MIEDU.Models; // Sử dụng dữ liệu từ tầng Models

namespace GV_MIEDU.WinForms
{
    public partial class LoginForm : Form
    {
        // Gọi Interface để tương tác với Database (Tính Trừu tượng)
        IQuanLyCanBo db = new DatabaseHelper();
        TextBox txtUser, txtPass;

        public LoginForm()
        {
            // Thiết lập cửa sổ
            this.Text = "Hệ thống MIEDU - Đăng nhập";
            this.Size = new Size(450, 280);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            // Tạo các thành phần giao diện bằng code
            Label lblTitle = new Label() { Text = "ĐĂNG NHẬP HỆ THỐNG", Font = new Font("Arial", 14, FontStyle.Bold), ForeColor = Color.Blue, Left = 100, Top = 20, Width = 250 };
            Label lblUser = new Label() { Text = "Tài khoản:", Left = 50, Top = 80 };
            txtUser = new TextBox() { Left = 150, Top = 78, Width = 200 };

            Label lblPass = new Label() { Text = "Mật khẩu:", Left = 50, Top = 120 };
            txtPass = new TextBox() { Left = 150, Top = 118, Width = 200, UseSystemPasswordChar = true }; // Ẩn mật khẩu

            Button btnLogin = new Button() { Text = "Đăng Nhập", Left = 150, Top = 170, Width = 120, Height = 40, BackColor = Color.LightSkyBlue, Cursor = Cursors.Hand };

            // Xử lý sự kiện khi bấm nút Đăng nhập
            btnLogin.Click += (s, e) => {
                var user = db.KiemTraDangNhap(txtUser.Text, txtPass.Text);
                if (user != null)
                {
                    this.Hide(); // Ẩn màn đăng nhập
                    new MainForm(user).ShowDialog(); // Mở màn hình chính và truyền user vào
                    this.Close(); // Đóng hẳn khi màn hình chính tắt
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            // Thêm tất cả vào Form
            this.Controls.AddRange(new Control[] { lblTitle, lblUser, txtUser, lblPass, txtPass, btnLogin });
        }
    }
}
