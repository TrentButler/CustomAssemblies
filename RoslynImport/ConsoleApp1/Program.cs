using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        #region CodeStrings
        //string assembly_path = "C:\\Users\\Redtrent\\Documents\\GitHub\\Roslyn-Import\\Practice\\ALibrary\\bin\\Debug";

        //string code = "var lib = new Class1(); System.Console.WriteLine(\"DO WORK \" + lib.ToString()); System.Console.ReadLine();";
        //RoslynCompiler.RoslynWrapper.Execute<object>(code, assembly_path, new List<string>() { "ALibrary.dll" });

        //string code = "var stuff = 1" +
        //    "System.Console.WriteLine(\"DO WORK \")" +
        //    "System.Console.ReadLine()";


        

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

        //string code =
        //    "var a = Vector3.up;" +
        //    "System.Console.WriteLine(a.ToString());" +
        //    "System.Console.ReadLine();";

        ////OPEN A COMMAND PROMPT INSTANCE
        ////CHANGE THE WORKING DIRECTORY TO WHERE THE 'PYTHON STEERING BEHAVIOR' APPLICATION EXISTS
        ////START THE 'PYTHON STEERING BEHAVIOUR' APPLICATION
        //TO BE USED WITH RoslynCompiler.RoslynWrapper.Execute(string, List<Type>, List<string>)
        //string code = 
        //"string run_app = @\"/C app.py\";" +
        //"var process = new System.Diagnostics.Process();" + 
        //"process.StartInfo.FileName = @\"cmd.exe\";" +
        //"process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;" + 
        //"process.StartInfo.UseShellExecute = false;" + 
        //"process.StartInfo.WorkingDirectory = @\"C:\\Users\\s178061\\Downloads\\PythonIntro-master\\PythonIntro-master\\Projects\";" +
        //"process.StartInfo.Arguments = run_app;" +
        //"process.Start();";
        #endregion

        static void Main(string[] args)
        {
            string code = "public class CustomClass\n" +
                "{\n" +
                "public int data {get {return 1} set {data = value}}\n" +
                "public string myData {get set}\n" +
                "public string someData\n" +
                "{\n" +
                "get { return myData }\n" +
                "set { myData = value }\n" +
                "}\n" +
                "public CustomClass(string data)\n" +
                "{\n" +
                "myData = data\n" +
                "}\n" +
                "}\n" +
                "public string myFunc(string output)\n" +
                "{\n" +
                "return output + \"myFunc\"\n" +
                "}\n" +
                "var output = myFunc(\"INVOKE \")\n" +
                "System.Console.WriteLine(output)\n" +
                "var custom_class = new CustomClass(\"Hello Roslyn\")\n" +
                "System.Console.WriteLine(custom_class.myData)\n" +
                "System.Console.ReadLine()\n" +
                "return output\n";

            var result = RoslynCompiler.RoslynWrapper.Evaluate<string>(code);
            var made_it = result.Result;
            //RoslynCompiler.RoslynWrapper.Execute(code);

        }
    }
}
