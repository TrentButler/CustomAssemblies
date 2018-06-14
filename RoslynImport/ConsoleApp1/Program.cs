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

            //string code = "var stuff = 1" +
            //    "System.Console.WriteLine(\"DO WORK \")" +
            //    "System.Console.ReadLine()";


            //string code = "public class CustomClass\n" +
            //    "{\n" +
            //    "public string myData\n" +
            //    "public CustomClass(string data)\n" +
            //    "{\n" +
            //    "myData = data\n" +
            //    "}\n" +
            //    "}\n" +
            //    "var custom_class = new CustomClass(\"Hello Roslyn\")\n" +
            //    "System.Console.WriteLine(custom_class.myData)\n" +
            //    "System.Console.ReadLine()\n";

            //string code = "#" +
            //    "var a = \"hello\"\n" +
            //    "var b = \"world\"\n" +
            //    "System.Console.WriteLine((a + b).Length)\n" +
            //    "System.Console.ReadLine()" +
            //    "#";

            //string code =
            //    "var a = \"hello\";" +
            //    "var b = \"world\";" +
            //    "System.Console.WriteLine((a + b).Length);" +
            //    "System.Console.ReadLine();";

            string code =
                "var a = Vector3.up;" +
                "System.Console.WriteLine(a.ToString());" +
                "System.Console.ReadLine();";

            var result = RoslynCompiler.RoslynWrapper.Evaluate<object>(code).Result;
            //RoslynCompiler.RoslynWrapper.SIMPLE_Execute(code);
        }
    }
}
