using System;
using System.IO;

namespace Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            //NEEDS WORK
            //CLASS 'State' AND 'Transition' MUST EXIST IN ASSEMBLY???
            //EXCEPTION THROWN 'The type or namespace name 'Transition' could not be found (are you missing a using directive or an assembly reference?'

            var roslynstring = "var t = new Transition(new State(), new State());";
            var evauluate_this = RoslynHelper.Evaluate(roslynstring);
            var result = evauluate_this.Result as Transition;
            
            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }
    }
}
