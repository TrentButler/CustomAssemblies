using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string code = "System.Console.WriteLine(\"DO WORK\")";
            RoslynCompiler.RoslynWrapper.Execute(code);
            Console.ReadLine();
        }
    }
}
