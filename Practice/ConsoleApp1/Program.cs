using System;
using System.IO;
using RoslynCompiler;
using Microsoft.CodeAnalysis.CSharp.Scripting;
namespace Practice
{

    public class GlobalsXY
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            //NEEDS WORK
            //CLASS 'State' AND 'Transition' MUST EXIST IN ASSEMBLY???
            //EXCEPTION THROWN 'The type or namespace name 'Transition' could not be found (are you missing a using directive or an assembly reference?'
            var globals = new Globals
            {
                A = new Globals.State(),
                B = new Globals.State(),
                T = new Globals.Transition()
            };

            var stringresult = "T = new Transition(A, B)";
            var globalss = new GlobalsXY { X = 1, Y = 2 };
            var runit = RoslynHelper.Evaluate<Globals.Transition>(stringresult, globals);
            Console.WriteLine(runit.Result);
            Console.ReadLine();

        }
    }
}
