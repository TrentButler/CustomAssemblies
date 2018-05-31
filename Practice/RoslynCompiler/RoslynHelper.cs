using System;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace RoslynCompiler
{
    public static class RoslynHelper : Object
    {
        public static async Task<object> Evaluate<T>(string sourceCode, object o)
        {
            object s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: o);
            return s;
        }

    }
}
