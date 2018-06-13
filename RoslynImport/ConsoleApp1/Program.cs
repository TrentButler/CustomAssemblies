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

            string code = "var stuff = 1" +
                "System.Console.WriteLine(\"DO WORK \")" +
                "System.Console.ReadLine()";


            //string code = "public class CustomClass {" +
            //    "public string myData " +
            //    "public CustomClass(string data) {myData = data} }" +
            //    "var custom_class = new CustomClass(\"Hello Roslyn\")" +
            //    "System.Console.WriteLine(custom_class.myData)" +
            //    "System.Console.ReadLine()";


            RoslynCompiler.RoslynWrapper.Execute(code);
        }
    }
}
