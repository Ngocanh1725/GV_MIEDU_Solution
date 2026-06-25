using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GV_MIEDU.Models;
using GV_MIEDU.BLL; // Chỉ giao tiếp với tầng BLL, cấm dùng DAL hay SqlClient ở đây

namespace GV_MIEDU.WinForms
{
    public partial class ucCanBo : UserControl
    {
        // Khởi tạo BLL để xử lý nghiệp vụ
        private CanBoBLL _bll = new CanBoBLL();

        public ucCanBo()
        {
            // Lệnh này nối file giao diện (Designer) với file Code này
            InitializeComponent();
        }

        private void ucCanBo_Load(object sender, EventArgs e)
        {
            cmbLocKhoa.SelectedIndex = 0;
            LoadData(); // Load danh sách ban đầu khi vừa mở UserControl
        }

        // ====================================================================
        // HÀM LOAD DATA (CHỨNG MINH TÍNH ĐA HÌNH - POLYMORPHISM)
        // ====================================================================
        private void LoadData(List<CanBo> list = null)
        {
            // Nếu không truyền list vào thì mặc định lấy toàn bộ từ BLL
            if (list == null) list = _bll.LayDanhSach();

            dgvData.DataSource = list.Select(c => new {
                MaCB = c.MaCB,
                HoTen = c.HoTen,
                Khoa = c.Khoa,
                // [TÍNH ĐA HÌNH]: 
                // Không cần dùng lệnh if-else để kiểm tra xem "c" là Giảng viên hay Chuyên viên.
                // Hệ thống tự động gọi hàm LayThongTinChiTiet() tương ứng của lớp con được khởi tạo.
                ThongTinChiTiet = c.LayThongTinChiTiet()
            }).ToList();
        }

        // ====================================================================
        // CÁC SỰ KIỆN NÚT BẤM (GỌI BLL)
        // ====================================================================
        private void btnThem_Click(object sender, EventArgs e)
        {
            ShowDialogAddEdit(null); // Truyền null báo hiệu là Thêm mới
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn 1 dòng để sửa!"); return; }

            string ma = dgvData.SelectedRows[0].Cells["MaCB"].Value.ToString();
            var cb = _bll.LayDanhSach().FirstOrDefault(x => x.MaCB == ma);

            if (cb != null) ShowDialogAddEdit(cb); // Truyền đối tượng vào báo hiệu là Sửa
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvData.SelectedRows.Count == 0) { MessageBox.Show("Vui lòng chọn 1 dòng để xóa!"); return; }

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa cán bộ này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string ma = dgvData.SelectedRows[0].Cells["MaCB"].Value.ToString();
                    _bll.Xoa(ma); // Đẩy xuống BLL xử lý xóa
                    LoadData();   // Tải lại bảng
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            LoadData(_bll.TimKiem(txtTimKiem.Text));
        }

        private void btnSapXep_Click(object sender, EventArgs e)
        {
            LoadData(_bll.SapXepTheoTen());
        }

        private void cmbLocKhoa_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLocKhoa.SelectedIndex == 0)
                LoadData(); // "Tất cả khoa"
            else
                LoadData(_bll.LocTheoKhoa(cmbLocKhoa.SelectedItem.ToString()));
        }

        // ====================================================================
        // HỘP THOẠI THÊM / SỬA (TẠO DYNAMIC BẰNG CODE)
        // CHỨNG MINH TÍNH KẾ THỪA VÀ ĐÓNG GÓI
        // ====================================================================
        private void ShowDialogAddEdit(CanBo obj = null)
        {
            bool isEdit = obj != null;
            using (Form f = new Form() { Text = isEdit ? "Sửa Cán Bộ" : "Thêm Cán Bộ", Size = new Size(420, 380), StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, BackColor = Color.White })
            {
                Font font = new Font("Segoe UI", 11);

                TextBox txtMa = new TextBox() { Left = 150, Top = 30, Width = 220, Font = font, Text = isEdit ? obj.MaCB : "", Enabled = !isEdit };
                TextBox txtTen = new TextBox() { Left = 150, Top = 70, Width = 220, Font = font, Text = isEdit ? obj.HoTen : "" };

                ComboBox cmbKhoa = new ComboBox() { Left = 150, Top = 110, Width = 220, Font = font, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbKhoa.Items.AddRange(new[] { "Công nghệ Thông tin", "Kinh tế", "Ngoại ngữ" });
                cmbKhoa.SelectedItem = isEdit ? obj.Khoa : "Công nghệ Thông tin";

                ComboBox cmbLoai = new ComboBox() { Left = 150, Top = 150, Width = 220, Font = font, DropDownStyle = ComboBoxStyle.DropDownList, Enabled = !isEdit };
                cmbLoai.Items.AddRange(new[] { "Giảng Viên", "Chuyên Viên" });
                // Phân biệt đối tượng cũ là loại gì
                cmbLoai.SelectedIndex = isEdit ? (obj is GiangVien ? 0 : 1) : 0;

                Label lblPhu = new Label() { Text = "Môn dạy:", Left = 30, Top = 190, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                TextBox txtPhu = new TextBox() { Left = 150, Top = 190, Width = 220, Font = font };

                // Nạp dữ liệu cũ vào form nếu là chế độ Sửa
                if (isEdit) txtPhu.Text = obj is GiangVien gv ? gv.MonHocDay : ((ChuyenVien)obj).ChucVu;

                // Tự động đổi Label khi đổi loại cán bộ
                cmbLoai.SelectedIndexChanged += (s, e) => lblPhu.Text = cmbLoai.SelectedIndex == 0 ? "Môn dạy:" : "Chức vụ:";

                Button btnLuu = new Button() { Text = "Lưu Dữ Liệu", Left = 80, Top = 260, Width = 120, Height = 40, BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
                Button btnHuy = new Button() { Text = "Hủy Bỏ", Left = 220, Top = 260, Width = 120, Height = 40, BackColor = Color.Gray, ForeColor = Color.White, Font = new Font("Segoe UI", 10, FontStyle.Bold), FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };

                btnHuy.Click += (s, e) => f.Close();

                btnLuu.Click += (s, e) => {
                    try
                    {
                        // [TÍNH KẾ THỪA] - Khởi tạo đối tượng lớp con tùy vào lựa chọn ComboBox
                        CanBo cb = cmbLoai.SelectedIndex == 0
                            ? (CanBo)new GiangVien(txtMa.Text, txtTen.Text, cmbKhoa.Text, txtPhu.Text)
                            : (CanBo)new ChuyenVien(txtMa.Text, txtTen.Text, cmbKhoa.Text, txtPhu.Text);

                        // [TÍNH ĐÓNG GÓI] - Nếu txtMa, txtTen rỗng, property Set trong class CanBo 
                        // hoặc tầng BLL sẽ Throw Exception và bị khối catch ở dưới bắt lại.
                        if (isEdit)
                            _bll.Sua(cb);
                        else
                            _bll.Them(cb);

                        f.DialogResult = DialogResult.OK; // Báo lưu thành công
                        f.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Lỗi Nghiệp vụ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                };

                f.Controls.AddRange(new Control[] {
                    new Label() { Text = "Mã Cán Bộ:", Left = 30, Top = 30, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, txtMa,
                    new Label() { Text = "Họ và Tên:", Left = 30, Top = 70, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, txtTen,
                    new Label() { Text = "Thuộc Khoa:", Left = 30, Top = 110, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, cmbKhoa,
                    new Label() { Text = "Loại Cán Bộ:", Left = 30, Top = 150, Font = new Font("Segoe UI", 10, FontStyle.Bold) }, cmbLoai,
                    lblPhu, txtPhu, btnLuu, btnHuy
                });

                // Nếu hộp thoại đóng lại với trạng thái OK thì tải lại dữ liệu mới lên bảng
                if (f.ShowDialog() == DialogResult.OK) LoadData();
            }
        }
    }
}