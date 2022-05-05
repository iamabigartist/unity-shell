using System;
using Labs.TestEvaluator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
namespace Labs.TestFrontend
{
    public class UnityShellV0 : EditorWindow
    {
        [MenuItem( "Labs.Test1.Editor/UnityShellV0" )]
        static void ShowWindow()
        {
            var window = GetWindow<UnityShellV0>();
            window.titleContent = new GUIContent( "UnityShellV0" );
            window.Show();
        }

        [SerializeField]
        VisualTreeAsset m_visualTreeAsset;

        [SerializeField]
        string variables;
        [SerializeField]
        string logs;
        [SerializeField]
        Object add_object;
        [SerializeField]
        string add_object_name;
        public static Object cur_add_object;

        MyEvaluator m_evaluator;

        string[] completion_string_list;

        void CreateGUI()
        {
            add_object = new Object();
            m_evaluator = new MyEvaluator();
            m_visualTreeAsset.CloneTree( rootVisualElement );
            rootVisualElement.Bind( new SerializedObject( this ) );
            input_field = rootVisualElement.Q<VisualElement>( "Input" ).Q<TextField>();

            input_field.RegisterCallback<KeyUpEvent>( InputExecuteCallback );



            var add_input = rootVisualElement.Q<VisualElement>( "Tools" ).Q<VisualElement>( "AddInput" );
            add_input.Q<Button>().clicked += () =>
            {
                var cur_type = add_object.GetType();
                cur_add_object = add_object;
                var command = $"var {add_object_name.Replace( ' ', '_' )} = Labs.TestFrontend.UnityShellV0.cur_add_object as {cur_type};";
                m_evaluator.evaluator.Run( command );
                variables = m_evaluator.evaluator.GetVars();
            };

            m_completionList = rootVisualElement.Q<VisualElement>( "Tools" ).Q<ListView>( "Completion" );

            m_completionList.itemsSource = completion_string_list;
            m_completionList.makeItem = () => new Label();
            m_completionList.bindItem = (v, i) =>
            {
                ((Label)v).text = completion_string_list[i];
            };
            m_completionList.Rebuild();

            input_field.RegisterValueChangedCallback( (e) =>
            {
                var code = e.newValue;
                completion_string_list =
                    m_evaluator.evaluator.GetCompletions( code, out var prefix ) ??
                    Array.Empty<string>();
                // for (int i = 0; i < completion_string_list.Length; i++)
                // {
                //     completion_string_list[i] = prefix + completion_string_list[i];
                // }
                m_completionList.itemsSource = completion_string_list;
                m_completionList.Rebuild();
            } );

        }


        TextField input_field;
        ListView m_completionList;
        void InputExecuteCallback(KeyUpEvent e)
        {
            if (!(e.keyCode == KeyCode.Return && e.ctrlKey)) { return; }
            logs += "> " + input_field!.value + "\n";
            var result = m_evaluator.Compile( input_field.value );
            logs += result == null ? "" : "> " + result + "\n";
            variables = m_evaluator.evaluator.GetVars();
            input_field.value = string.Empty;
        }

    }
}
