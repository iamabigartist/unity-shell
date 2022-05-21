using UnityEditor;
using UnityEngine;
namespace Labs.TestSourceGen.Editor
{
	public class TestMySourceGen : EditorWindow
	{
		[MenuItem("Labs/Labs.TestSourceGen.Editor/TestMySourceGen")]
		static void ShowWindow()
		{
			var window = GetWindow<TestMySourceGen>();
			window.titleContent = new("TestMySourceGen");
			window.Show();
		}

		void OnEnable()
		{
			HelloWorldGenerated.HelloWorld.SayHello();
		}

		void OnGUI()
		{

		}
	}
}