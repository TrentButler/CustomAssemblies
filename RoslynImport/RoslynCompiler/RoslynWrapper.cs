using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynCompiler
{
    public class RoslynWrapper : Object
    {
        private static Script _script;

        public static async Task<T> Evaluate<T>(string sourceCode, object o)
        {
            T s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: o);
            return s;
        }

        private static void CreateScript(string sourceCode)
        {
            _script = CSharpScript.Create(sourceCode);
        }
        public static void CompileScript(string sourceCode)
        {
            CreateScript(sourceCode);
            _script?.Compile();
        }
        public static async void RunScript()
        {
            var run = await _script?.RunAsync();
        }

        public static void Execute(string sourceCode)
        {
            CreateScript(sourceCode);
            var new_sourceCode = sourceCode;

            try
            {
                CompileScript(new_sourceCode);
                _script?.RunAsync();
            }
            catch (CompilationErrorException e) when (e.Message.Contains("error CS1002: ; expected"))
            {
                var compilation = _script?.GetCompilation();

                var diagonstic_list = compilation.GetDiagnostics().ToList();
                Stream code_stream = Stream.Null;
                var emit_result = compilation.Emit(code_stream);
                var stuff = emit_result.Diagnostics;

                var diagnostics = emit_result.Diagnostics.ToList();
                diagnostics.ForEach(diagnostic =>
                {
                    //var info = diagnostic.ToString();

                    //var start = diagnostic.Location.SourceSpan.End;
                    //var length = diagnostic.Location.SourceSpan.Length;
                    //var error_substring = new_sourceCode.Substring(start, length);
                    //Console.WriteLine(error_substring + "\n");

                    var error_location = diagnostic.Location.SourceSpan.Start;
                    if (new_sourceCode[error_location] == ')')
                    {
                        error_location++;
                    }
                    new_sourceCode = new_sourceCode.Insert(error_location, ";");
                });

                #region OLD
                //diagonstic_list.ForEach(diagnostic =>
                //{
                //    var source_span = diagnostic.Location.SourceSpan;
                //    var line_span = diagnostic.Location.GetLineSpan().Span;
                //    var start = source_span.Start;
                //    var length = source_span.Length;
                //    var end = source_span.End;

                //    //var invald_string = new_sourceCode.Substring(start, length);
                //    //Console.WriteLine(invald_string);
                //    //Console.ReadLine();

                //    var error_location = diagnostic.Location.SourceSpan.Start;
                //    if (new_sourceCode[error_location] == ')')
                //    {
                //        error_location++;
                //    }
                //    new_sourceCode = new_sourceCode.Insert(error_location, ";");
                //    var code = new_sourceCode;
                //}); 
                #endregion

                CompileScript(new_sourceCode);
                _script?.RunAsync();
            }
        }
        public static void Execute<T>(string sourceCode, string assembly_path, List<string> assemblies)
        {
            var prev_directory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(assembly_path);

            var _assemblies = new List<Assembly>();
            var _imports = new List<string>();
            var current_app_domain = AppDomain.CurrentDomain;

            using (var assembly_loader = new InteractiveAssemblyLoader())
            {
                assemblies.ForEach(assembly =>
                {
                    var assembly_fullName = AssemblyName.GetAssemblyName(assembly_path + "\\" + assembly);
                    var _assembly = current_app_domain.Load(assembly_fullName);
                    var modules = _assembly.GetModules(true).ToList();
                    modules.ForEach(module =>
                    {
                        var raw_module_str = module.Name;
                        var formatted_module_str = raw_module_str.Remove(raw_module_str.Length - 4);

                        _imports.Add(formatted_module_str);
                    });

                    _assemblies.Add(_assembly);
                });

                _assemblies.ForEach(assembly =>
                {
                    assembly_loader.RegisterDependency(assembly);
                });

                var script_options = ScriptOptions.Default;
                _imports.ForEach(import =>
                {
                    script_options = script_options.AddImports(import);
                });

                script_options = script_options.AddImports("System");

                Directory.SetCurrentDirectory(prev_directory);

                var script = CSharpScript.Create<T>(sourceCode, script_options, null, assembly_loader);
                script.RunAsync();
            }

            //CSharpScript.RunAsync(sourceCode);
        }
    }
}