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
        public static async Task<T> Evaluate<T>(string sourceCode, object o)
        {
            T s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: o);
            return s;
        }
        //public static async Task<T> Evaluate<T>(string sourceCode)
        //{
        //    T s = await CSharpScript.EvaluateAsync<T>(sourceCode);
        //    return s;
        //}
        public static async Task<T> Evaluate<T>(string sourceCode)
        {
            var new_sourceCode = string.Empty;

            if(sourceCode.Contains('#'))
            {
                var hashtag_split_strings = sourceCode.Split('#').ToList();
                foreach (var split_string in hashtag_split_strings)
                {
                    if (split_string is "")
                    {
                        continue;
                    }

                    var newLine_split_strings = split_string.Split('\n').ToList();
                    newLine_split_strings.ForEach(newLine_split_string =>
                    {
                        new_sourceCode += newLine_split_string.Insert(newLine_split_string.Length, ";");
                    });
                }
            }
            else
            {
                new_sourceCode = sourceCode;                
            }

            Globals globals = new Globals();

            T s = await CSharpScript.EvaluateAsync<T>(new_sourceCode, null, globals);
            return s;
        }
        public static async Task<T> _Evaluate<T>(string sourceCode)
        {
            //IGNORE EMPTY STRINGS -> ""
            //IGNORE CLASS DECLARATIONS -> class
            //IGNORE SCOPE OPERATORS -> { }

            var new_sourceCode = string.Empty;
            var newLine_split_code = sourceCode.Split('\n');
            foreach(var newLine_string in newLine_split_code)
            {
                var newLine = newLine_string;
            }            

            T s = await CSharpScript.EvaluateAsync<T>(new_sourceCode);
            return s;
        }

        public static void SIMPLE_Execute(string sourceCode)
        {
            var new_sourceCode = sourceCode.Replace('\n', ';');
            var _script = CSharpScript.Create(new_sourceCode);
            _script?.Compile();
            _script?.RunAsync();
        }
        public static void AUTOFIX_Execute(string sourceCode)
        {
            var _script = CSharpScript.Create(sourceCode);
            var new_sourceCode = sourceCode;

            try
            {
                _script = CSharpScript.Create(new_sourceCode);
                _script?.Compile();
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
                var error_strings = new List<string>();
                diagnostics.ForEach(diagnostic =>
                {
                    var suppression_info = diagnostic.GetSuppressionInfo(compilation);
                    var suppression_attribute = suppression_info.Attribute;
                    var application_syntax_reference = suppression_attribute.ApplicationSyntaxReference;
                    var syntax = application_syntax_reference.GetSyntax();
                    var full_span = syntax.FullSpan.ToString();
                    var code = full_span;


                    #region OLD
                    //var info = diagnostic.ToString();
                    //var info_tuple = info.Substring(0, 6);
                    //var trimmed_info = info_tuple.Trim('(', ')');
                    //var split_info = trimmed_info.Split(',');
                    //var error_index = Int32.Parse(split_info[1]);

                    //var start = diagnostic.Location.SourceSpan.Start;
                    //var end = diagnostic.Location.SourceSpan.End;
                    //var length = diagnostic.Location.SourceSpan.Length;
                    //var error_substring = new_sourceCode.Substring(start, length);
                    //error_strings.Add(error_substring);


                    //var error_location = diagnostic.Location.SourceSpan.Start;
                    //if (new_sourceCode[error_location] == ')')
                    //{
                    //    error_location++;
                    //}
                    //new_sourceCode = new_sourceCode.Insert(error_index-1, ";");
                    //var code = new_sourceCode;
                    //new_sourceCode = new_sourceCode.Insert(error_location, ";"); 
                    #endregion
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

                _script = CSharpScript.Create(new_sourceCode);
                _script?.Compile();
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