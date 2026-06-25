using System;
using System.Drawing;
using System.Windows.Forms;
using GV_MIEDU.Models;

namespace GV_MIEDU.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm(TaiKhoan user)
        {
            // Liên kết với giao diện đã khai báo trong Designer
            InitializeComponent();

            // Hiển thị thông tin người dùng truyền vào từ form Đăng nhập
            lblUser.Text = $"👤 Xin chào:\n{user.HoTen}";

            // Cập nhật tiêu đề cửa sổ kèm quyền của user
            this.Text = $"Hệ thống Quản lý Cán Bộ MIEDU - Phiên làm việc của: {user.Quyen}";
        }

        // Sự kiện xảy ra ngay khi Form vừa load lên
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Mở mặc định màn hình quản lý cán bộ
            btnQL.PerformClick();
        }

        // --- XỬ LÝ SỰ KIỆN CHUYỂN TRANG ---
        private void btnQL_Click(object sender, EventArgs e)
        {
            OpenControl(new ucCanBo(), "QUẢN LÝ CÁN BỘ GIẢNG DẠY");
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn đăng xuất để đổi tài khoản khác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Restart();
            }
        }

        // --- HÀM HỖ TRỢ ĐIỀU HƯỚNG ---
        // Xóa màn hình cũ và gắn màn hình (UserControl) mới vào pnlContent
        private void OpenControl(UserControl uc, string title)
        {
            pnlContent.Controls.Clear();
            uc.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(uc);
            lblTitle.Text = title;
        }

        // --- HIỆU ỨNG HOVER CHUỘT CHO CÁC NÚT BẤM SIDEBAR ---
        private void SidebarButton_MouseEnter(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.FromArgb(52, 73, 94); // Đổi màu sáng hơn khi đưa chuột vào
            }
        }

        private void SidebarButton_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.Transparent; // Trả về màu cũ khi bỏ chuột ra
            }
        }
    }
}