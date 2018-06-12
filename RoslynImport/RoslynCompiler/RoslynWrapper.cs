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
        public static async Task<T> Evaluate<T>(string sourceCode, object o)
        {
            T s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: o);
            return s;
        }

        public static void Execute(string sourceCode)
        {
            //EXPECT NO SEMICOLONS 
            //ADD THEM WHERE THEY NEED TO BE




            CSharpScript.RunAsync(sourceCode);
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