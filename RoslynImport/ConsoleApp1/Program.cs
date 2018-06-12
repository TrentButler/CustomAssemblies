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
            //string assembly_path = "C:\\Users\\Redtrent\\Documents\\GitHub\\Roslyn-Import\\Practice\\ALibrary\\bin\\Debug";

            //string code = "var lib = new Class1(); System.Console.WriteLine(\"DO WORK \" + lib.ToString()); System.Console.ReadLine();";
            //RoslynCompiler.RoslynWrapper.Execute<object>(code, assembly_path, new List<string>() { "ALibrary.dll" });

            string code = "System.Console.WriteLine(\"DO WORK \")System.Console.ReadLine()";
            RoslynCompiler.RoslynWrapper.Execute(code);
        }
    }
}
