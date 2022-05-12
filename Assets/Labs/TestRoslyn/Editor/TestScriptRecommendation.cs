using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.Scripting;
using UnityEditor;
using UnityEngine;
namespace Labs.TestRoslyn.Editor
{
	public class TestScriptRecommendation : EditorWindow
	{

		[MenuItem("Labs/Labs.TestRoslyn.Editor/TestScriptRecommendation")]
		static void ShowWindow()
		{
			var window = GetWindow<TestScriptRecommendation>();
			window.titleContent = new("TestScriptRecommendation");
			window.Show();
		}


		MefHostServices host;
		Workspace workspace;
		Assembly[] assemblies;
		ScriptOptions options;
		ScriptState<object> script_state;
		async void OnEnable()
		{
			host = MefHostServices.Create(MefHostServices.DefaultAssemblies);
			workspace = new AdhocWorkspace(host);
			assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var references = new List<MetadataReference>();
			for (int i = 0; i < assemblies.Length; i++)
			{
				if (assemblies[i].IsDynamic)
				{
					Debug.Log($"Dynamic Assembly {assemblies[i].FullName}");
					continue;
				}
				if (assemblies[i].Location == string.Empty)
				{
					Debug.Log($"Empty assembly: {assemblies[i].FullName}");
					continue;
				}
				references.Add(MetadataReference.CreateFromFile(assemblies[i].Location));
			}
			options = ScriptOptions.Default.AddReferences(references);
			script_state = await CSharpScript.RunAsync("using UnityEngine;", options);
			script_state = await script_state.ContinueWithAsync("var a = GameObject.FindObjectOfType<Camera>().transform.position.ToString();Debug.Log(a);", options);

		}

		void OnGUI()
		{

		}
	}
}