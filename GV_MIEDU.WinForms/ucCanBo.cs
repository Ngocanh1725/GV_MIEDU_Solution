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
    public partial class ucCanBo : UserControl
    {
        IQuanLyCanBo db = new DatabaseHelper();
        DataGridView dgvData;
        TextBox txtTimKiem;
        ComboBox cmbLocKhoa;

        public ucCanBo()
        {
            // --- 1. THANH CÔNG CỤ PHÍA TRÊN ---
            Panel pnlTop = new Panel() { Dock = DockStyle.Top, Height = 70, BackColor = Color.WhiteSmoke };

            Button btnThem = new Button() { Text = "Thêm mới", Left = 15, Top = 15, Width = 90, Height = 35, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Cursor = Cursors.Hand };
            Button btnSua = new Button() { Text = "Sửa", Left = 115, Top = 15, Width = 70, Height = 35, BackColor = Color.FromArgb(243, 156, 18), ForeColor = Color.White, Cursor = Cursors.Hand };
            Button btnXoa = new Button() { Text = "Xóa", Left = 195, Top = 15, Width = 70, Height = 35, BackColor = Color.FromArgb(231, 76, 60), ForeColor = Color.White, Cursor = Cursors.Hand };
            Button btnSapXep = new Button() { Text = "Sắp xếp Tên", Left = 275, Top = 15, Width = 100, Height = 35, BackColor = Color.Gray, ForeColor = Color.White, Cursor = Cursors.Hand };

            cmbLocKhoa = new ComboBox() { Left = 400, Top = 20, Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbLocKhoa.Items.AddRange(new string[] { "Tất cả khoa", "Công nghệ Thông tin", "Kinh tế", "Ngoại ngữ" });
            cmbLocKhoa.SelectedIndex = 0;

            txtTimKiem = new TextBox() { Left = 570, Top = 20, Width = 160 };
            Button btnTim = new Button() { Text = "Tìm", Left = 740, Top = 18, Width = 60, Height = 30, BackColor = Color.FromArgb(52, 152, 219), ForeColor = Color.White, Cursor = Cursors.Hand };

            // --- BẮT SỰ KIỆN CÁC NÚT BẤM ---
            btnThem.Click += (s, e) => ShowDialogAddEdit();

            btnSua.Click += (s, e) => {
                if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn 1 dòng để sửa!"); return; }
                string ma = dgvData.SelectedRows[0].Cells["MaCB"].Value.ToString();
                var cb = db.LayDanhSach().FirstOrDefault(x => x.MaCB == ma);
                if (cb != null) ShowDialogAddEdit(cb);
            };

            btnXoa.Click += (s, e) => {
                if (dgvData.SelectedRows.Count > 0 && MessageBox.Show("Bạn có chắc chắn muốn xóa cán bộ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    db.Xoa(dgvData.SelectedRows[0].Cells["MaCB"].Value.ToString());
                    LoadData(); // Load lại bảng
                }
            };

            btnSapXep.Click += (s, e) => LoadData(db.SapXepTheoTen());
            btnTim.Click += (s, e) => LoadData(db.TimKiem(txtTimKiem.Text));

            cmbLocKhoa.SelectedIndexChanged += (s, e) => {
                if (cmbLocKhoa.SelectedIndex == 0) LoadData();
                else LoadData(db.LocTheoKhoa(cmbLocKhoa.SelectedItem.ToString()));
            };

            pnlTop.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnSapXep, cmbLocKhoa, txtTimKiem, btnTim });

            // --- 2. BẢNG DỮ LIỆU ---
            dgvData = new DataGridView()
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                RowTemplate = { Height = 35 }
            };

            this.Controls.Add(dgvData);
            this.Controls.Add(pnlTop);

            // Load dữ liệu lần đầu
            LoadData();
        }

        // Hàm LoadData thể hiện tính ĐA HÌNH
        private void LoadData(System.Collections.Generic.List<CanBo> list = null)
        {
            if (list == null) list = db.LayDanhSach();

            dgvData.DataSource = list.Select(c => new {
                MaCB = c.MaCB,
                HoTen = c.HoTen,
                Khoa = c.Khoa,
                // [TÍNH ĐA HÌNH]: Hàm LayThongTinChiTiet() tự động nhận biết đối tượng là Giảng viên hay Chuyên viên để trả về kết quả tương ứng
                ThongTinBoSung = c.LayThongTinChiTiet()
            }).ToList();
        }

        // Hộp thoại Thêm / Sửa Cán bộ (Thể hiện tính KẾ THỪA)
        private void ShowDialogAddEdit(CanBo obj = null)
        {
            bool isEdit = obj != null;
            using (Form f = new Form() { Text = isEdit ? "Sửa Cán Bộ" : "Thêm Cán Bộ", Size = new Size(400, 350), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false })
            {
                TextBox txtMa = new TextBox() { Left = 130, Top = 20, Width = 200, Text = isEdit ? obj.MaCB : "", Enabled = !isEdit };
                TextBox txtTen = new TextBox() { Left = 130, Top = 60, Width = 200, Text = isEdit ? obj.HoTen : "" };

                ComboBox cmbKhoa = new ComboBox() { Left = 130, Top = 100, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbKhoa.Items.AddRange(new[] { "Công nghệ Thông tin", "Kinh tế", "Ngoại ngữ" });
                cmbKhoa.SelectedItem = isEdit ? obj.Khoa : "Công nghệ Thông tin";

                ComboBox cmbLoai = new ComboBox() { Left = 130, Top = 140, Width = 200, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = !isEdit };
                cmbLoai.Items.AddRange(new[] { "Giảng Viên", "Chuyên Viên" });
                cmbLoai.SelectedIndex = isEdit ? (obj is GiangVien ? 0 : 1) : 0;

                Label lblPhu = new Label() { Text = "Môn dạy:", Left = 20, Top = 180 };
                TextBox txtPhu = new TextBox() { Left = 130, Top = 180, Width = 200 };

                // Nếu là sửa, nạp dữ liệu cũ vào ô phụ
                if (isEdit) txtPhu.Text = obj is GiangVien gv ? gv.MonHocDay : ((ChuyenVien)obj).ChucVu;

                // Tự động đổi nhãn khi chọn loại Cán bộ
                cmbLoai.SelectedIndexChanged += (s, e) => lblPhu.Text = cmbLoai.SelectedIndex == 0 ? "Môn dạy:" : "Chức vụ:";

                Button btnLuu = new Button() { Text = "Lưu Dữ Liệu", Left = 130, Top = 240, Width = 120, Height = 40, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Cursor = Cursors.Hand };

                btnLuu.Click += (s, e) => {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(txtMa.Text) || string.IsNullOrWhiteSpace(txtTen.Text)) throw new Exception("Vui lòng nhập đủ Mã và Tên!");

                        // [TÍNH KẾ THỪA]: Dựa vào ComboBox Loại để ép kiểu (Casting) và khởi tạo đối tượng tương ứng
                        CanBo cb = cmbLoai.SelectedIndex == 0
                            ? (CanBo)new GiangVien(txtMa.Text, txtTen.Text, cmbKhoa.Text, txtPhu.Text)
                            : (CanBo)new ChuyenVien(txtMa.Text, txtTen.Text, cmbKhoa.Text, txtPhu.Text);

                        if (isEdit) db.Sua(cb); else db.Them(cb);

                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                };

                f.Controls.AddRange(new Control[] {
                    new Label() { Text = "Mã Cán Bộ:", Left = 20, Top = 20 }, txtMa,
                    new Label() { Text = "Họ và Tên:", Left = 20, Top = 60 }, txtTen,
                    new Label() { Text = "Thuộc Khoa:", Left = 20, Top = 100 }, cmbKhoa,
                    new Label() { Text = "Loại Cán Bộ:", Left = 20, Top = 140 }, cmbLoai,
                    lblPhu, txtPhu, btnLuu
                });

                if (f.ShowDialog() == DialogResult.OK) LoadData();
            }
        }
    }
}