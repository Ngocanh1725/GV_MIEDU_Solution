using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terminal.Gui;

namespace GV_MIEDU.ConsoleApp
{
    public static class ThemeManager
    {
        public static ColorScheme HackerScheme { get; private set; }
        public static ColorScheme InputScheme { get; private set; }

        public static void Initialize()
        {
            HackerScheme = new ColorScheme() { Normal = Application.Driver.MakeAttribute(Color.Green, Color.Black), Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray) };
            InputScheme = new ColorScheme() { Normal = Application.Driver.MakeAttribute(Color.Black, Color.Gray), Focus = Application.Driver.MakeAttribute(Color.White, Color.DarkGray) };
        }
    }
}

