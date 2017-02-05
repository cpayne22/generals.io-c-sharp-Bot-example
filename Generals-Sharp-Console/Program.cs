using Generals_Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generals_Sharp_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var gs = new Generals_Sharp.Main();
            gs.OnDisconnect += OnDisconnect;
            gs.OnLog += OnLog;
            gs.Initialise();

            Console.ReadLine();
        }

        private static void OnLog(object sender, EventArgs e)
        {
            var l = (Logging)e;
            Console.WriteLine(l.Message);
        }

        private static void OnDisconnect(object sender, EventArgs e)
        {
            Console.WriteLine("Disconnected");
        }
    }
}
