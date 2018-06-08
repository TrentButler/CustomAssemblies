using System;
using System.IO;
using OLD_RoslynCompiler;
namespace Practice
{
    public class Program
    {
        static void Main(string[] args)
        {
            //NEEDS WORK
            //CLASS 'State' AND 'Transition' MUST EXIST IN ASSEMBLY???
            //EXCEPTION THROWN 'The type or namespace name 'Transition' could not be found (are you missing a using directive or an assembly reference?'
            //var globals = new Globals
            //{
            //    A = new Globals.State(),
            //    B = new Globals.State(),
            //    T = new Globals.Transition()
            //};

            //var newStateA = "a = new State(\"MY STATE A\")";
            //var newStateB = "b = new State(\"MY STATE B\")";
            //var stringresult = "T = new Transition(A, B)";
            //var runit = RoslynHelper.Evaluate<Globals.Transition>(stringresult, globals);
            //Console.WriteLine(runit.Result);

            var code = "System.Console.WriteLine(\"EXECUTE THIS CODE\")";

            OLD_RoslynHelper.Execute(code);
            Console.ReadLine();
        }
    }
}
