using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GV_MIEDU.Models;

namespace GV_MIEDU.WinForms
{
    public partial class MainForm : Form
    {
        public MainForm(TaiKhoan user)
        {
            this.Text = $"Quản lý Cán Bộ MIEDU - Xin chào: {user.HoTen} ({user.Quyen})";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // --- TẠO THANH SIDEBAR BÊN TRÁI ---
            Panel pnlSidebar = new Panel() { Dock = DockStyle.Left, Width = 220, BackColor = Color.FromArgb(41, 128, 185) };

            // --- TẠO KHUNG HIỂN THỊ NỘI DUNG BÊN PHẢI ---
            Panel pnlContent = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White };

            // Nút Menu: Quản lý Cán bộ
            Button btnQL = new Button() { Text = "Quản lý Cán Bộ", Dock = DockStyle.Top, Height = 60, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Arial", 11, FontStyle.Bold), Cursor = Cursors.Hand };
            btnQL.FlatAppearance.BorderSize = 0;

            // Khi bấm vào nút Quản lý, hiển thị UserControl ucCanBo vào khung Content
            btnQL.Click += (s, e) => {
                pnlContent.Controls.Clear();
                var uc = new ucCanBo() { Dock = DockStyle.Fill };
                pnlContent.Controls.Add(uc);
            };

            // Nút Menu: Đăng xuất (Nằm ở đáy Sidebar)
            Button btnThoat = new Button() { Text = "Đăng xuất", Dock = DockStyle.Bottom, Height = 60, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Arial", 11, FontStyle.Bold), Cursor = Cursors.Hand };
            btnThoat.FlatAppearance.BorderSize = 0;
            btnThoat.Click += (s, e) => { Application.Restart(); };

            pnlSidebar.Controls.Add(btnQL);
            pnlSidebar.Controls.Add(btnThoat);

            this.Controls.Add(pnlContent);
            this.Controls.Add(pnlSidebar);

            // Tự động bấm nút Quản lý khi vừa mở app
            btnQL.PerformClick();
        }
    }
}