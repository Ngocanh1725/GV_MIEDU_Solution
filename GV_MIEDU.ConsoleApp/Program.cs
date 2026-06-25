using System;
using Terminal.Gui;
using GV_MIEDU.Models;

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

            while (true)
            {
                // Mở màn hình đăng nhập (Không cần truyền db nữa vì LoginDialog sẽ tự gọi AuthBLL)
                var loginDialog = new LoginDialog();
                Application.Run(loginDialog);

                // Nếu người dùng tắt form mà chưa đăng nhập thành công thì thoát chương trình
                if (loginDialog.AuthUser == null) break;

                // Nếu đăng nhập thành công, mở màn hình chính (truyền user vào)
                var mainWindow = new MainWindow(loginDialog.AuthUser);
                mainWindow.Run();
            }

            // Tắt ứng dụng an toàn
            Application.Shutdown();
        }
    }
}