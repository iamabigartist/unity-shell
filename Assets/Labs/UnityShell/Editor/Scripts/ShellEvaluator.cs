using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mono.CSharp;
namespace UnityShell
{
    [Serializable]
    public class ShellEvaluator
    {
        public string cur_prefix;
        public string[] completions;

        int handleCount;

        public Evaluator evaluator;

        public ShellEvaluator()
        {
            new Thread( InitializeEvaluator ).Start();
        }

        void InitializeEvaluator()
        {
            evaluator = new Evaluator( new CompilerContext( new CompilerSettings(), new ConsoleReportPrinter() ) );
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach( asm =>
            {
                try
                {
                    evaluator.ReferenceAssembly( asm );
                }
                catch { }
            } );
            evaluator.Run( "using UnityEngine; using UnityEditor; using System; using System.Collections.Generic;" );
        }

        public void SetInput(string input)
        {
            if (evaluator == null)
            {
                return;
            }

            handleCount++;
            new Thread( () =>
            {
                int handle = handleCount;

                if (!string.IsNullOrEmpty( input ))
                {
                    var result = evaluator.GetCompletions( input, out cur_prefix );

                    // Avoid old threads overriding with old results
                    if (handle == handleCount)
                    {
                        completions = result;
                        if (completions == null)
                        {
                            return;
                        }
                        for (var i = 0; i < completions.Length; i++)
                        {
                            completions[i] = input + completions[i];
                        }

                        if (completions.Length == 1 && completions[0].Trim() == input.Trim())
                        {
                            completions = new string[0];
                        }
                    }
                }
                else
                {
                    completions = new string[0];
                }
            } ).Start();
        }

        public object Evaluate(string command)
        {
            if (evaluator == null)
            {
                return null;
            }

            if (!command.EndsWith( ";" ))
            {
                command += ";";
            }

            var success = evaluator.Compile( command, out var compilationMethod );
            var num = success?.Length ?? -1;
            if (compilationMethod == null)
            {
                return "Incomplete expression or statement! Can not compile.";
            }

            object result = null;
            compilationMethod( ref result );
            string result_s = result == null ? "" : result.ToString();
            result = result_s + evaluator.GetVars();
            return result;
        }
    }
}
