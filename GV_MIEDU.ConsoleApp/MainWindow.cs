using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Terminal.Gui;
using GV_MIEDU.Models;

namespace GV_MIEDU.ConsoleApp
{
    public class MainWindow
    {
        private IQuanLyCanBo _db;
        private TaiKhoan _user;
        private TableView _tableView;

        public MainWindow(TaiKhoan user, IQuanLyCanBo db) { _user = user; _db = db; }

        public void Run()
        {
            var top = new Toplevel() { ColorScheme = ThemeManager.HackerScheme };
            var menu = new MenuBar(new MenuBarItem[] {
                new MenuBarItem ("_Hệ thống", new MenuItem [] { new MenuItem ("_Đăng xuất", "", () => Application.RequestStop()) }),
                new MenuBarItem ("_Hành động", new MenuItem [] {
                    new MenuItem ("_Thêm", "", () => ShowDialog(null)),
                    new MenuItem ("_Sửa (Chọn dòng)", "", () => EditSelected()),
                    new MenuItem ("_Xóa (Chọn dòng)", "", () => DeleteSelected())
                })
            });

            var win = new Window($"BẢNG ĐIỀU KHIỂN - {_user.HoTen}") { X = 0, Y = 1, Width = Dim.Fill(), Height = Dim.Fill() };
            var leftPane = new FrameView("Chức năng") { X = 0, Y = 0, Width = Dim.Percent(20), Height = Dim.Fill() };
            var rightPane = new FrameView("Dữ Liệu") { X = Pos.Right(leftPane), Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };

            var menuList = new ListView(new[] { "1. Xem tất cả", "2. Thêm mới", "3. Sửa", "4. Xóa", "5. Sắp xếp Tên", "6. Lọc Khoa CNTT", "7. Tìm kiếm" }) { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill() };
            _tableView = new TableView() { X = 0, Y = 0, Width = Dim.Fill(), Height = Dim.Fill(), FullRowSelect = true };

            menuList.OpenSelectedItem += (e) => {
                if (e.Item == 0) LoadData(_db.LayDanhSach());
                if (e.Item == 1) ShowDialog(null);
                if (e.Item == 2) EditSelected();
                if (e.Item == 3) DeleteSelected();
                if (e.Item == 4) LoadData(_db.SapXepTheoTen());
                if (e.Item == 5) LoadData(_db.LocTheoKhoa("Công nghệ Thông tin"));
                if (e.Item == 6) SearchData();
            };

            leftPane.Add(menuList); rightPane.Add(_tableView); win.Add(leftPane, rightPane); top.Add(menu, win);
            LoadData(_db.LayDanhSach()); Application.Run(top);
        }

        private void LoadData(List<CanBo> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Mã CB"); dt.Columns.Add("Họ Tên"); dt.Columns.Add("Khoa"); dt.Columns.Add("Chi Tiết (Đa Hình)");
            foreach (var item in list) dt.Rows.Add(item.MaCB, item.HoTen, item.Khoa, item.LayThongTinChiTiet());
            _tableView.Table = dt; _tableView.Update();
        }

        private void ShowDialog(CanBo cb)
        {
            var dialog = new AddCanBoDialog(_db, cb); Application.Run(dialog);
            if (dialog.IsSaved) LoadData(_db.LayDanhSach());
        }

        private void EditSelected()
        {
            if (_tableView.SelectedRow < 0) return;
            string ma = _tableView.Table.Rows[_tableView.SelectedRow][0].ToString();
            var item = _db.LayDanhSach().Find(c => c.MaCB == ma);
            if (item != null) ShowDialog(item);
        }

        private void DeleteSelected()
        {
            if (_tableView.SelectedRow < 0) return;
            string ma = _tableView.Table.Rows[_tableView.SelectedRow][0].ToString();
            if (MessageBox.Query("Xác nhận", $"Xóa CB {ma}?", "Có", "Không") == 0) { _db.Xoa(ma); LoadData(_db.LayDanhSach()); }
        }

        private void SearchData()
        {
            var txt = new TextField("") { X = 2, Y = 2, Width = 30, ColorScheme = ThemeManager.InputScheme };
            var d = new Dialog("Tìm kiếm", 40, 8) { ColorScheme = ThemeManager.HackerScheme };
            var b = new Button("Tìm", true) { X = Pos.Center(), Y = 4 };
            b.Clicked += () => { LoadData(_db.TimKiem(txt.Text.ToString())); Application.RequestStop(); };
            d.Add(new Label("Nhập tên/mã:") { X = 2, Y = 1 }, txt, b); Application.Run(d);
        }
    }
}