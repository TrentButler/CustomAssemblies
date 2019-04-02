using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.CodeAnalysis.Emit;

namespace RoslynCompiler
{
    public class RoslynWrapper
    {
        private string _target_code;
        private string _output_code;

        public RoslynWrapper()
        {
            this._target_code = string.Empty;
            this._output_code = string.Empty;
        }
        

        private void ParseSourceCode()
        {
            var sourceCode = this._target_code;
            var add_hashtag_start = sourceCode.Insert(0, "#");
            var add_hashtag_end = add_hashtag_start.Insert(add_hashtag_start.Length, "\n#");
            var start_sourceCode = add_hashtag_end;
            var new_sourceCode = "";

            var split_by_hastag = start_sourceCode.Split('#').ToList();
            foreach (var hastag_split in split_by_hastag)
            {
                if (hastag_split is "")
                {
                    continue;
                }

                var newLine_split_code = hastag_split.Split('\n').ToList();
                for (int i = 0; i < newLine_split_code.Count; i++)
                {
                    if (newLine_split_code.Contains(";"))
                    {
                        new_sourceCode += newLine_split_code;
                        continue;
                    }

                    if (newLine_split_code[i] == "") //IGNORE EMPTY STRINGS -> ""
                    {
                        continue;
                    }

                    if (newLine_split_code[i].StartsWith("//")) //IGNORE STRINGS THAT START WITH "//"
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    if (newLine_split_code[i].Contains(" class ")) //IGNORE CLASS DECLARATIONS -> class
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    if (newLine_split_code[i].Contains("get") || newLine_split_code[i].Contains("set"))
                    {
                        var needs_format = newLine_split_code[i];
                        var scopeOperator_split = newLine_split_code[i].Split('{', '}');
                        foreach (var split in scopeOperator_split)
                        {
                            if (split == "")
                            {
                                continue;
                            }

                            if (split.Contains("return"))
                            {
                                var proptery_return = split.Insert(split.Length, ";");
                                needs_format = needs_format.Replace(split, proptery_return);
                                continue;
                            }
                            if (split.Contains("="))
                            {
                                var proptery_assignment = split.Insert(split.Length, ";");
                                needs_format = needs_format.Replace(split, proptery_assignment);
                                continue;
                            }
                            if (split.Contains("get") && split.Contains("set"))
                            {
                                var auto_proptery_get = "get;";
                                var auto_propter_set = "set;";
                                needs_format = needs_format.Replace("get", auto_proptery_get);
                                needs_format = needs_format.Replace("set", auto_propter_set);
                                continue;
                            }
                        }

                        new_sourceCode += needs_format;
                        continue;
                    }

                    if (newLine_split_code[i].Contains("{") || newLine_split_code[i].Contains("}")) //IGNORE SCOPE OPERATORS -> { }
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    else
                    {
                        var next_line = newLine_split_code[i + 1];
                        if (next_line != null)
                        {
                            if (next_line.Contains("{"))
                            {
                                new_sourceCode += newLine_split_code[i];
                                continue;
                            }
                            else
                            {
                                var needs_format = newLine_split_code[i];
                                needs_format = needs_format.Insert(needs_format.Length, ";");
                                new_sourceCode += needs_format;
                                continue;
                            }
                        }
                    }
                }
            }

            this._output_code = new_sourceCode;
        }

        private static Thread ParseCode(string sourceCode)
        {
            //this._target_code = sourceCode;
            //var child_thread = new ThreadStart(ParseSourceCode);
            //var thread = new Thread(child_thread);
            //thread.Start();

            var thread = new Thread(() => ParseSourceCode(ref sourceCode));
            thread.Start();
            return thread;
        }

        public async Task<T> Evaluate<T>(string sourceCode, List<Type> assembly_references, List<string> imports)
        {
            ScriptOptions options = ScriptOptions.Default;

            assembly_references.ForEach(refrence =>
            {
                options = options.AddReferences(new List<Assembly>() { refrence?.Assembly });
            });

            options.AddImports(imports);

            //this.ParseCode(sourceCode);

            T s = await CSharpScript.EvaluateAsync<T>(this._output_code, options);
            return s;
        }

        public void Execute(string sourceCode, List<Type> assembly_references, List<string> imports)
        {
            ScriptOptions options = ScriptOptions.Default;

            assembly_references.ForEach(refrence =>
            {
                options = options.AddReferences(new List<Assembly>() { refrence?.Assembly });
            });

            options.AddImports(imports);

            //this.ParseCode(sourceCode);

            CSharpScript.RunAsync(this._output_code, options);
        }

        #region STATIC_METHODS
        private static void ParseSourceCode(ref string source)
        {
            var sourceCode = source as string;
            var add_hashtag_start = sourceCode.Insert(0, "#");
            var add_hashtag_end = add_hashtag_start.Insert(add_hashtag_start.Length, "\n#");
            var start_sourceCode = add_hashtag_end;
            var new_sourceCode = "";

            var split_by_hastag = start_sourceCode.Split('#').ToList();
            foreach (var hastag_split in split_by_hastag)
            {
                if (hastag_split is "")
                {
                    continue;
                }

                var newLine_split_code = hastag_split.Split('\n').ToList();
                for (int i = 0; i < newLine_split_code.Count; i++)
                {
                    if (newLine_split_code.Contains(";"))
                    {
                        new_sourceCode += newLine_split_code;
                        continue;
                    }

                    if (newLine_split_code[i] == "") //IGNORE EMPTY STRINGS -> ""
                    {
                        continue;
                    }

                    if (newLine_split_code[i].StartsWith("//")) //IGNORE STRINGS THAT START WITH "//"
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    if (newLine_split_code[i].Contains(" class ")) //IGNORE CLASS DECLARATIONS -> class
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    if (newLine_split_code[i].Contains("get") || newLine_split_code[i].Contains("set"))
                    {
                        var needs_format = newLine_split_code[i];
                        var scopeOperator_split = newLine_split_code[i].Split('{', '}');
                        foreach (var split in scopeOperator_split)
                        {
                            if (split == "")
                            {
                                continue;
                            }

                            if (split.Contains("return"))
                            {
                                var proptery_return = split.Insert(split.Length, ";");
                                needs_format = needs_format.Replace(split, proptery_return);
                                continue;
                            }
                            if (split.Contains("="))
                            {
                                var proptery_assignment = split.Insert(split.Length, ";");
                                needs_format = needs_format.Replace(split, proptery_assignment);
                                continue;
                            }
                            if (split.Contains("get") && split.Contains("set"))
                            {
                                var auto_proptery_get = "get;";
                                var auto_propter_set = "set;";
                                needs_format = needs_format.Replace("get", auto_proptery_get);
                                needs_format = needs_format.Replace("set", auto_propter_set);
                                continue;
                            }
                        }

                        new_sourceCode += needs_format;
                        continue;
                    }

                    if (newLine_split_code[i].Contains("{") || newLine_split_code[i].Contains("}")) //IGNORE SCOPE OPERATORS -> { }
                    {
                        new_sourceCode += newLine_split_code[i];
                        continue;
                    }

                    else
                    {
                        var next_line = newLine_split_code[i + 1];
                        if (next_line != null)
                        {
                            if (next_line.Contains("{"))
                            {
                                new_sourceCode += newLine_split_code[i];
                                continue;
                            }
                            else
                            {
                                var needs_format = newLine_split_code[i];
                                needs_format = needs_format.Insert(needs_format.Length, ";");
                                new_sourceCode += needs_format;
                                continue;
                            }
                        }
                    }
                }
            }

            source = new_sourceCode;
        }

        public static async Task<T> Evaluate<T>(string sourceCode, object globals)
        {
            T s = await CSharpScript.EvaluateAsync<T>(sourceCode, globals: globals);
            return s;
        }
        public static async Task<T> Evaluate<T>(string sourceCode)
        {
            var new_sourceCode = sourceCode;

            //MULTI-THREAD THIS
            var t = ParseCode(new_sourceCode);

            Globals globals = new Globals();
            T s = await CSharpScript.EvaluateAsync<T>(new_sourceCode, null, globals);
            return s;
        }
        public static async Task<T> Evaluate<T>(string sourceCode, object globals, List<Type> assembly_references, List<string> imports)
        {
            ScriptOptions options = ScriptOptions.Default;

            assembly_references.ForEach(refrence =>
            {
                options = options.AddReferences(new List<Assembly>() { refrence?.Assembly });
            });

            options.AddImports(imports);

            var new_sourceCode = sourceCode;

            //MULTI-THREAD THIS
            //ParseSourceCode(ref new_sourceCode);
            var t = ParseCode(new_sourceCode);

            T s = await CSharpScript.EvaluateAsync<T>(new_sourceCode, options, globals);
            return s;
        }

        public static void Execute(string sourceCode)
        {
            var new_sourceCode = sourceCode;

            //MULTI-THREAD THIS
            ParseSourceCode(ref new_sourceCode);

            Globals globals = new Globals();

            CSharpScript.RunAsync(new_sourceCode, null, globals);
        }
        public static void Execute(string sourceCode, object globals, List<Type> assembly_references, List<string> imports)
        {
            ScriptOptions options = ScriptOptions.Default;

            assembly_references.ForEach(refrence =>
            {
                options = options.AddReferences(new List<Assembly>() { refrence?.Assembly });
            });

            options.AddImports(imports);

            var new_sourceCode = sourceCode;

            //MULTI-THREAD THIS
            ParseSourceCode(ref new_sourceCode);

            CSharpScript.RunAsync(new_sourceCode, options, globals);
        } 
        #endregion

        #region EXPERMENTAL
        public static void SIMPLE_AUTOFIX_Execute(string sourceCode)
        {
            var new_sourceCode = sourceCode.Replace('\n', ';');
            var _script = CSharpScript.Create(new_sourceCode);
            _script?.Compile();
            _script?.RunAsync();
        }
        public static void CODEANALYSIS_AUTOFIX_Execute(string sourceCode)
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
        public static void ASSEMBLY_LOAD_Execute<T>(string sourceCode, string assembly_path, List<string> assemblies)
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
        #endregion

        #region OLD
        //public static async Task<T> Evaluate<T>(string sourceCode)
        //{
        //    var new_sourceCode = string.Empty;

        //    if(sourceCode.Contains('#'))
        //    {
        //        var hashtag_split_strings = sourceCode.Split('#').ToList();
        //        foreach (var split_string in hashtag_split_strings)
        //        {
        //            if (split_string is "")
        //            {
        //                continue;
        //            }

        //            var newLine_split_strings = split_string.Split('\n').ToList();
        //            newLine_split_strings.ForEach(newLine_split_string =>
        //            {
        //                new_sourceCode += newLine_split_string.Insert(newLine_split_string.Length, ";");
        //            });
        //        }
        //    }
        //    else
        //    {
        //        new_sourceCode = sourceCode;                
        //    }

        //    Globals globals = new Globals();

        //    T s = await CSharpScript.EvaluateAsync<T>(new_sourceCode, null, globals);
        //    return s;
        //} 
        #endregion
    }
}