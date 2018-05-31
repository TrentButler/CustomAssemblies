using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace Practice
{
    public static class RoslynHelper : Object
    {
        public static async Task<object> Evaluate(string sourceCode)
        {
            object s = await CSharpScript.EvaluateAsync(sourceCode);
            return s;
        }

        public static async Task<int> Evaluate_INT(string sourceCode)
        {
            int s = await CSharpScript.EvaluateAsync<int>(sourceCode);
            return s;
        }
    }
}
