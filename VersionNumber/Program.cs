using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace VersionNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            FileVersionInfo file1 = FileVersionInfo.GetVersionInfo(@"C:\Users\serzh\VirtualBox VMs\mutualSSD\CalanderPresentation\CalanderPresentation.exe");
            Console.WriteLine(file1.FileVersion.ToString());
            Console.ReadLine();
        }
    }
}
