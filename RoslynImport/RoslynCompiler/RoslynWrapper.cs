using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

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
            catch (Microsoft.CodeAnalysis.Scripting.CompilationErrorException e) when (e.Message.Contains("error CS1002: ; expected"))
            {
                var compilation = _script?.GetCompilation();
                var diagonstic_list = compilation.GetDiagnostics().ToList();
                diagonstic_list.ForEach(diagonsitc =>
                {
                    var error_location = diagonsitc.Location.SourceSpan.Start;
                    new_sourceCode = new_sourceCode.Insert(error_location, ";");
                });
            }

            //ADDS SEMICOLON TO FIRST ERROR, THEN FALLS OUT OF METHOD

            Console.WriteLine(new_sourceCode);
            Console.ReadKey();

            //CSharpScript.RunAsync(sourceCode);
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