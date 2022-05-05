using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using UnityEngine;
namespace Labs.TestRoslyn
{
	public class TestCSharpScriptMemoryLeak : MonoBehaviour
	{
		ScriptOptions options;
		void Start()
		{
			var UnityEngineAssembly = typeof(Object).Assembly;

			var ref_list = new[]
			{
				UnityEngineAssembly,
				Assembly.Load(UnityEngineAssembly.GetReferencedAssemblies().First())
			};

			options = ScriptOptions.Default.AddImports("UnityEngine").AddReferences(ref_list);
			Debug.Log(GameObject.FindObjectOfType<Camera>().transform.position.ToString());
		}

		void Update()
		{
			CSharpScript.EvaluateAsync("Debug.Log(GameObject.FindObjectOfType<Camera>().transform.position.ToString());", options);
		}
	}
}