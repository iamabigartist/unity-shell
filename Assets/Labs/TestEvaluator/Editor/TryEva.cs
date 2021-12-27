using System;
using System.Collections.Generic;
using System.Linq;
using Mono.CSharp;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Labs.TestEvaluator
{
    public class TryEva : EditorWindow
    {

        public static SerializedProperty cur_property;
        public static SerializedObject cur_object;

        public static void Aaaa() { }

        [MenuItem( "Tests/TryEva" )]
        static void ShowWindow()
        {
            var window = GetWindow<TryEva>();
            window.titleContent = new GUIContent( "TryEva" );
            window.Show();
        }


        Evaluator evaluator;
        CompilerContext compilerContext;
        CompilerSettings compilerSettings;
        ConsoleReportPrinter consoleReportPrinter;

        void OnEnable()
        {
            var a = new GameObject();

            compilerSettings = new CompilerSettings();
            consoleReportPrinter = new ConsoleReportPrinter();
            compilerContext = new CompilerContext( compilerSettings, consoleReportPrinter );
            compilerContext.Report.EnableReporting();
            evaluator = new Evaluator( compilerContext );
            AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach( asm =>
            {
                evaluator.ReferenceAssembly( asm );
            } );

            evaluator.Run( @"
using UnityEngine; using UnityEditor; using System.Collections.Generic;using Mono.CSharp;using Labs.TestEvaluator;" );
            Debug.Log( evaluator.GetUsing() );

            this_ = new SerializedObject( this );
            cur_obj_ = this_.FindProperty( nameof(cur_obj) );

            cur_object = this_;
            lista = new List<int>();

            object result = null;
            evaluator.Compile( "TryEva.cur_property = TryEva.cur_object.FindProperty( \"lista\" );" )( ref result );
            // cur_property = cur_object.FindProperty( "lista" );
        }



        [SerializeField]
        List<int> lista { get; set; }

        SerializedObject this_;
        SerializedProperty cur_obj_;
        [SerializeField]
        Object cur_obj;
        Type cur_type;
        Editor cur_editor;

        void OnGUI()
        {
            EditorGUILayout.LabelField( string.Join( ",", lista ) );
            EditorGUILayout.PropertyField( cur_property );
            cur_property.serializedObject.ApplyModifiedProperties();
            GUILayout.FlexibleSpace();
            EditorGUILayout.PropertyField( cur_obj_ );
            bool flag = this_.hasModifiedProperties;
            if (flag)
            {
                this_.ApplyModifiedProperties();
                cur_type = cur_obj.GetType();
                cur_editor = Editor.CreateEditor( cur_obj );
            }

            EditorGUILayout.LabelField( $"{cur_type}" );
            if (cur_editor != null)
            {
                cur_editor.DrawDefaultInspector();
                cur_editor.OnInspectorGUI();
            }
        }
    }

    namespace AAA
    {
        [Serializable]
        public class AnimalBBB
        {
            public bool aswecan = false;
        }
    }
}
