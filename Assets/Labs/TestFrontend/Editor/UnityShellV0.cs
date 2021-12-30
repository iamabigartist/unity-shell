using Labs.TestEvaluator;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
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

        void CreateGUI()
        {
            add_object = new Object();
            m_evaluator = new MyEvaluator();
            m_visualTreeAsset.CloneTree( rootVisualElement );
            rootVisualElement.Bind( new SerializedObject( this ) );
            rootVisualElement.Q<VisualElement>( "Input" ).Q<TextField>().RegisterCallback<KeyUpEvent>( (e) =>
            {
                var input_field = e.currentTarget as TextField;
                if (!(e.keyCode == KeyCode.Return && e.ctrlKey)) { return; }
                logs += "> " + input_field!.value + "\n";
                var result = m_evaluator.Compile( input_field.value );
                logs += result == null ? "" : "> " + result + "\n";
                variables = m_evaluator.evaluator.GetVars();
                input_field.value = string.Empty;
            } );

            var add_input = rootVisualElement.Q<VisualElement>( "AddInput" );
            add_input.Q<Button>().clicked += () =>
            {
                var cur_type = add_object.GetType();
                cur_add_object = add_object;
                var command = $"var {add_object_name.Replace( ' ', '_' )} = Labs.TestFrontend.UnityShellV0.cur_add_object as {cur_type};";
                m_evaluator.evaluator.Run( command );
                variables = m_evaluator.evaluator.GetVars();
            };

        }

    }
}
