using System;
using System.Windows.Forms;
using GV_MIEDU.Models;
using GV_MIEDU.BLL; // Gọi tầng BLL

namespace GV_MIEDU.WinForms
{
    public partial class LoginForm : Form
    {
        // Giao tiếp qua BLL thay vì kết nối trực tiếp CSDL
        private AuthBLL _authBLL = new AuthBLL();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Lớp BLL sẽ kiểm tra tính hợp lệ và check SQL (thông qua DAL)
                TaiKhoan user = _authBLL.Login(txtUser.Text, txtPass.Text);

                if (user != null)
                {
                    this.Hide();
                    MainForm main = new MainForm(user);
                    // Khi Form chính đóng thì cũng đóng luôn Form đăng nhập ngầm
                    main.FormClosed += (s, args) => this.Close();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Sai tài khoản hoặc mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Bắt các lỗi nghiệp vụ từ BLL trả về (ví dụ để trống tài khoản)
                MessageBox.Show(ex.Message, "Cảnh báo Nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}