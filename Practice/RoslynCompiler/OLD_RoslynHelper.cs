using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace OLD_RoslynCompiler
{
    public static class OLD_RoslynHelper : Object
    {
        public static async Task<T> Evaluate<T>(string sourceCode, object o)
        {
            T s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: o);
            return s;
        }

        public static void Execute(string sourceCode)
        {
            CSharpScript.RunAsync(sourceCode);
        }
    }
}
