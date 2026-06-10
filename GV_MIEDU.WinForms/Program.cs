using System;
using System.Windows.Forms;

namespace GV_MIEDU.WinForms
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Chạy màn hình Đăng nhập đầu tiên
            Application.Run(new LoginForm());
        }
    }
}