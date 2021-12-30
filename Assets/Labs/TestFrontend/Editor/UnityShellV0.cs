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

        MyEvaluator m_evaluator;

        void CreateGUI()
        {
            m_evaluator = new MyEvaluator();
            m_visualTreeAsset.CloneTree( rootVisualElement );
            rootVisualElement.Bind( new SerializedObject( this ) );
            rootVisualElement.Q<VisualElement>( "Input" ).Q<TextField>().RegisterCallback<KeyUpEvent>( (e) =>
            {
                var input_field = e.currentTarget as TextField;
                if (!(e.keyCode == KeyCode.Return && e.ctrlKey)) { return; }
                logs += "> " + input_field!.value + "\n";
                logs += "> " + m_evaluator.Compile( input_field.value ) + "\n";
                variables = m_evaluator.evaluator.GetVars();
                input_field.value = string.Empty;
            } );
        }

    }
}
