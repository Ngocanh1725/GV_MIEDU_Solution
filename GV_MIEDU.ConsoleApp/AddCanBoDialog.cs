using GV_MIEDU.BLL; // Gọi tầng BLL
using GV_MIEDU.Models;
using System;
using Terminal.Gui;
using NStack;

namespace GV_MIEDU.ConsoleApp
{
    public class AddCanBoDialog : Dialog
    {
        private CanBoBLL _bll = new CanBoBLL(); // Giao tiếp qua BLL
        private bool _isEdit;
        public bool IsSaved { get; private set; } = false;

        public AddCanBoDialog(CanBo cb = null) : base(cb == null ? "Thêm Cán Bộ" : "Sửa Cán Bộ", 60, 15)
        {
            _isEdit = cb != null;
            this.ColorScheme = ThemeManager.HackerScheme;

            var txtMa = new TextField(_isEdit ? cb.MaCB : "") { X = 15, Y = 1, Width = 35, ReadOnly = _isEdit, ColorScheme = ThemeManager.InputScheme };
            var txtTen = new TextField(_isEdit ? cb.HoTen : "") { X = 15, Y = 3, Width = 35, ColorScheme = ThemeManager.InputScheme };
            var txtKhoa = new TextField(_isEdit ? cb.Khoa : "") { X = 15, Y = 5, Width = 35, ColorScheme = ThemeManager.InputScheme };

            var radioLoai = new RadioGroup(new ustring[] { "Giảng Viên", "Chuyên Viên" }) { X = 15, Y = 7 };
            var lblPhu = new Label("Môn dạy/Chức vụ:") { X = 2, Y = 9 };

            string phuText = "";
            if (_isEdit)
            {
                if (cb is GiangVien gv) { radioLoai.SelectedItem = 0; phuText = gv.MonHocDay; }
                else if (cb is ChuyenVien cv) { radioLoai.SelectedItem = 1; phuText = cv.ChucVu; }
            }
            var txtPhu = new TextField(phuText) { X = 18, Y = 9, Width = 32, ColorScheme = ThemeManager.InputScheme };

            var btnSave = new Button("Lưu Lại", true) { X = Pos.Center() - 10, Y = 12 };
            var btnBack = new Button("Quay lại") { X = Pos.Center() + 4, Y = 12 };

            btnBack.Clicked += () => Application.RequestStop();

            btnSave.Clicked += () => {
                try
                {
                    // [TÍNH KẾ THỪA] Khởi tạo đối tượng lớp con tùy vào loại RadioGroup đang chọn
                    CanBo newCb = radioLoai.SelectedItem == 0
                        ? (CanBo)new GiangVien(txtMa.Text.ToString(), txtTen.Text.ToString(), txtKhoa.Text.ToString(), txtPhu.Text.ToString())
                        : (CanBo)new ChuyenVien(txtMa.Text.ToString(), txtTen.Text.ToString(), txtKhoa.Text.ToString(), txtPhu.Text.ToString());

                    // Đẩy dữ liệu qua tầng BLL để kiểm tra quy tắc nghiệp vụ trước khi lưu xuống SQL
                    if (_isEdit)
                        _bll.Sua(newCb);
                    else
                        _bll.Them(newCb);

                    IsSaved = true;
                    Application.RequestStop();
                }
                catch (Exception ex)
                {
                    // Bắt các Exception do Models ném ra (VD: Tên rỗng) hoặc do BLL ném ra
                    MessageBox.ErrorQuery("Lỗi", ex.Message, "OK");
                }
            };

            this.Add(new Label("Mã CB:") { X = 2, Y = 1 }, txtMa, new Label("Họ tên:") { X = 2, Y = 3 }, txtTen,
                     new Label("Khoa:") { X = 2, Y = 5 }, txtKhoa, new Label("Loại:") { X = 2, Y = 7 }, radioLoai,
                     lblPhu, txtPhu, btnSave, btnBack);
        }
    }
}