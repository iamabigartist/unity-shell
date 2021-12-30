using System;
using System.Linq;
using Mono.CSharp;
namespace Labs.TestEvaluator
{
    public class MyEvaluator
    {

        enum CommandType
        {
            /// <summary>
            ///     Simply run the given code
            /// </summary>
            Run,
            /// <summary>
            ///     Try to evaluate a result from the given code
            /// </summary>
            EvaluateExpression
        }

        public Evaluator evaluator;
        public MyEvaluator()
        {
            InitEvaluator();
        }

        void InitEvaluator()
        {
            evaluator = new Evaluator( new CompilerContext( new CompilerSettings(), new ConsoleReportPrinter() ) );
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach( asm =>
            {
                evaluator.ReferenceAssembly( asm );
            } );
            evaluator.Run( "using UnityEngine; using UnityEditor; using System; using System.Collections.Generic;" );
        }

        public object Compile(string command)
        {
            if (evaluator == null)
            {
                return null;
            }

            if (!command.EndsWith( ";" ))
            {
                command += ";";
            }

            evaluator.Compile( command, out var compilationMethod );
            if (compilationMethod == null)
            {
                return "No result";
            }

            object result = null;
            compilationMethod( ref result );
            return result;
        }

        public string EvaluateExpression(string expression)
        {

            if (!expression.EndsWith( ";" ))
            {
                expression += ";";
            }
            try
            {
                evaluator.Evaluate( expression, out var result, out bool resultSet );
                string result_s = result == null ? "No result." : result.ToString();
                return result_s;
            }
            catch (ArgumentException e)
            {
                return "Incomplete expression or statement! Can not compile.";
            }

        }
    }
}
