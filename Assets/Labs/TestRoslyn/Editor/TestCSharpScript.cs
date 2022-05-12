using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using UnityEditor;
using UnityEngine;
namespace Labs.TestRoslyn.Editor
{
	public class TestCSharpScript : EditorWindow
	{
		[MenuItem("Labs/Labs.TestRoslyn.Editor/TestCSharpScript")]
		static void ShowWindow()
		{
			var window = GetWindow<TestCSharpScript>();
			window.titleContent = new("TestCSharpScript");
			window.Show();
		}

		ScriptOptions options;


		async void OnEnable()
		{
			var UnityEngineAssembly = typeof(Object).Assembly;

			var ref_list = new[]
			{
				UnityEngineAssembly,
				Assembly.Load(UnityEngineAssembly.GetReferencedAssemblies().First())
			};

			options = ScriptOptions.Default.AddReferences(ref_list);


			var script_state = await CSharpScript.RunAsync("using UnityEngine;", options);
			script_state = await script_state.ContinueWithAsync("var a = GameObject.FindObjectOfType<Camera>().transform.position.ToString();Debug.Log(a);", options);
			Debug.Log(string.Join(",", script_state.Variables.Select(v => $"{v.Type} {v.Name} = {v.Value}")));
			Debug.Log($"return: { script_state.ReturnValue}");
			script_state = await script_state.ContinueWithAsync("a", options);
			Debug.Log($"return: { script_state.ReturnValue}");

		}

		void OnGUI()
		{

		}
	}
}