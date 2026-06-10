using System;
using Terminal.Gui; // Dòng này cực kỳ quan trọng để sửa lỗi "Application does not exist"
using GV_MIEDU.Models; // Đổi lại theo đúng tên Project Models của bạn

namespace GV_MIEDU.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            // Khởi tạo thư viện đồ họa Terminal.Gui
            Application.Init();

            // Khởi tạo Theme màu sắc
            ThemeManager.Initialize();

            // Khởi tạo kết nối CSDL (Đa hình qua Interface)
            IQuanLyCanBo db = new DatabaseHelper();

            while (true)
            {
                // Mở màn hình đăng nhập
                var loginDialog = new LoginDialog(db);
                Application.Run(loginDialog);

                // Nếu người dùng tắt form mà chưa đăng nhập thành công thì thoát chương trình
                if (loginDialog.AuthUser == null) break;

                // Nếu đăng nhập thành công, mở màn hình chính (truyền user và db vào)
                var mainWindow = new MainWindow(loginDialog.AuthUser, db);
                mainWindow.Run();
            }

            // Tắt ứng dụng an toàn
            Application.Shutdown();
        }
    }
}