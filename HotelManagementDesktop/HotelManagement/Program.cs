using HotelManagement.GUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManagement
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Truyền tham số 1: "Quản lý" (Quyền), Tham số 2: "admin" (Username)
            Application.Run(new MainForm("Quản lý", "admin"));
            //Application.Run(new LoginForm());
        }
    }
}
