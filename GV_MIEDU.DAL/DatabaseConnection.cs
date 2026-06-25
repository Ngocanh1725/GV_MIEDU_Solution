using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GV_MIEDU.DAL
{ 
    public static class DatabaseConnection
    {
        // ĐỔI TÊN SERVER SQL CỦA BẠN VÀO ĐÂY
        public static string ConnectionString { get; } = @"Server=ADMIN-PC;Database=QuanLyMIEDU;Trusted_Connection=True;TrustServerCertificate=True;";
    }
}
